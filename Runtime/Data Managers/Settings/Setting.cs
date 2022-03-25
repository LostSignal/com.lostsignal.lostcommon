//-----------------------------------------------------------------------
// <copyright file="Setting.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Lost
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class Setting : ScriptableObject
    {
        #pragma warning disable 0649
        [SerializeField] private DataStoreObject dataStore;
        [SerializeField] [ReadOnly] private int id;
        #pragma warning restore 0649

        private Action onSettingChanged;

        public event Action OnSettingChanged
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            add => this.onSettingChanged += value;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            remove => this.onSettingChanged -= value;
        }

        protected DataStore DataStore
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.dataStore.DataStore;
        }

        protected int Id
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.id;
        }

        protected virtual void OnValidate()
        {
            #if UNITY_EDITOR
            var path = UnityEditor.AssetDatabase.GetAssetPath(this);
            var guid = UnityEditor.AssetDatabase.GUIDFromAssetPath(path);
            int hash = Math.Abs(guid.GetHashCode());

            if (this.id != hash)
            {
                this.id = hash;
                EditorUtil.SetDirty(this);
            }
            #endif
        }

        private void OnEnable()
        {
            if (this.dataStore != null)
            {
                this.dataStore.DataStore.OnDataStoreChanged += this.DataStoreChanged;
                this.dataStore.DataStore.OnDataStoreKeyChanged += this.DataStoreKeyChanged;
            }
        }

        private void OnDisable()
        {
            if (this.dataStore != null)
            {
                this.dataStore.DataStore.OnDataStoreChanged -= this.DataStoreChanged;
                this.dataStore.DataStore.OnDataStoreKeyChanged -= this.DataStoreKeyChanged;
            }
        }

        private void DataStoreKeyChanged(string obj)
        {
            this.onSettingChanged.SafeInvoke();
        }

        private void DataStoreChanged()
        {
            this.onSettingChanged.SafeInvoke();
        }
    }
}
