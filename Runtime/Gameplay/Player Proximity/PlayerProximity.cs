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

    public class PlayerProximity : MonoBehaviour, IAwake
    {
        #pragma warning disable 0649
        [SerializeField] private Area area;
        [SerializeField] private bool isDynamic;
        #pragma warning restore 0649

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

        private void Awake() => ActivationManager.Register(this);

        public void OnAwake()
        {
            PlayerProximityManager.Instance.Register(this);
        }

        private void OnDestroy()
        {
            PlayerProximityManager.Instance.Unregister(this);
        }
    }
}

#endif
