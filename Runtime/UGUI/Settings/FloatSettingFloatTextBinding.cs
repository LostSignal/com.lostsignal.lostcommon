//-----------------------------------------------------------------------
// <copyright file="FloatSettingFloatTextBinding.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Lost
{
    using System.Collections.Generic;
    using UnityEngine;

    public class FloatSettingFloatTextBinding : MonoBehaviour, IAwake, IValidate
    {
#pragma warning disable 0649
        [SerializeField] private FloatSetting floatSetting;

        [Header("Float Text Binding Object")]
        [SerializeField] private FloatText floatText;
        [SerializeField] private TextUpdateType floatTextUpdateType;
#pragma warning restore 0649

        public void OnAwake()
        {
            if (this.floatSetting == null)
            {
                Debug.LogError($"{nameof(FloatSettingFloatTextBinding)} has no float setting set, making it invalid!", this);
                return;
            }

            if (this.floatText == null)
            {
                Debug.LogError($"{nameof(FloatSettingFloatTextBinding)} has a no binding objects set!", this);
                return;
            }

            this.floatSetting.OnSettingChanged += this.OnSettingChanged;

            this.OnSettingChanged();
        }

        public void Validate(List<ValidationError> errors)
        {
            if (this.floatSetting == null)
            {
                errors.Add(new ValidationError { AffectedObject = this, Name = "Float Setting is NULL!" });
            }

            if (this.floatText == null)
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
            if (this.floatText != null)
            {
                this.floatText.UpdateValue(this.floatSetting.Value, this.floatTextUpdateType);
            }
        }

        private void OnDestroy()
        {
            if (this.floatSetting == null)
            {
                return;
            }

            this.floatSetting.OnSettingChanged -= this.OnSettingChanged;
        }
    }
}
