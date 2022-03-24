//-----------------------------------------------------------------------
// <copyright file="DataStoreEnumBindingToggle.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Lost
{
    using UnityEngine;
    using UnityEngine.UI;

    public class DataStoreEnumBindingToggle : MonoBehaviour, IAwake
    {
        #if USING_UGUI

        #pragma warning disable 0649
        [SerializeField] private Toggle toggle;

        [Header("Data Store")]
        [SerializeField] private DataStoreLocation dataStoreLocation;
        [SerializeField] private string dataStoreName;
        [SerializeField] private string dataStoreKey;
        [SerializeField] private int dataStoreEnumIntValue;
        #pragma warning restore 0649

        private DataStore dataStore;

        public void OnAwake()
        {
            this.dataStore = DataStoreManager.Instance.GetDataStore(this.dataStoreLocation, this.dataStoreName);
            this.dataStore.OnDataStoreKeyChanged += this.OnDataStoreKeyChanged;
            this.dataStore.OnDataStoreChanged += this.OnDataStoreChanged;

            this.toggle.onValueChanged.AddListener(this.OnToggleValueChanged);

            this.OnDataStoreChanged();
        }

        private void Awake() => ActivationManager.Register(this);

        private void OnToggleValueChanged(bool newValue)
        {
            if (newValue && this.dataStore != null)
            {
                this.dataStore.SetEnumAsInt(this.dataStoreKey, this.dataStoreEnumIntValue);
            }
        }

        private void OnDataStoreKeyChanged(string key)
        {
            if (this.dataStore != null && this.dataStoreKey == key)
            {
                this.SetToggleValueNoNotify();
            }
        }

        private void OnDataStoreChanged()
        {
            this.SetToggleValueNoNotify();
        }

        private void SetToggleValueNoNotify()
        {
            int enumAsInt = this.dataStore.GetEnumAsInt(this.dataStoreKey);
            this.toggle.SetIsOnWithoutNotify(enumAsInt == this.dataStoreEnumIntValue);
        }

        private void OnDestroy()
        {
            if (this.dataStore != null)
            {
                this.dataStore.OnDataStoreKeyChanged -= this.OnDataStoreKeyChanged;
                this.dataStore.OnDataStoreChanged -= this.OnDataStoreChanged;
            }

            this.toggle.onValueChanged.RemoveListener(this.OnToggleValueChanged);
        }

        #endif
    }
}
