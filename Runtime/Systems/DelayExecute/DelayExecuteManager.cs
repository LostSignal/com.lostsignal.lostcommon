//-----------------------------------------------------------------------
// <copyright file="DelayExecuteManager.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#if UNITY

namespace Lost
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class DelayExecuteManager : SingletonMonoBehaviour<DelayExecuteManager>, IName, IAwake, IUpdate
    {
        private DelayedActionList delayedActionList = new DelayedActionList("Delay Action List", 30);

#pragma warning disable 0649
        [SerializeField] private int initialCapacity = 50;
#pragma warning restore 0649

        private List<int> idsToDelete = new List<int>(30);
        private int currentId = 0;

        //// private UpdateChannelReceipt updateReceipt;
        //// private DelayedAction[] delayedActions;
        //// private int count;

        public int UpdateOrder => 1;

        public string Name => "Delay Execute Manager";

        //// public override void Initialize()
        //// {
        ////     UpdateManager.OnInitialized += SetupUpdateChannel;
        //// 
        ////     this.delayedActions = new DelayedAction[this.initialCapacity];
        ////     this.count = 0;
        ////     this.SetInstance(this);
        //// 
        ////     void SetupUpdateChannel()
        ////     {
        ////         var updateChannel = UpdateManager.Instance.GetChannel(ChannelName);
        //// 
        ////         if (updateChannel == null)
        ////         {
        ////             Debug.LogError($"{nameof(DelayExecuteManager)} couldn't find Update Channel \"{ChannelName}\".  This manager will not work!", this);
        ////         }
        ////         else
        ////         {
        ////             this.updateReceipt = updateChannel.RegisterCallback(this, this);
        ////         }
        ////     }
        //// }

        public void OnAwake()
        {
            this.delayedActionList.OnDeleteId += (id) =>
            {
                this.idsToDelete.Add(id);
            };
        }

        public void Add(Action action, float seconds)
        {
            int id = this.currentId++;

            this.delayedActionList.Add(
                id,
                new DelayedActionListItem
                {
                    Id = id,
                    Action = action,
                    ExecuteTime = seconds,
                },
                null);

            //// if (this.count >= this.delayedActions.Length)
            //// {
            ////     Debug.LogWarning("DelayExecuteManager had to grow in size at runtime.  Please update initialCapacity to stop this from happening.", this);
            ////     Array.Resize(ref this.delayedActions, this.delayedActions.Length * 2);
            //// }
            //// 
            //// this.delayedActions[this.count++] = new DelayedAction { ExecuteTime = Time.realtimeSinceStartup + seconds, Action = action };
        }

        void IUpdate.OnUpdate(float deltaTime)
        {
            this.delayedActionList.RunAll();

            if (this.idsToDelete.Count > 0)
            {
                for (int i = 0; i < this.idsToDelete.Count; i++)
                {
                    this.delayedActionList.Remove(this.idsToDelete[i]);
                }

                this.idsToDelete.Clear();
            }


            //// float currentTime = Time.realtimeSinceStartup;
            //// int i = 0;
            //// 
            //// while (i < this.count)
            //// {
            ////     if (this.delayedActions[i].ExecuteTime <= currentTime)
            ////     {
            ////         this.delayedActions[i].Action?.Invoke();
            //// 
            ////         int lastIndex = this.count - 1;
            //// 
            ////         if (i != lastIndex)
            ////         {
            ////             this.delayedActions[i] = this.delayedActions[lastIndex];
            ////         }
            //// 
            ////         this.delayedActions[lastIndex] = default;
            //// 
            ////         currentTime = Time.realtimeSinceStartup;
            ////         this.count--;
            ////     }
            ////     else
            ////     {
            ////         i++;
            ////     }
            //// }
        }

        private void OnDestroy()
        {
            //// this.updateReceipt.Cancel();
        }

        private struct DelayedActionListItem
        {
            public int Id;
            public float ExecuteTime;
            public Action Action;
        }

        private class DelayedActionList : ProcessList<DelayedActionListItem>
        {
            public Action<int> OnDeleteId;

            public DelayedActionList(string name, int capacity)
                : base(name, capacity)
            {
            }

            protected override void Process(ref DelayedActionListItem item)
            {
                this.OnDeleteId?.Invoke(item.Id);
            }
        }
    }
}

#endif
