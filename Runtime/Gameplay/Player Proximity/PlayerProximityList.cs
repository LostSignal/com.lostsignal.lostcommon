//-----------------------------------------------------------------------
// <copyright file="PlayerProximityList.cs" company="Lost Signal">
//     Copyright (c) Lost Signal. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#if UNITY

namespace Lost
{
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public sealed class PlayerProximityList : ProcessList<PlayerProximityItem>
    {
        private Transform mainCameraTransform;
        private Vector3 playerPosition;

        public PlayerProximityList(string name, int capacity)
            : base(name, capacity)
        {
        }

        protected override void OnBeforeProcess()
        {
            if (this.mainCameraTransform == null)
            {
                this.mainCameraTransform = Camera.main.transform;
            }

            this.playerPosition = this.mainCameraTransform.position; // Get this value from the Actor class
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override void Process(ref PlayerProximityItem item)
        {
            if (item.IsDynamic)
            {
                item.WorldToLocal = item.Transform.worldToLocalMatrix;
            }

            bool isInside = item.Area.IsInside(item.WorldToLocal, this.playerPosition);

            if (item.IsInProximity != isInside)
            {
                //// TODO [bgish]: Set the new state, or queue it up?
                item.IsInProximity = isInside;

                if (isInside)
                {
                    item.PlayerProximity.SetInside();
                }
                else
                {
                    item.PlayerProximity.SetOutside();
                }
            }
        }
    }
}

#endif
