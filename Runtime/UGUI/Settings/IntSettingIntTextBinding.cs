//-----------------------------------------------------------------------
// <copyright file="IntSettingIntTextBinding.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Lost
{
    using System.Collections.Generic;
    using UnityEngine;

    public class IntSettingIntTextBinding : MonoBehaviour, IAwake, IValidate
    {
        #if USING_UGUI && USING_TEXT_MESH_PRO

        #pragma warning disable 0649
        [SerializeField] private IntSetting intSetting;

        [Header("Binding Objects")]
        [SerializeField] private IntText intText;
        [SerializeField] private TextUpdateType intTextUpdateType;
        #pragma warning restore 0649

        public void OnAwake()
        {
            if (this.intSetting == null)
            {
                Debug.LogError($"{nameof(IntSettingIntTextBinding)} has no float setting set, making it invalid!", this);
                return;
            }

            if (this.intText == null)
            {
                Debug.LogError($"{nameof(IntSettingIntTextBinding)} has a no binding objects set!", this);
                return;
            }

            this.intSetting.OnSettingChanged += this.OnSettingChanged;

            this.OnSettingChanged();
        }

        public void Validate(List<ValidationError> errors)
        {
            if (this.intSetting == null)
            {
                errors.Add(new ValidationError { AffectedObject = this, Name = "Int Setting is NULL!" });
            }

            if (this.intText == null)
            {
                errors.Add(new ValidationError { AffectedObject = this, Name = "No Binding Objects Set!" });
            }
        }

        private void Awake() => ActivationManager.Register(this);

        private void OnSettingChanged()
        {
            if (this.intText != null)
            {
                this.intText.UpdateValue(this.intSetting.Value, this.intTextUpdateType);
            }
        }

        private void OnDestroy()
        {
            if (this.intSetting == null)
            {
                return;
            }

            this.intSetting.OnSettingChanged -= this.OnSettingChanged;
        }

        #endif
    }
}
