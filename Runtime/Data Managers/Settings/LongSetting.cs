//-----------------------------------------------------------------------
// <copyright file="LongSetting.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Lost
{
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Lost/Settings/Long Setting")]
    public sealed class LongSetting : Setting
    {
        #pragma warning disable 0649
        [SerializeField] private long defaultValue;
        #pragma warning restore 0649

        public long Value
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.DataStore.GetLong(this.name, this.defaultValue);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => this.DataStore.SetLong(this.name, value);
        }
    }
}
