#pragma warning disable

//-----------------------------------------------------------------------
// <copyright file="ProjectDefinesHelper.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#if UNITY

namespace Lost
{
    using System;
    using System.Collections.Generic;

    #if UNITY_EDITOR
    using System.Linq;
    #endif

    public static class ProjectDefinesHelper
    {
        public static void AddDefineToProject(string defineToAdd)
        {
            #if UNITY_EDITOR

            var buildTargetGroups = (UnityEditor.BuildTargetGroup[])Enum.GetValues(typeof(UnityEditor.BuildTargetGroup));

            foreach (var buildTargetGroup in buildTargetGroups)
            {
                string currentDefinesString = UnityEditor.PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
                string newDefinesString = GetNewDefinesString(buildTargetGroup);

                if (currentDefinesString != newDefinesString)
                {
                    UnityEditor.PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, newDefinesString);
                }
            }

            string GetNewDefinesString(UnityEditor.BuildTargetGroup buildTargetGroup)
            {
                var currentDefines = UnityEditor.PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup).Split(';').ToList();
                currentDefines.AddIfUnique(defineToAdd);
                return string.Join(";", currentDefines);
            }

            UnityEditor.AssetDatabase.SaveAssets();

            #endif
        }
    }
}

#endif
