//-----------------------------------------------------------------------
// <copyright file="LostSettingsLogListener.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Lost
{
    using System.IO;
    using UnityEditor;
    using UnityEngine;

    [InitializeOnLoad]
    public static class LostSettingsLogListener
    {
        static LostSettingsLogListener()
        {
            // Listen for logs about inconsistent line endings
            Application.logMessageReceived += OnLogMessageReceived;
        }

        private static void OnLogMessageReceived(string condition, string stackTrace, LogType type)
        {
            if (Application.isPlaying == false &&
                LostSettings.Instance.AutomaticallyFixLineEndingMismatches &&
                condition.StartsWith("There are inconsistent line endings in the"))
            {
                FixFile(condition);
            }
        }

        private static void FixFile(string condition)
        {
            int startIndex = condition.IndexOf("'") + 1;
            int endIndex = condition.IndexOf("' script. Some are");

            if (startIndex > 1 && endIndex > 0)
            {
                string filePath = condition.Substring(startIndex, endIndex - startIndex);
                string fullFilePath = Path.GetFullPath(filePath).Replace("\\", "/");

                if (fullFilePath.Contains("/PackageCache/") == false)
                {
                    string fileText = File.ReadAllText(fullFilePath);
                    Debug.Log($"Fixed line endings for file {fullFilePath}");
                    FileUtil.UpdateFile(FileUtil.ConvertLineEndings(fileText), fullFilePath, true);
                }
            }
        }
    }
}
