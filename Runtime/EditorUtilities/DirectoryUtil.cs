//-----------------------------------------------------------------------
// <copyright file="DirectoryUtil.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#if UNITY_EDITOR

namespace Lost
{
    using System.IO;
    using System.Linq;
    using UnityEditor;
    using UnityEngine;

    public static class DirectoryUtil
    {
        public static void CreateFolder(string path)
        {
            path = path.Replace("\\", "/");
            var directories = path.Split('/');
            Debug.Assert(directories[0] == "Assets");
            string parentDirectory = directories[0];

            for (int i = 1; i < directories.Length; i++)
            {
                string fullDirectory = Path.Combine(parentDirectory, directories[i]).Replace("\\", "/");

                if (AssetDatabase.IsValidFolder(fullDirectory) == false)
                {
                    AssetDatabase.CreateFolder(parentDirectory, directories[i]);
                }

                parentDirectory = fullDirectory;
            }
        }

        public static void DeleteDirectory(string directoryPath)
        {
            foreach (var file in Directory.GetFiles(directoryPath, "*", SearchOption.AllDirectories))
            {
                File.Delete(file);
            }

            var directories = Directory.GetDirectories(directoryPath).ToList();
            directories.Sort();
            directories.Reverse();

            foreach (var directory in directories)
            {
                Directory.Delete(directory);
            }

            Directory.Delete(directoryPath);
        }
    }
}

#endif
