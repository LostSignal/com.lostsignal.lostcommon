//-----------------------------------------------------------------------
// <copyright file="CameraState.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Lost
{
    using UnityEngine;

    public struct CameraState
    {
        public Camera Camera;
        public Transform Transform;
        public Vector3 Position;
        public Vector3 Forward;
        public float FOV;
        public float CosOfFOV;

        public bool IsInView(Vector3 position)
        {
            Vector3 toPosition = (position - this.Position).normalized;
            return Vector3.Dot(this.Forward, toPosition) > this.CosOfFOV;
        }
    }
}
