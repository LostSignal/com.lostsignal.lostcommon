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
    using UnityEditor;
    #endif

    public static class ProjectDefinesHelper
    {
        private static readonly HashSet<int> BuildTargetsToIgnore = new HashSet<int>
        {
            0,  // Unknown
            2,  // WebPlayer
            5,  // PS3
            6,  // XBOX360
            15, // WP8
            16, // BlackBerry
            17, // Tizen
            18, // PSP2
            20, // PSM
            22, // SamsungTV
            23, // N3DS
            24, // WiiU
            26, // Facebook
        };

        public static void AddDefineToProject(string defineToAdd)
        {
            #if UNITY_EDITOR

            var buildTargetGroups = (BuildTargetGroup[])Enum.GetValues(typeof(BuildTargetGroup));

            foreach (var buildTargetGroup in buildTargetGroups)
            {
                if (BuildTargetsToIgnore.Contains((int)buildTargetGroup))
                {
                    continue;
                }

                string currentDefinesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
                string newDefinesString = GetNewDefinesString(buildTargetGroup);

                if (currentDefinesString != newDefinesString)
                {
                    PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, newDefinesString);
                }
            }

            EditorApplication.ExecuteMenuItem("File/Save Project");

            string GetNewDefinesString(BuildTargetGroup buildTargetGroup)
            {
                var defines = GetBuildTargetDefines(buildTargetGroup);
                defines.AddIfUnique(defineToAdd);
                return string.Join(";", defines);
            }

            #endif
        }

        public static List<string> GetBuildTargetDefines(BuildTargetGroup buildTargetGroup)
        {
            var result = new List<string>();

            #if UNITY_EDITOR
            
            var currentDefinesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);

            if (string.IsNullOrWhiteSpace(currentDefinesString))
            {
                // Do nothing, there are no defines
            }
            else if (currentDefinesString.Contains(';') == false)
            {
                result.Add(currentDefinesString);
            }
            else
            {
                result.AddRange(currentDefinesString
                    .Split(';')
                    .Where(x => string.IsNullOrWhiteSpace(x) == false)
                    .Select(x => x.Trim()));
            }

            #endif

            return result;
        }
    }
}

#endif
