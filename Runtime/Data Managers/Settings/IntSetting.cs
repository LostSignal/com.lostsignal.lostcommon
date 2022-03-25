//-----------------------------------------------------------------------
// <copyright file="IntSetting.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Lost
{
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Lost/Settings/Int Setting")]
    public sealed class IntSetting : Setting
    {
        #pragma warning disable 0649
        [SerializeField] private int defaultValue;
        #pragma warning restore 0649

        public int Value
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.DataStore.GetInt(this.name, this.defaultValue);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => this.DataStore.SetInt(this.name, value);
        }
    }
}
