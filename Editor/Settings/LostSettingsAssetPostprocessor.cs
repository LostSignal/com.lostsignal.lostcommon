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
        public static string OnGeneratedSlnSolution(string path, string content)
        {
            return LostSettings.Instance.AddEditorConfigToSolution(content);
        }

        public static string OnGeneratedCSProject(string path, string content)
        {
            return AnalyzerUtil.AddAnalyzersToCSProjects(path, content);
        }
    }
}
