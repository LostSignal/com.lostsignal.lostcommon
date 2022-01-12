//-----------------------------------------------------------------------
// <copyright file="PackageMapper.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Lost
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using UnityEditor;
    using UnityEngine;

    public static class PackageMapper
    {
        private const string MapToLocalSource = "Assets/Lost/Package Mapper/Map Package(s) To Local Source";
        private const string MapToGithubSource = "Assets/Lost/Package Mapper/Map Package(s) To Latest GitHub";

        private const string ManifestPath = "./Packages/manifest.json";
        private const string PackagesPath = "Packages/";

        public enum Mode
        {
            None,
            GitHub,
            LocalFolder,
        }

        [MenuItem(MapToLocalSource, false, priority = -100)]
        public static void UpdateToLocalSource()
        {
            foreach (var selectedObject in Selection.objects)
            {
                SwitchRepositoryTo(selectedObject, Mode.LocalFolder);
            }
        }

        [MenuItem(MapToLocalSource, true, priority = -100)]
        public static bool UpdateToLocalSourceValidate()
        {
            return IsSelectionAllPackages();
        }

        [MenuItem(MapToGithubSource, false, priority = -100)]
        public static void UpdateToLatestGitHub()
        {
            foreach (var selectedObject in Selection.objects)
            {
                SwitchRepositoryTo(selectedObject, Mode.GitHub);
            }
        }

        [MenuItem(MapToGithubSource, true, priority = -100)]
        public static bool UpdateToLatestGitHubValidate()
        {
            return IsSelectionAllPackages();
        }

        public static string GetPackageLocalPath(string packageName, List<string> packagesDirectories)
        {
            // Going through each one and moving the files if found
            foreach (var packagesDirectory in packagesDirectories)
            {
                if (Directory.Exists(packagesDirectory))
                {
                    return packagesDirectory.Replace("\\", "/");
                }
            }

            return null;
        }

        private static bool IsSelectionAllPackages()
        {
            if (Selection.objects == null || Selection.objects.Length == 0)
            {
                return false;
            }

            foreach (var selectedObject in Selection.objects)
            {
                GetRepository(selectedObject, out PackageMapperRepository repository, out string _, out Mode _);

                if (repository == null)
                {
                    return false;
                }
            }

            return true;
        }

        private static void GetRepository(UnityEngine.Object selectedObject, out PackageMapperRepository repository, out string packageLocalPath, out Mode currentMode)
        {
            repository = null;
            packageLocalPath = null;
            currentMode = Mode.None;

            var packageMapperRepositories = LostCoreSettings.Instance.PackageMapperRepositories;

            string packageName = GetPackageName(selectedObject);

            if (string.IsNullOrEmpty(packageName))
            {
                return;
            }

            foreach (var repo in packageMapperRepositories)
            {
                if (repo.PackageIdentifier == packageName)
                {
                    repository = repo;
                }
            }

            if (repository == null)
            {
                Debug.LogError($"Unable to find Package Identifier {packageName} in \".packageutil\" config file.");
                return;
            }

            packageLocalPath = GetPackageLocalPath(repository.PackageIdentifier, repository.LocalSourceDirectories);

            foreach (var line in File.ReadAllLines(ManifestPath))
            {
                if (line.Contains($"\"{repository.PackageIdentifier}\""))
                {
                    currentMode = line.Contains("file:") ? Mode.LocalFolder : Mode.GitHub;
                }
            }
        }

        private static void SwitchRepositoryTo(UnityEngine.Object selectedObject, Mode newMode)
        {
            GetRepository(selectedObject, out PackageMapperRepository repository, out string packageLocalPath, out Mode _);

            StringBuilder newFileContents = new StringBuilder();
            foreach (var line in File.ReadAllLines(ManifestPath))
            {
                if (line.Contains($"\"{repository.PackageIdentifier}\""))
                {
                    int colonIndex = line.IndexOf(":");
                    newFileContents.Append(line.Substring(0, colonIndex + 1));

                    if (newMode == Mode.LocalFolder)
                    {
                        newFileContents.AppendLine($" \"file:{packageLocalPath}\",");
                    }
                    else
                    {
                        newFileContents.AppendLine($" \"{repository.GitHubUrl}#{GetLastestGitHash(packageLocalPath)}\",");
                    }
                }
                else
                {
                    newFileContents.AppendLine(line);
                }
            }

            File.WriteAllText(ManifestPath, newFileContents.ToString());
            UnityEditor.PackageManager.Client.Resolve();
        }

        private static string GetLastestGitHash(string gitPath)
        {
            var gitProcess = new System.Diagnostics.Process
            {
                StartInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "git",
                    Arguments = "log",
                    WorkingDirectory = gitPath,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                },
            };

            gitProcess.Start();
            while (gitProcess.StandardOutput.EndOfStream == false)
            {
                return gitProcess.StandardOutput.ReadLine()
                    .Replace("commit ", string.Empty)
                    .Substring(0, 40);
            }

            return null;
        }

        private static string GetPackageName(UnityEngine.Object selectedObject)
        {
            string path = AssetDatabase.GetAssetPath(selectedObject);

            if (path.StartsWith(PackagesPath) && Directory.Exists(path) && path.IndexOf("/") == path.LastIndexOf("/"))
            {
                return path.Substring(PackagesPath.Length);
            }

            return null;
        }
    }
}
