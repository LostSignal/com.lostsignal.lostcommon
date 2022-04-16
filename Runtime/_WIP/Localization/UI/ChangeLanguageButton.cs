//-----------------------------------------------------------------------
// <copyright file="ChangeLanguageButton.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#if UNITY

namespace Lost.Localization
{
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(Button))]
    public class ChangeLanguageButton : MonoBehaviour
    {
#pragma warning disable 0649
        [ReadOnly]
        [SerializeField] private Button button;
        [SerializeField] private string isoLanguageName;
#pragma warning restore 0649

        private void OnValidate()
        {
            EditorUtil.SetIfNull(this, ref this.button);
        }

        private void Awake()
        {
            this.button.onClick.AddListener(this.Clicked);
        }

        private void Clicked()
        {
            foreach (var language in Localization.GetSupportedLanguages())
            {
                if (language.IsoLanguageName == this.isoLanguageName)
                {
                    Localization.CurrentLanguage = language;
                    return;
                }
            }

            Debug.LogErrorFormat(this, "ChangeLanguage.ChangeLanguageTo couldn't find supported language {0}!", this.isoLanguageName);
        }
    }
}

#endif
