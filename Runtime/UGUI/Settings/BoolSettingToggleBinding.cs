//-----------------------------------------------------------------------
// <copyright file="BoolSettingToggleBinding.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Lost
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class BoolSettingToggleBinding : MonoBehaviour, IAwake, IValidate
    {
#pragma warning disable 0649
        [SerializeField] private BoolSetting boolSetting;

        [Header("Toggle Binding Object")]
        [SerializeField] private Toggle toggle;
        [SerializeField] private bool toggleOnValue;
#pragma warning restore 0649

        public void OnAwake()
        {
            if (this.boolSetting == null)
            {
                Debug.LogError($"{nameof(BoolSettingToggleBinding)} has no bool setting set, making it invalid!", this);
                return;
            }

            if (this.toggle == null)
            {
                Debug.LogError($"{nameof(BoolSettingToggleBinding)} has a no binding objects set!", this);
                return;
            }

            this.boolSetting.OnSettingChanged += this.OnSettingChanged;

            if (this.toggle != null)
            {
                this.toggle.onValueChanged.AddListener(this.OnToggleValueChanged);
            }

            this.OnSettingChanged();
        }

        public void Validate(List<ValidationError> errors)
        {
            if (this.boolSetting == null)
            {
                errors.Add(new ValidationError { AffectedObject = this, Name = "Bool Setting is NULL!" });
            }

            if (this.toggle == null)
            {
                errors.Add(new ValidationError { AffectedObject = this, Name = "No Binding Objects Set!" });
            }
        }

        private void Awake() => ActivationManager.Register(this);

        private void OnToggleValueChanged(bool newValue)
        {
            bool boolSettingsValue = newValue ? this.toggleOnValue : !this.toggleOnValue;

            if (this.boolSetting.Value != boolSettingsValue)
            {
                this.boolSetting.Value = boolSettingsValue;
            }
        }

        private void OnSettingChanged()
        {
            if (this.toggle != null)
            {
                this.toggle.SetIsOnWithoutNotify(this.boolSetting.Value == this.toggleOnValue);
            }
        }

        private void OnDestroy()
        {
            if (this.boolSetting == null)
            {
                return;
            }

            this.boolSetting.OnSettingChanged -= this.OnSettingChanged;

            if (this.toggle != null)
            {
                this.toggle.onValueChanged.RemoveListener(this.OnToggleValueChanged);
            }
        }
    }
}
