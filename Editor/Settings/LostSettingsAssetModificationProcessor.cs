//-----------------------------------------------------------------------
// <copyright file="LostSettingsAssetModificationProcessor.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Lost
{
    using UnityEditor;

    public class LostSettingsAssetModificationProcessor : AssetModificationProcessor
    {
        public static void OnWillCreateAsset(string assetPath)
        {
            LostSettings.Instance.OverrideCSharpTemplateFiles(assetPath);
        }
    }
}
