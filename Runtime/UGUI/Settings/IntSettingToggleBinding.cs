//-----------------------------------------------------------------------
// <copyright file="IntSettingToggleBinding.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Lost
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class IntSettingToggleBinding : MonoBehaviour, IAwake, IValidate
    {
#pragma warning disable 0649
        [SerializeField] private IntSetting intSetting;

        [Header("Toggle Binding Object")]
        [SerializeField] private Toggle intToggle;
        [SerializeField] private int intToggleValue;
#pragma warning restore 0649

        public void OnAwake()
        {
            if (this.intSetting == null)
            {
                Debug.LogError($"{nameof(IntSettingToggleBinding)} has no float setting set, making it invalid!", this);
                return;
            }

            if (this.intToggle == null)
            {
                Debug.LogError($"{nameof(IntSettingToggleBinding)} has a no binding objects set!", this);
                return;
            }

            this.intSetting.OnSettingChanged += this.OnSettingChanged;

            if (this.intToggle != null)
            {
                this.intToggle.onValueChanged.AddListener(this.OnToggleValueChanged);
            }

            this.OnSettingChanged();
        }

        public void Validate(List<ValidationError> errors)
        {
            if (this.intSetting == null)
            {
                errors.Add(new ValidationError { AffectedObject = this, Name = "Int Setting is NULL!" });
            }

            if (this.intToggle == null)
            {
                errors.Add(new ValidationError { AffectedObject = this, Name = "No Binding Objects Set!" });
            }
        }

        private void Awake() => ActivationManager.Register(this);

        private void OnToggleValueChanged(bool newValue)
        {
            if (newValue)
            {
                this.intSetting.Value = this.intToggleValue;
            }
        }

        private void OnSettingChanged()
        {
            if (this.intToggle != null)
            {
                this.intToggle.SetIsOnWithoutNotify(this.intSetting.Value == this.intToggleValue);
            }
        }

        private void OnDestroy()
        {
            if (this.intSetting == null)
            {
                return;
            }

            this.intSetting.OnSettingChanged -= this.OnSettingChanged;

            if (this.intToggle != null)
            {
                this.intToggle.onValueChanged.RemoveListener(this.OnToggleValueChanged);
            }
        }
    }
}
