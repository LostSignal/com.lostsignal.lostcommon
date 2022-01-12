//-----------------------------------------------------------------------
// <copyright file="NewBehaviourScript.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Lost
{
    using System.IO;
    using System.Text;
    using UnityEditor;
    using UnityEditor.VersionControl;
    using UnityEngine;

    public static class MenuItemTools
    {
        [MenuItem("Tools/Lost/Actions/Disable Warnings In C# Files (Selected Directory)", priority = 13)]
        public static void DisableWarnings()
        {
            ConvertAllCSharpFiles(true);
            ConvertAllCSharpFiles(false);
        }

        [MenuItem("Tools/Lost/Actions/Cleanup C# Files (Selected Directory)", priority = 12)]
        public static void ConvertAllCSharpFiles()
        {
            ConvertAllCSharpFiles(false);
        }

        public static void ConvertAllCSharpFiles(bool disableWarnings)
        {
            string path = ".";

            // If a directory is selected, then only convert the things under the directory
            if (Selection.activeObject != null)
            {
                string rootDirectoryAssetPath = AssetDatabase.GetAssetPath(Selection.activeObject.GetInstanceID());

                if (Directory.Exists(rootDirectoryAssetPath))
                {
                    path = rootDirectoryAssetPath;
                }
            }

            foreach (string file in Directory.GetFiles(path, "*.cs", SearchOption.AllDirectories))
            {
                StringBuilder fileBuilder = new StringBuilder();

                string fileContents = FileUtil.ConvertLineEndings(File.ReadAllText(file), LineEndingsMode.Unix);
                string[] lines = fileContents.Split('\n');
                int lastLineIndex = lines.Length - 1;

                // Calculating the line of the file that actually has content
                while (lastLineIndex > 0 && string.IsNullOrEmpty(lines[lastLineIndex].Trim()))
                {
                    lastLineIndex--;
                }

                if (disableWarnings)
                {
                    fileBuilder.Append("#pragma warning disable\n\n");
                }

                // Converting each line to our project standards
                for (int i = 0; i <= lastLineIndex; i++)
                {
                    string convertedLine = lines[i];

                    convertedLine = convertedLine.TrimEnd();              // trim_trailing_whitespace = true
                    convertedLine = convertedLine.Replace("\t", "    ");  // indent_style = space, indent_size = 4
                    convertedLine += '\n';                                // insert_final_newline = true, end_of_line = lf

                    fileBuilder.Append(convertedLine);
                }

                // Generating the final file bytes
                MemoryStream finalFileContents = new MemoryStream();
                var utf8Encoder = new UTF8Encoding(true);
                var preamble = utf8Encoder.GetPreamble();

                if (preamble != null && preamble.Length > 0)
                {
                    finalFileContents.Write(preamble, 0, preamble.Length);
                }

                byte[] contents = utf8Encoder.GetBytes(FileUtil.ConvertLineEndings(fileBuilder.ToString()));
                finalFileContents.Write(contents, 0, contents.Length);

                byte[] finalFileBytes = finalFileContents.ToArray();

                // Checking if the file needs to be saved out
                if (AreByteArraysEqual(File.ReadAllBytes(file), finalFileBytes) == false)
                {
                    Debug.Log("Updating File " + file);

                    var asset = file.Replace("\\", "/").Replace("./", string.Empty);

                    if (asset.StartsWith("Packages/") == false)
                    {
                        Provider.Checkout(asset, CheckoutMode.Asset).Wait();
                    }

                    File.WriteAllBytes(file, finalFileBytes);
                }
            }
        }

        [MenuItem("Tools/Lost/Actions/Warnings As Errors/Create Warnings As Errors File", priority = 1)]
        public static void GenerateWarngingsAsErrorsFile()
        {
            string mcsFilePath = "Assets/mcs.rsp";
            string warningsAsErrors = "-warnaserror+";

            if (File.Exists(mcsFilePath))
            {
                StringBuilder fileContents = new StringBuilder();
                fileContents.AppendLine(warningsAsErrors);

                foreach (var line in File.ReadAllLines(mcsFilePath))
                {
                    if (line.Trim() == warningsAsErrors)
                    {
                        return; // no need to generate the file since the warnings as error command exists
                    }
                    else
                    {
                        fileContents.AppendLine(line);
                    }
                }

                // checking out the file
                if (Provider.enabled && Provider.isActive)
                {
                    Provider.Checkout(mcsFilePath, CheckoutMode.Asset).Wait();
                }

                try
                {
                    File.WriteAllText(mcsFilePath, fileContents.ToString());
                }
                catch
                {
                    Debug.LogErrorFormat("Unable to update file {0}.  Is it read only?", mcsFilePath);
                }
            }
            else
            {
                FileUtil.CreateFile(warningsAsErrors, mcsFilePath, true);
            }
        }

        [MenuItem("Tools/Lost/Actions/Warnings As Errors/Remove Warnings As Errors File", priority = 1)]
        public static void RemoveWarngingsAsErrorsFile()
        {
            string mcsFilePath = "Assets/mcs.rsp";
            string warningsAsErrors = "-warnaserror+";

            if (File.Exists(mcsFilePath))
            {
                StringBuilder fileContents = new StringBuilder();

                foreach (var line in File.ReadAllLines(mcsFilePath))
                {
                    // Skip the warnings as errors line
                    if (line.Trim() == warningsAsErrors)
                    {
                        continue;
                    }

                    fileContents.AppendLine(line);
                }

                // If the file is empty, then we should just delete it
                bool shouldDeleteFile = string.IsNullOrEmpty(fileContents.ToString().Trim());

                if (Provider.enabled && Provider.isActive)
                {
                    if (shouldDeleteFile)
                    {
                        Provider.Delete(mcsFilePath).Wait();
                    }
                    else
                    {
                        Provider.Checkout(mcsFilePath, CheckoutMode.Asset).Wait();
                    }
                }

                try
                {
                    if (shouldDeleteFile)
                    {
                        // Making sure it still exists (Provider.Delete might have already deleted it)
                        if (File.Exists(mcsFilePath))
                        {
                            File.Delete(mcsFilePath);
                        }
                    }
                    else
                    {
                        File.WriteAllText(mcsFilePath, fileContents.ToString());
                    }
                }
                catch
                {
                    Debug.LogErrorFormat("Unable to update file {0}.  Is it read only?", mcsFilePath);
                }

                AssetDatabase.Refresh();
            }
        }

        [MenuItem("Tools/Lost/Actions/Remove Empty Directories")]
        public static void RemoveEmptyDirectories()
        {
            FileUtil.RemoveEmptyDirectories("Assets");
            EditorApplication.delayCall += AssetDatabase.Refresh;
        }

        [MenuItem("Tools/Lost/Actions/Generate Files/.gitignore")]
        public static void GenerateGitIgnoreFile()
        {
            GenerateFile("gitignore", ".gitignore", "fae63426d3cf11c4cb39244488e2ec17");
        }

        [MenuItem("Tools/Lost/Actions/Generate Files/.p4ignore")]
        public static void GenerateP4IgnoreFile()
        {
            GenerateFile("p4ignore", ".p4ignore", "6d6c8d3e6aeaff34d89c7f2be0a80a0d");
        }

        [MenuItem("Tools/Lost/Actions/Generate Files/.editorconfig")]
        public static void GenerateEditorConfigFile()
        {
            GenerateFile("editor config", ".editorconfig", "f6c774b1ff43524428c88bc6afaca2d7");
        }

        public static void GenerateFile(string displayName, string filePath, string guid)
        {
            var textAsset = EditorUtil.GetAssetByGuid<TextAsset>(guid);

            if (textAsset != null)
            {
                if (File.Exists(filePath) == false)
                {
                    FileUtil.CreateFile(textAsset.text, filePath, false);
                }
                else
                {
                    try
                    {
                        FileUtil.CopyFile(textAsset.text, filePath, false);
                    }
                    catch
                    {
                        Debug.LogErrorFormat("Unable to update {0} file. Is it read only?", displayName);
                    }
                }
            }
            else
            {
                Debug.LogErrorFormat("Unable to find {0} asset file!", displayName);
            }
        }

        private static bool AreByteArraysEqual(byte[] array1, byte[] array2)
        {
            if (array1.Length != array2.Length)
            {
                return false;
            }

            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
