//-----------------------------------------------------------------------
// <copyright file="LostSettingsAssetPostprocessor.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Lost
{
    using UnityEditor;

    public class LostSettingsAssetPostprocessor : AssetPostprocessor
    {
        private static void OnGeneratedCSProjectFiles()
        {
            LostSettings.Instance.AddEditorConfigToSolution();

            AnalyzerUtil.AddAnalyzersToCSProjects();
        }
    }
}
