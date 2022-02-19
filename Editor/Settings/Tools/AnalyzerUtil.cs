//-----------------------------------------------------------------------
// <copyright file="AnalyzerUtil.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Lost
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using UnityEditor;

    public static class AnalyzerUtil
    {
        public static string AddAnalyzersToCSProjects(string csProjFilePath, string csprojFileContents)
        {
            var fileName = Path.GetFileNameWithoutExtension(csProjFilePath);
            var analyzers = LostSettings.Instance.Analyzers;

            if (analyzers?.Count > 0)
            {
                for (int i = 0; i < analyzers.Count; i++)
                {
                    var analyzer = analyzers[i];

                    if (analyzer.CSProjects.Contains(fileName))
                    {
                        csprojFileContents = AddAnalyzerToCSProj(csprojFileContents, analyzer, i);
                    }
                }
            }

            return csprojFileContents;
        }

        private static string AddAnalyzerToCSProj(string contents, Analyzer analyzer, int analyzerIndex)
        {
            var additionalFiles = new List<string>();
            var ruleSets = new List<string>();
            var analyzers = new List<string>();

            if (analyzer.Config != null)
            {
                additionalFiles.Add(FullPath(analyzer.Config));
            }

            if (analyzer.Ruleset != null)
            {
                ruleSets.Add(FullPath(analyzer.Ruleset));
            }

            if (analyzer.DLLs != null)
            {
                for (int i = 0; i < analyzer.DLLs.Count; i++)
                {
                    if (analyzer.DLLs[i] != null)
                    {
                        analyzers.Add(CreateDLL(analyzer.DLLs[i]));
                    }
                }
            }

            return AnalyzerUtil.UpdateCSProjFile(contents, additionalFiles, ruleSets, analyzers);

            string CreateDLL(UnityEngine.TextAsset dllAsset)
            {
                var sourceFilePath = FullPath(dllAsset);
                var sourceFileBytes = File.ReadAllBytes(sourceFilePath);

                string dllDirectory = $"./Library/Analyzers/{analyzerIndex}_{analyzer.Name}";

                if (Directory.Exists(dllDirectory) == false)
                {
                    Directory.CreateDirectory(dllDirectory);
                }

                string dllName = Path.GetFileNameWithoutExtension(sourceFilePath);
                string fullDllPath = Path.GetFullPath(Path.Combine(dllDirectory, dllName));

                if (File.Exists(fullDllPath))
                {
                    File.Delete(fullDllPath);
                }

                File.WriteAllBytes(fullDllPath, sourceFileBytes);

                return fullDllPath;
            }

            string FullPath(UnityEngine.Object obj) => Path.GetFullPath(AssetDatabase.GetAssetPath(obj));
        }

        private static string UpdateCSProjFile(string csprojFileContents, List<string> additionalFiles, List<string> rulesetFiles, List<string> analyzerDlls)
        {
            var lines = csprojFileContents.Replace("\r\n", "\n").Split("\n").ToList();

            AddRulesets(rulesetFiles, lines);
            AddAnalyzerDLLs(analyzerDlls, lines);
            AddAdditionalFiles(additionalFiles, lines);

            return GetFileContents(lines);

            void AddRulesets(List<string> files, List<string> lines)
            {
                if (files == null || files.Count == 0)
                {
                    return;
                }

                int rulesetIndex = GetLineIndex("<CodeAnalysisRuleSet>", lines);
                int firstItemGroupIndex = GetLineIndex("<ItemGroup>", lines);
                var newLines = new List<string>();
                var newLinesAdded = false;

                if (rulesetIndex == -1)
                {
                    newLines.Add("  <PropertyGroup>");
                }

                foreach (var file in files)
                {
                    string newLine = $"   <CodeAnalysisRuleSet>{file}</CodeAnalysisRuleSet>";

                    if (lines.Contains(newLine) == false)
                    {
                        newLinesAdded = true;
                        newLines.Add(newLine);
                    }
                }

                if (rulesetIndex == -1)
                {
                    newLines.Add("  </PropertyGroup>");
                }

                if (newLinesAdded)
                {
                    lines.InsertRange(rulesetIndex != -1 ? rulesetIndex + 1 : firstItemGroupIndex, newLines);
                }
            }

            void AddAdditionalFiles(List<string> files, List<string> lines)
            {
                if (files == null || files.Count == 0)
                {
                    return;
                }

                int additionalFilesIndex = GetLineIndex("<AdditionalFiles ", lines);
                int firstItemGroupIndex = GetLineIndex("<ItemGroup>", lines);
                var newLines = new List<string>();
                var newLinesAdded = false;

                if (additionalFilesIndex == -1)
                {
                    newLines.Add("  <ItemGroup>");
                }

                foreach (var file in files)
                {
                    string newLine = $"    <AdditionalFiles Include=\"{file}\" />";

                    if (lines.Contains(newLine) == false)
                    {
                        newLinesAdded = true;
                        newLines.Add(newLine);
                    }
                }

                if (additionalFilesIndex == -1)
                {
                    newLines.Add("  </ItemGroup>");
                }

                if (newLinesAdded)
                {
                    lines.InsertRange(additionalFilesIndex != -1 ? additionalFilesIndex + 1 : firstItemGroupIndex, newLines);
                }
            }

            void AddAnalyzerDLLs(List<string> files, List<string> lines)
            {
                if (files == null || files.Count == 0)
                {
                    return;
                }

                int analyzerFilesIndex = GetLineIndex("<Analyzer ", lines);
                int firstItemGroupIndex = GetLineIndex("<ItemGroup>", lines);
                var newLines = new List<string>();
                var newLinesAdded = false;

                if (analyzerFilesIndex == -1)
                {
                    newLines.Add("  <ItemGroup>");
                }

                foreach (var file in files)
                {
                    string newLine = $"    <Analyzer Include=\"{file}\" />";

                    if (lines.Contains(newLine) == false)
                    {
                        newLinesAdded = true;
                        newLines.Add(newLine);
                    }
                }

                if (analyzerFilesIndex == -1)
                {
                    newLines.Add("  </ItemGroup>");
                }

                if (newLinesAdded)
                {
                    lines.InsertRange(analyzerFilesIndex != -1 ? analyzerFilesIndex + 1 : firstItemGroupIndex, newLines);
                }
            }

            int GetLineIndex(string startsWith, List<string> lines)
            {
                for (int i = 0; i < lines.Count; i++)
                {
                    if (lines[i] != null && lines[i].Trim().StartsWith(startsWith))
                    {
                        return i;
                    }
                }

                return -1;
            }

            string GetFileContents(List<string> lines)
            {
                var result = new StringBuilder();

                foreach (var line in lines)
                {
                    result.AppendLine(line);
                }

                return result.ToString();
            }
        }
    }
}
