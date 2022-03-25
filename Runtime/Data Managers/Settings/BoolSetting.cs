//-----------------------------------------------------------------------
// <copyright file="BoolSetting.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Lost
{
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Lost/Settings/Bool Setting")]
    public sealed class BoolSetting : Setting
    {
        #pragma warning disable 0649
        [SerializeField] private bool defaultValue;
        #pragma warning restore 0649

        public bool Value
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.DataStore.GetBool(this.name, this.defaultValue);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => this.DataStore.SetBool(this.name, value);
        }
    }
}
