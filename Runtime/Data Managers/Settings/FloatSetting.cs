//-----------------------------------------------------------------------
// <copyright file="FloatSetting.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Lost
{
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Lost/Settings/Float Setting")]
    public sealed class FloatSetting : Setting
    {
        #pragma warning disable 0649
        [SerializeField] private float defaultValue;
        [SerializeField] private float minValue = float.MinValue;
        [SerializeField] private float maxValue = float.MaxValue;
        #pragma warning restore 0649

        public float Value
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.DataStore.GetFloat(this.name, this.defaultValue);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => this.DataStore.SetFloat(this.name, value);
        }

        public float MinValue
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.minValue;
        }

        public float MaxValue
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.maxValue;
        }
    }
}
