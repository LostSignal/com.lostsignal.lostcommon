//-----------------------------------------------------------------------
// <copyright file="LostLibrary.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Lost
{
    using System.IO;
    using UnityEditor;

    public static class LostLibrary
    {
        private static readonly string LostLibraryAssetsPath = "Assets/Editor/com.lostsignal.lostlibrary";

        public static T CreateScriptableObject<T>(string guid, string assetName)
            where T : UnityEngine.Object
        {
            string assetPath = GetAssetPath(assetName);

            if (System.IO.File.Exists(assetPath) == false)
            {
                AssetDatabase.CopyAsset(AssetDatabase.GUIDToAssetPath(guid), assetPath);
            }

            return AssetDatabase.LoadAssetAtPath<T>(assetPath);
        }

        public static string GetAssetPath(string assetName)
        {
            // Making sure EditorAppConfig path exists
            string assetPath = Path.Combine(LostLibraryAssetsPath, assetName);
            string assetDirectory = Path.GetDirectoryName(assetPath);

            if (Directory.Exists(assetDirectory) == false)
            {
                Directory.CreateDirectory(assetDirectory);
            }

            return assetPath;
        }
    }
}
