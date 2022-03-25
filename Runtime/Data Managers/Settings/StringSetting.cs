//-----------------------------------------------------------------------
// <copyright file="StringSetting.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Lost
{
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Lost/Settings/String Setting")]
    public sealed class StringSetting : Setting
    {
        #pragma warning disable 0649
        [SerializeField] private string defaultValue;
        #pragma warning restore 0649

        public string Value
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.DataStore.GetString(this.name, this.defaultValue);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => this.DataStore.SetString(this.name, value);
        }
    }
}
