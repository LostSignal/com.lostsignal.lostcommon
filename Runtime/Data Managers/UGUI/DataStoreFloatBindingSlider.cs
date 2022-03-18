//-----------------------------------------------------------------------
// <copyright file="DataStoreFloatBindingSlider.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Lost
{
    using UnityEngine;
    using UnityEngine.UI;

    public class DataStoreFloatBindingSlider : MonoBehaviour, IAwake
    {
        #if USING_UGUI

        #pragma warning disable 0649
        [SerializeField] private Slider slider;
        [SerializeField] private float minValue;
        [SerializeField] private float maxValue;

        [Header("Data Store")]
        [SerializeField] private DataStoreLocation dataStoreLocation;
        [SerializeField] private string dataStoreName;
        [SerializeField] private string dataStoreKey;
        #pragma warning restore 0649

        private DataStore dataStore;

        public void OnAwake()
        {
            this.dataStore = DataStoreManager.Instance.GetDataStore(this.dataStoreLocation, this.dataStoreName);
            this.dataStore.OnDataStoreKeyChanged += this.OnDataStoreKeyChanged;
            this.dataStore.OnDataStoreChanged += this.OnDataStoreChanged;

            this.slider.onValueChanged.AddListener(this.OnSliderValueChanged);
            this.slider.minValue = this.minValue;
            this.slider.maxValue = this.maxValue;

            this.OnDataStoreChanged();
        }

        private void Awake() => ActivationManager.Register(this);

        private void OnSliderValueChanged(float newValue)
        {
            if (this.dataStore != null)
            {
                this.dataStore.SetFloat(this.dataStoreKey, newValue);
            }
        }

        private void OnDataStoreKeyChanged(string key)
        {
            if (this.dataStore != null && this.dataStoreKey == key)
            {
                this.SetSiderValueNoNotify();
            }
        }

        private void OnDataStoreChanged()
        {
            this.SetSiderValueNoNotify();
        }

        private void SetSiderValueNoNotify()
        {
            this.slider.SetValueWithoutNotify(this.dataStore.GetFloat(this.dataStoreKey));
        }

        private void OnDestroy()
        {
            if (this.dataStore != null)
            {
                this.dataStore.OnDataStoreKeyChanged -= this.OnDataStoreKeyChanged;
                this.dataStore.OnDataStoreChanged -= this.OnDataStoreChanged;
            }

            this.slider.onValueChanged.RemoveListener(this.OnSliderValueChanged);
        }

        #endif
    }
}
