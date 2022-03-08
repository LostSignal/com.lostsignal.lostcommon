//-----------------------------------------------------------------------
// <copyright file="ManagersReady.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#if UNITY

namespace Lost
{
    using System.Collections;
    using System.Collections.Generic;

    public class ManagersReady : Manager<ManagersReady>
    {
        public static readonly ObjectTracker<IManager> Managers = new ObjectTracker<IManager>(50);
        private static readonly List<IOnManagersReady> OnManagersReadyQueue = new List<IOnManagersReady>(200);

        private bool areManagersReady;
        private bool isWaitForManagersRunning;
        
        static ManagersReady()
        {
            Platform.OnReset += Reset;

            static void Reset()
            {
                OnManagersReadyQueue.Clear();
            }
        }

        public bool IsProcessing => false;

        public static void Register(IOnManagersReady action)
        {
            if (IsInitialized && Instance.areManagersReady)
            {
                action.OnManagersReady();
            }
            else
            {
                OnManagersReadyQueue.Add(action);
            }
        }

        public static T FindManagerOfType<T>() where T : class
        {
            for (int i = 0; i < Managers.Count; i++)
            {
                if (Managers[i] is T t)
                {
                    return t;
                }
            }

            return null;
        }

        public override void Initialize()
        {
            this.SetInstance(this);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            Managers.OnItemsChanged += this.Managers_OnItemsChanged;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            Managers.OnItemsChanged -= this.Managers_OnItemsChanged;
        }

        private void Managers_OnItemsChanged(object sender, System.EventArgs e)
        {
            this.WaitForManagers();
        }

        private void WaitForManagers()
        {
            if (this.isWaitForManagersRunning || Platform.IsApplicationQuitting)
            {
                return;
            }

            this.isWaitForManagersRunning = true;
            this.areManagersReady = false;
            CoroutineRunner.Instance.StartCoroutine(Coroutine());

            IEnumerator Coroutine()
            {
                // Wait a few ticks for managers to register themselves
                yield return null;
                yield return null;
                yield return null;

                // Waiting for all the managers to initialize
                while (true)
                {
                    bool areAllManagersInitialized = true;

                    for (int i = 0; i < Managers.Count; i++)
                    {
                        if (Managers[i].IsManagerInitialized() == false)
                        {
                            areAllManagersInitialized = false;
                            break;
                        }
                    }

                    if (areAllManagersInitialized)
                    {
                        break;
                    }

                    yield return WaitForUtil.Seconds(0.1f);
                }

                // Queue up all the items in TempQueue so they can all be processed.
                for (int i = 0; i < OnManagersReadyQueue.Count; i++)
                {
                    // TODO [bgish]: Check for null and catch exceptions
                    OnManagersReadyQueue[i].OnManagersReady();
                }

                OnManagersReadyQueue.Clear();

                this.areManagersReady = true;
                this.isWaitForManagersRunning = false;
            }
        }
    }
}

#endif
