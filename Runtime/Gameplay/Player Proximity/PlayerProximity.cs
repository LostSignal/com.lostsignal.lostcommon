//-----------------------------------------------------------------------
// <copyright file="PlayerProximity.cs" company="Lost Signal">
//     Copyright (c) Lost Signal. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#if UNITY

namespace Lost
{
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.Events;

    public class PlayerProximity : MonoBehaviour, IAwake
    {
        #pragma warning disable 0649
        [ReadOnly] [SerializeField] private Transform proximityTransform;
        [SerializeField] private Area area;
        [SerializeField] private bool isDynamic;
        [SerializeField] private UnityEvent inside;
        [SerializeField] private UnityEvent outside;
        #pragma warning restore 0649

        public Transform ProximityTransform
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.proximityTransform;
        }

        public Area Area
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.area;
        }

        public bool IsDynamic
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.isDynamic;
        }

        public void OnAwake()
        {
            PlayerProximityManager.Instance.Register(this);
        }

        public void SetInside()
        {
            this.inside.SafeInvoke();
        }

        public void SetOutside()
        {
            this.outside.SafeInvoke();
        }

        private void Awake() => ActivationManager.Register(this);

        private void OnDestroy()
        {
            if (PlayerProximityManager.Instance != null)
            {
                PlayerProximityManager.Instance.Unregister(this);
            }
        }

        private void OnValidate()
        {
            if (this.proximityTransform == null)
            {
                this.proximityTransform = this.transform;
                EditorUtil.SetDirty(this.gameObject);
            }
        }

        #if UNITY_EDITOR

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.white.SetA(0.2f);

            if (this.area.Type == AreaType.Sphere)
            {
                Gizmos.DrawWireSphere(this.transform.position, this.area.Size.x * this.transform.lossyScale.x);
            }
            else if (this.area.Type == AreaType.Cylinder)
            {
                GizmosUtil.DrawWireCylinder(this.transform, this.area.Size.x, this.area.Size.y);
            }
            else if (this.area.Type == AreaType.Box)
            {
                GizmosUtil.DrawWireCube(this.transform, this.area.Size);
            }
        }

        #endif
    }
}

#endif
