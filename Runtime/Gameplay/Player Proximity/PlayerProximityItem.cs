//-----------------------------------------------------------------------
// <copyright file="PlayerProximityItem.cs" company="Lost Signal">
//     Copyright (c) Lost Signal. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#if UNITY

namespace Lost
{
    using UnityEngine;

    public struct PlayerProximityItem
    {
        public Matrix4x4 WorldToLocal;
        public Area Area;
        public bool IsInProximity;
        public bool IsDynamic;
        public PlayerProximity PlayerProximity;
        public Transform Transform;
    }
}

#endif
