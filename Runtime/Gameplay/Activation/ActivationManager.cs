//-----------------------------------------------------------------------
// <copyright file="ActivationManager.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Lost
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [DefaultExecutionOrder(-1000)]
    public class ActivationManager : MonoBehaviour, ILevelLoadPreprocessor
    {
        private static ActivationManager instance;
        private static bool isInitialized;
        private static bool isProcessing;

        #pragma warning disable 0649
        [SerializeField] private double maxActivationTimeInMillis = 0.1;
        #pragma warning restore 0649

        private Queue<MonoBehaviour> monoBehaviours = new Queue<MonoBehaviour>(1000);
        private Queue<IAwake> awakes = new Queue<IAwake>(1000);
        private Queue<IStart> starts = new Queue<IStart>(1000);

        private Queue<IUpdate> updates = new Queue<IUpdate>(100);
        private Queue<ILateUpdate> lateUpdates = new Queue<ILateUpdate>(100);
        private Queue<IFixedUpdate> fixedUpdates = new Queue<IFixedUpdate>(100);

        private List<IUpdate> runningUpdates = new List<IUpdate>(100);
        private List<ILateUpdate> runningLateUpdates = new List<ILateUpdate>(100);
        private List<IFixedUpdate> runningFixedUpdates = new List<IFixedUpdate>(100);

        public bool IsProcessing => isProcessing;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Register(MonoBehaviour monoBehaviour)
        {
            isProcessing = true;
            (isInitialized ? instance : Initialize()).monoBehaviours.Enqueue(monoBehaviour);
        }

        private static ActivationManager Initialize()
        {
            isInitialized = true;
            instance = SingletonUtil.CreateSingleton<ActivationManager>("Activation Manager");
            return instance;
        }

        private void Awake()
        {
            if (Application.isEditor || Debug.isDebugBuild)
            {
                this.awakes.OnGrow += () => Debug.LogWarning("ActivationManager Awake Queue Grew!");
                this.starts.OnGrow += () => Debug.LogWarning("ActivationManager Start Queue Grew!");
                this.updates.OnGrow += () => Debug.LogWarning("ActivationManager Update Queue Grew!");
                this.lateUpdates.OnGrow += () => Debug.LogWarning("ActivationManager LateUpdate Queue Grew!");
                this.fixedUpdates.OnGrow += () => Debug.LogWarning("ActivationManager FixedUpdate Queue Grew!");
            }

            LevelManager.Instance.AddLevelLoadPreprocessor(this);
        }

        private void OnDestroy()
        {
            if (LevelManager.Instance)
            {
                LevelManager.Instance.RemoveLevelLoadPreprocessor(this);
            }
        }

        private void Update()
        {
            //// TODO [bgish]: If ManagersReady == false, early out

            if (isProcessing)
            {
                this.ProcessActivationRequests();
            }

            float deltaTime = Time.deltaTime;

            for (int i = 0; i < this.runningUpdates.Count; i++)
            {
                //// if this is a bad reference, then remove it (and swap with last one in the list)
                //// if not enabled, then skip it
                //// keep stats on worst offenders?

                this.runningUpdates[i].OnUpdate(deltaTime);
            }
        }

        private void LateUpdate()
        {
            float deltaTime = Time.deltaTime;

            for (int i = 0; i < this.runningLateUpdates.Count; i++)
            {
                //// if this is a bad reference, then remove it (and swap with last one in the list)
                //// if not enabled, then skip it
                //// keep stats on worst offenders?

                this.runningLateUpdates[i].OnLateUpdate(deltaTime);
            }
        }

        private void FixedUpdate()
        {
            float fixedDeltaTime = Time.fixedDeltaTime;

            for (int i = 0; i < this.runningFixedUpdates.Count; i++)
            {
                //// if this is a bad reference, then remove it (and swap with last one in the list)
                //// if not enabled, then skip it
                //// keep stats on worst offenders?

                this.runningFixedUpdates[i].OnFixedUpdate(fixedDeltaTime);
            }
        }

        private void ProcessActivationRequests()
        {
            //// TODO [bgish]: Need to handle any exception being thrown

            var startTime = Time.realtimeSinceStartupAsDouble;
            var endTime = startTime + (this.maxActivationTimeInMillis / 1000.0);
            var currentTime = startTime;

            while (currentTime < endTime)
            {
                if (this.monoBehaviours.Count > 0)
                {
                    ProcessMonobehaviours();
                }
                else if (this.awakes.Count > 0)
                {
                    var awake = this.awakes.Dequeue();

                    if (awake != null)
                    {
                        awake.OnAwake();
                    }
                }
                else if (this.starts.Count > 0)
                {
                    var start = this.starts.Dequeue();

                    if (start != null)
                    {
                        start.OnStart();
                    }
                }
                else if (this.updates.Count > 0)
                {
                    var update = this.updates.Dequeue();

                    if (update != null)
                    {
                        this.runningUpdates.Add(update);
                    }
                }
                else if (this.lateUpdates.Count > 0)
                {
                    var lateUpdate = this.lateUpdates.Dequeue();

                    if (lateUpdate != null)
                    {
                        this.runningLateUpdates.Add(lateUpdate);
                    }
                }
                else if (this.fixedUpdates.Count > 0)
                {
                    var fixedUpdate = this.fixedUpdates.Dequeue();

                    if (fixedUpdate != null)
                    {
                        this.runningFixedUpdates.Add(fixedUpdate);
                    }
                }
                else
                {
                    isProcessing = false;
                    break;
                }

                currentTime = Time.realtimeSinceStartupAsDouble;
            }

            void ProcessMonobehaviours()
            {
                if (this.monoBehaviours.Count > 0)
                {
                    var monoBehaviour = this.monoBehaviours.Dequeue();

                    if (monoBehaviour == null)
                    {
                        return;
                    }

                    if (monoBehaviour is IAwake awake)
                    {
                        this.awakes.Enqueue(awake);
                    }

                    if (monoBehaviour is IStart start)
                    {
                        this.starts.Enqueue(start);
                    }

                    if (monoBehaviour is IUpdate update)
                    {
                        this.updates.Enqueue(update);
                    }

                    if (monoBehaviour is ILateUpdate lateUpdate)
                    {
                        this.lateUpdates.Enqueue(lateUpdate);
                    }

                    if (monoBehaviour is IFixedUpdate fixedUpdate)
                    {
                        this.fixedUpdates.Enqueue(fixedUpdate);
                    }
                }
            }
        }
    }
}
