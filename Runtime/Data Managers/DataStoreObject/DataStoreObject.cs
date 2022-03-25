//-----------------------------------------------------------------------
// <copyright file="DataStoreObject.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Lost
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public abstract class DataStoreObject : ScriptableObject
    {
#pragma warning disable 0649
        [SerializeField] [ReadOnly] private int id;
        [SerializeField] private bool saveOnUnload;
#pragma warning restore 0649

        private DataStore dataStore = new DataStore();

        public int Id
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.id;
        }

        public DataStore DataStore
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.dataStore;
        }

        public abstract void Load();

        public abstract void Save();

        protected virtual void OnEnable()
        {
            this.Load();
        }

        protected virtual void OnDisable()
        {
            if (this.saveOnUnload)
            {
                this.Save();
            }
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
    }
}
