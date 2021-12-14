//-----------------------------------------------------------------------
// <copyright file="EditorUtil.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#if UNITY

namespace Lost
{
    using UnityEngine;

    public static class EditorUtil
    {
        public static T GetAssetByGuid<T>(string guid)
            where T : UnityEngine.Object
        {
            if (Application.isEditor == false)
            {
                Debug.LogError("Trying to call EditorUtil.GetAssetByGuid from a build!");
            }
            else
            {
                #if UNITY_EDITOR
                if (string.IsNullOrEmpty(guid))
                {
                    return null;
                }

                string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);

                if (string.IsNullOrEmpty(path))
                {
                    return null;
                }

                return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
                #endif
            }

            return null;
        }
    }
}

#endif
