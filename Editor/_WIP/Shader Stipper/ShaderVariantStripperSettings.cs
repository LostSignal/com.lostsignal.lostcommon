//-----------------------------------------------------------------------
// <copyright file="ShaderVariantStripperSettings.cs" company="DefaultCompany">
//     Copyright (c) DefaultCompany. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#if UNITY_EDITOR

namespace Lost
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.Rendering;

    [Serializable]
    public class LostShaderVariantStripperSettings
    {
        #pragma warning disable 0649
        [SerializeField]
        private bool enableShaderStipping = false;

        [SerializeField]
        private bool isForwardRednderer = true;

        [SerializeField]
        private List<DefaultAsset> approvedFolders = new List<DefaultAsset>();

        [SerializeField]
        private List<string> essentialShadersStartWith = new List<string>
        {
            "Hidden/",
            "Particles/",
            "Unlit/",
            "TextMeshPro/",
        };

        [SerializeField]
        private List<string> essentialShadersEqual = new List<string>
        {
            "Sprites/Default",
            "Sprites/Mask",
            "UI/Default",
            "Skybox/Procedural",
            "Legacy Shaders/Diffuse",        // Non URP
            "Legacy Shaders/VertexLit",      // Non URP
            "Universal Render Pipeline/Lit",
        };

        [SerializeField]
        private List<string> varientsToSkip = new List<string>
        {
            // "DIRECTIONAL_COOKIE",
            // "POINT_COOKIE",
            // "LIGHTPROBE_SH",
        };
        #pragma warning restore 0649

        public bool EnableShaderStipping => this.enableShaderStipping;

        public bool IsForwardRenderer => this.isForwardRednderer;

        public IEnumerable<string> ApprovedFolders
        {
            get => this.approvedFolders
                .Select(x => AssetDatabase.GetAssetPath(x))
                .Where(x => x != null && Directory.Exists(x));
        }

        public List<string> EssentialShadersStartWith => this.essentialShadersStartWith;

        public List<string> EssentialShadersEqual => this.essentialShadersEqual;

        public IEnumerable<ShaderKeyword> VarientsToSkip
        {
            get => this.varientsToSkip.Select(x => new ShaderKeyword(x));
        }
    }
}

#endif
