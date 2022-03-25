//-----------------------------------------------------------------------
// <copyright file="DoubleSetting.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Lost
{
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Lost/Settings/Double Setting")]
    public sealed class DoubleSetting : Setting
    {
        #pragma warning disable 0649
        [SerializeField] private double defaultValue;
        #pragma warning restore 0649

        public double Value
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.DataStore.GetDouble(this.name, this.defaultValue);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => this.DataStore.SetDouble(this.name, value);
        }
    }
}
