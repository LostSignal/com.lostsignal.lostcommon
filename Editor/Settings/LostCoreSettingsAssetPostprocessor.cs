//-----------------------------------------------------------------------
// <copyright file="LostCoreSettingsAssetPostprocessor.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Lost
{
    using UnityEditor;

    public class LostCoreSettingsAssetPostprocessor : AssetPostprocessor
    {
        private static void OnGeneratedCSProjectFiles()
        {
            LostCoreSettings.Instance.AddEditorConfigToSolution();

            AnalyzerUtil.AddAnalyzersToCSProjects();
        }
    }
}
