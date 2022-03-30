//-----------------------------------------------------------------------
// <copyright file="StringSettingTextBinding.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Lost
{
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;

    public class StringSettingTextBinding : MonoBehaviour, IAwake, IValidate
    {
#pragma warning disable 0649
        [SerializeField] private StringSetting stringSetting;

        [Header("Binding Objects")]
        [SerializeField] private TMP_Text text;
#pragma warning restore 0649

        public void OnAwake()
        {
            if (this.stringSetting == null)
            {
                Debug.LogError($"{nameof(StringSettingTextBinding)} has no string setting set, making it invalid!", this);
                return;
            }

            if (this.text == null)
            {
                Debug.LogError($"{nameof(StringSettingTextBinding)} has a no binding objects set!", this);
                return;
            }

            this.stringSetting.OnSettingChanged += this.OnSettingChanged;

            this.OnSettingChanged();
        }

        public void Validate(List<ValidationError> errors)
        {
            if (this.stringSetting == null)
            {
                errors.Add(new ValidationError { AffectedObject = this, Name = "Int Setting is NULL!" });
            }

            if (this.text == null)
            {
                errors.Add(new ValidationError { AffectedObject = this, Name = "No Binding Objects Set!" });
            }
        }

        private void Awake() => ActivationManager.Register(this);

        private void OnSettingChanged()
        {
            if (this.text != null)
            {
                BetterStringBuilder.New().Append(this.stringSetting.Value).Set(this.text);
            }
        }

        private void OnDestroy()
        {
            if (this.stringSetting == null)
            {
                return;
            }

            this.stringSetting.OnSettingChanged -= this.OnSettingChanged;
        }
    }
}
