//-----------------------------------------------------------------------
// <copyright file="FloatSettingSliderBinding.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Lost
{
    using System.Collections.Generic;
    using UnityEngine;

    #if USING_UGUI && USING_TEXT_MESH_PRO
    using UnityEngine.UI;
    #endif

    public class FloatSettingSliderBinding : MonoBehaviour, IAwake, IValidate
    {
        #if USING_UGUI && USING_TEXT_MESH_PRO

        #pragma warning disable 0649
        [SerializeField] private FloatSetting floatSetting;

        [Header("Slider Binding Object")]
        [SerializeField] private Slider floatSlider;
        #pragma warning restore 0649

        public void OnAwake()
        {
            if (this.floatSetting == null)
            {
                Debug.LogError($"{nameof(FloatSettingSliderBinding)} has no float setting set, making it invalid!", this);
                return;
            }

            if (this.floatSlider == null)
            {
                Debug.LogError($"{nameof(FloatSettingSliderBinding)} has a no binding objects set!", this);
                return;
            }

            this.floatSetting.OnSettingChanged += this.OnSettingChanged;

            if (this.floatSlider != null)
            {
                this.floatSlider.onValueChanged.AddListener(this.OnSliderValueChanged);
                this.floatSlider.minValue = this.floatSetting.MinValue;
                this.floatSlider.maxValue = this.floatSetting.MaxValue;
            }

            this.OnSettingChanged();
        }

        public void Validate(List<ValidationError> errors)
        {
            if (this.floatSetting == null)
            {
                errors.Add(new ValidationError { AffectedObject = this, Name = "Float Setting is NULL!" });
            }

            if (this.floatSlider == null)
            {
                errors.Add(new ValidationError { AffectedObject = this, Name = "No Binding Objects Set!" });
            }
        }

        private void Awake() => ActivationManager.Register(this);

        private void OnSliderValueChanged(float newValue)
        {
            if (this.floatSetting.Value != newValue)
            {
                this.floatSetting.Value = newValue;
            }
        }

        private void OnSettingChanged()
        {
            if (this.floatSlider != null)
            {
                this.floatSlider.SetValueWithoutNotify(this.floatSetting.Value);
            }
        }

        private void OnDestroy()
        {
            if (this.floatSetting == null)
            {
                return;
            }

            this.floatSetting.OnSettingChanged -= this.OnSettingChanged;

            if (this.floatSlider != null)
            {
                this.floatSlider.onValueChanged.RemoveListener(this.OnSliderValueChanged);
            }
        }

        #endif
    }
}
