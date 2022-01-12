//-----------------------------------------------------------------------
// <copyright file="LostCoreSettingsAssetModificationProcessor.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Lost
{
    using UnityEditor;

    public class LostCoreSettingsAssetModificationProcessor : AssetModificationProcessor
    {
        public static void OnWillCreateAsset(string assetPath)
        {
            LostCoreSettings.Instance.OverrideCSharpTemplateFiles(assetPath);
        }
    }
}
