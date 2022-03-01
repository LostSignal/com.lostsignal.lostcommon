//-----------------------------------------------------------------------
// <copyright file="CameraManager.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Lost
{
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public sealed class CameraManager : SingletonMonoBehaviour<CameraManager>, IName, IAwake, IUpdate
    {
        private CameraState cameraState;
        
        public string Name => "Camera Manager";

        public CameraState CameraState
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.cameraState;
        }

        public int UpdateOrder => -1;

        public void OnAwake()
        {
            this.OnUpdate(0.0f);
        }

        public void OnUpdate(float deltaTime)
        {
            if (this.cameraState.Camera == null)
            {
                var camera = Camera.main;
                var cameraTransform = camera.transform;
                var fov = camera.fieldOfView;

                this.cameraState = new CameraState
                {
                    Camera = camera,
                    Transform = cameraTransform,
                    Position = cameraTransform.position,
                    Forward = cameraTransform.forward,
                    FOV = fov,
                    CosOfFOV = Mathf.Cos(fov * Mathf.Deg2Rad),
                };
            }
            else
            {
                // The camera is still valid, so lets just update the camera postion/forward
                this.cameraState.Position = this.cameraState.Transform.position;
                this.cameraState.Forward = this.cameraState.Transform.forward;
            }
        }

        private void Awake() => ActivationManager.Register(this);
    }
}
