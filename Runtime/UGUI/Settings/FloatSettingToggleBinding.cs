//-----------------------------------------------------------------------
// <copyright file="FloatSettingToggleBinding.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Lost
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class FloatSettingToggleBinding : MonoBehaviour, IAwake, IValidate
    {
        #if USING_UGUI && USING_TEXT_MESH_PRO

        #pragma warning disable 0649
        [SerializeField] private FloatSetting floatSetting;

        [Header("Toggle Binding Object")]
        [SerializeField] private Toggle floatToggle;
        [SerializeField] private float floatToggleValue;
        #pragma warning restore 0649

        public void OnAwake()
        {
            if (this.floatSetting == null)
            {
                Debug.LogError($"{nameof(FloatSettingToggleBinding)} has no float setting set, making it invalid!", this);
                return;
            }

            if (this.floatToggle == null)
            {
                Debug.LogError($"{nameof(FloatSettingToggleBinding)} has a no binding objects set!", this);
                return;
            }

            this.floatSetting.OnSettingChanged += this.OnSettingChanged;

            if (this.floatToggle != null)
            {
                this.floatToggle.onValueChanged.AddListener(this.OnToggleValueChanged);
            }

            this.OnSettingChanged();
        }

        public void Validate(List<ValidationError> errors)
        {
            if (this.floatSetting == null)
            {
                errors.Add(new ValidationError { AffectedObject = this, Name = "Float Setting is NULL!" });
            }

            if (this.floatToggle == null)
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

        private void OnToggleValueChanged(bool newValue)
        {
            if (newValue)
            {
                this.floatSetting.Value = this.floatToggleValue;
            }
        }

        private void OnSettingChanged()
        {
            if (this.floatToggle != null)
            {
                bool isEqual = Mathf.Abs(this.floatSetting.Value - this.floatToggleValue) < 0.001f;
                this.floatToggle.SetIsOnWithoutNotify(isEqual);
            }
        }

        private void OnDestroy()
        {
            if (this.floatSetting == null)
            {
                return;
            }

            this.floatSetting.OnSettingChanged -= this.OnSettingChanged;

            if (this.floatToggle != null)
            {
                this.floatToggle.onValueChanged.RemoveListener(this.OnToggleValueChanged);
            }
        }

        #endif
    }
}
