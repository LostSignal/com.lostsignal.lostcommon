//-----------------------------------------------------------------------
// <copyright file="LostCoreSettingsEditor.cs" company="Lost Signal">
//     Copyright (c) Lost Signal. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Lost
{
    using System;
    using System.Collections.Generic;
    using Lost.EditorGrid;
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(LostCoreSettings))]
    public class LostCoreSettingsEditor : Editor
    {
        private const int FoldoutId = 8942147;

        private LostCoreSettings lostLibrarySettings;
        private SerializedObject lostLibrarySerializedObject;

        // Line Endings and Serialization
        private SerializedProperty projectLineEndings;
        private SerializedProperty forceSerializationMode;
        private SerializedProperty serializationMode;

        // Source Control Ignore File
        private SerializedProperty sourceControlType;
        private SerializedProperty ignoreTemplateGit;
        private SerializedProperty ignoreTemplateCollab;
        private SerializedProperty ignoreTemplatePlastic;
        private SerializedProperty ignoreTemplateP4;
        private SerializedProperty p4IgnoreFileName;
        private SerializedProperty autosetP4IgnoreEnvironmentVariable;

        // Editorconfig
        private SerializedProperty useEditorConfig;
        private SerializedProperty editorConfigTemplate;

        // Override Template Files
        private SerializedProperty overrideTemplateFiles;
        private SerializedProperty templateMonoBehaviour;
        private SerializedProperty templatePlayableAsset;
        private SerializedProperty templatePlayableBehaviour;
        private SerializedProperty templateStateMachineBehaviour;
        private SerializedProperty templateSubStateMachineBehaviour;
        private SerializedProperty templateEditorTestScript;

        // PlasticSCM Settings
        private SerializedProperty plasticAutoSetFileCasingError;
        private SerializedProperty plasticAutoSetYamlMergeToolPath;

        // Analyzers
        private SerializedProperty analyzers;

        // Package Mapper Settings
        private SerializedProperty packageMapperRepositories;

        // GUID Fixer
        private SerializedProperty guidFixerSettings;

        public override void OnInspectorGUI()
        {
            this.UpdateSerializedProperties();

            try
            {
                this.DrawProjectSettingsProxies();

                int currentFoldoutId = FoldoutId;
                this.DrawSerializationAndLineSettings(currentFoldoutId++);
                this.DrawIgnoreFiles(currentFoldoutId++);
                this.DrawEditorConfig(currentFoldoutId++);
                this.DrawOverrideTemplateFiles(currentFoldoutId++);
                this.DrawAnalyzers(currentFoldoutId++);
                this.DrawPackageMapperSettings(currentFoldoutId++);
                this.DrawGuidFixerSettings(currentFoldoutId++);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }

            EditorGUILayout.Space(20);

            if (GUILayout.Button("Reset To Defaults"))
            {
                this.lostLibrarySettings.LoadDefaults();
            }

            if (GUI.changed)
            {
                this.lostLibrarySerializedObject.ApplyModifiedProperties();
                this.lostLibrarySettings.Save();
            }
        }

        [SettingsProvider]
        private static SettingsProvider CreateLostLibrarySettingsProvider()
        {
            LostCoreSettings.Instance.name = LostCoreSettings.InstanceName;
            LostCoreSettings.Instance.Load();

            var keywords = GetSearchKeywordsFromSerializedProperties(LostCoreSettings.Instance);
            var provider = AssetSettingsProvider.CreateProviderFromObject(LostCoreSettings.SettingsWindowPath, LostCoreSettings.Instance, keywords);

            provider.inspectorUpdateHandler += () =>
            {
                if (provider.settingsEditor != null && provider.settingsEditor.serializedObject.UpdateIfRequiredOrScript())
                {
                    provider.Repaint();
                }
            };

            return provider;
        }

        private static List<string> GetSearchKeywordsFromSerializedProperties(UnityEngine.Object settingsObject)
        {
            var results = new List<string>();
            var serializedObject = new SerializedObject(settingsObject);
            var property = serializedObject.GetIterator();

            // TODO [bgish]: This returns too much, can I only get properties that belong to the LostLibrarySettings class?
            while (property.Next(true))
            {
                results.AddIfUnique(property.displayName.ToLowerInvariant());
            }

            return results;
        }

        private void UpdateSerializedProperties()
        {
            if (this.lostLibrarySettings == LostCoreSettings.Instance)
            {
                return;
            }

            this.lostLibrarySettings = LostCoreSettings.Instance;
            this.lostLibrarySerializedObject = new SerializedObject(LostCoreSettings.Instance);

            // Line Endings and Serialization
            this.projectLineEndings = this.lostLibrarySerializedObject.FindProperty("projectLineEndings");
            this.forceSerializationMode = this.lostLibrarySerializedObject.FindProperty("forceSerializationMode");
            this.serializationMode = this.lostLibrarySerializedObject.FindProperty("serializationMode");

            // Editorconfig
            this.useEditorConfig = this.lostLibrarySerializedObject.FindProperty("useEditorConfig");
            this.editorConfigTemplate = this.lostLibrarySerializedObject.FindProperty("editorConfigTemplate");

            // Override Template Files
            this.overrideTemplateFiles = this.lostLibrarySerializedObject.FindProperty("overrideTemplateFiles");
            this.templateMonoBehaviour = this.lostLibrarySerializedObject.FindProperty("templateMonoBehaviour");
            this.templatePlayableAsset = this.lostLibrarySerializedObject.FindProperty("templatePlayableAsset");
            this.templatePlayableBehaviour = this.lostLibrarySerializedObject.FindProperty("templatePlayableBehaviour");
            this.templateStateMachineBehaviour = this.lostLibrarySerializedObject.FindProperty("templateStateMachineBehaviour");
            this.templateSubStateMachineBehaviour = this.lostLibrarySerializedObject.FindProperty("templateSubStateMachineBehaviour");
            this.templateEditorTestScript = this.lostLibrarySerializedObject.FindProperty("templateEditorTestScript");

            // PlasticSCM Settings
            this.plasticAutoSetFileCasingError = this.lostLibrarySerializedObject.FindProperty("plasticAutoSetFileCasingError");
            this.plasticAutoSetYamlMergeToolPath = this.lostLibrarySerializedObject.FindProperty("plasticAutoSetYamlMergeToolPath");

            // Source Control Ignore File
            this.sourceControlType = this.lostLibrarySerializedObject.FindProperty("sourceControlType");
            this.ignoreTemplateGit = this.lostLibrarySerializedObject.FindProperty("ignoreTemplateGit");
            this.ignoreTemplateCollab = this.lostLibrarySerializedObject.FindProperty("ignoreTemplateCollab");
            this.ignoreTemplatePlastic = this.lostLibrarySerializedObject.FindProperty("ignoreTemplatePlastic");
            this.ignoreTemplateP4 = this.lostLibrarySerializedObject.FindProperty("ignoreTemplateP4");
            this.p4IgnoreFileName = this.lostLibrarySerializedObject.FindProperty("p4IgnoreFileName");
            this.autosetP4IgnoreEnvironmentVariable = this.lostLibrarySerializedObject.FindProperty("autosetP4IgnoreEnvironmentVariable");

            // Analyzers
            this.analyzers = this.lostLibrarySerializedObject.FindProperty("analyzers");

            // Package Mapper
            this.packageMapperRepositories = this.lostLibrarySerializedObject.FindProperty("packageMapperRepositories");

            // GUID Fixer
            this.guidFixerSettings = this.lostLibrarySerializedObject.FindProperty("guidFixerSettings");
        }

        private void DrawProjectSettingsProxies()
        {
            using (new BoxAreaScope("Project Settings Proxy"))
            {
                int labelWidth = 182;

                using (new IndentLevelScope(1))
                {
                    // Product Name
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        EditorGUILayout.LabelField("Product Name", GUILayout.Width(labelWidth));

                        var productName = EditorGUILayout.TextField(PlayerSettings.productName);

                        if (PlayerSettings.productName != productName)
                        {
                            PlayerSettings.productName = productName;
                        }
                    }

                    // Company Name
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        EditorGUILayout.LabelField("Compnay Name", GUILayout.Width(labelWidth));

                        var companyName = EditorGUILayout.TextField(PlayerSettings.companyName);

                        if (PlayerSettings.companyName != companyName)
                        {
                            PlayerSettings.companyName = companyName;
                        }
                    }

                    // Root Namespace
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        EditorGUILayout.LabelField("Root Namespace", GUILayout.Width(labelWidth));

                        var rootNamespace = EditorGUILayout.TextField(EditorSettings.projectGenerationRootNamespace);

                        if (EditorSettings.projectGenerationRootNamespace != rootNamespace)
                        {
                            EditorSettings.projectGenerationRootNamespace = rootNamespace;
                        }
                    }
                }
            }
        }

        private void DrawSerializationAndLineSettings(int foldoutId)
        {
            using (new FoldoutScope(foldoutId, "Serialization and Line Settings", out bool visible))
            {
                if (visible == false)
                {
                    return;
                }

                using (new IndentLevelScope(1))
                using (new LabelWidthScope(200))
                {
                    EditorGUILayout.PropertyField(this.forceSerializationMode);
                    EditorGUILayout.PropertyField(this.serializationMode);
                    EditorGUILayout.Space(10);
                    EditorGUILayout.PropertyField(this.projectLineEndings);
                }
            }
        }

        private void DrawEditorConfig(int foldoutId)
        {
            using (new FoldoutScope(foldoutId, "Editor Config", out bool visible))
            {
                if (visible == false)
                {
                    return;
                }

                using (new IndentLevelScope(1))
                using (new LabelWidthScope(200))
                {
                    EditorGUILayout.PropertyField(this.useEditorConfig);
                    EditorGUILayout.PropertyField(this.editorConfigTemplate);
                }

                if (this.editorConfigTemplate.objectReferenceValue != null)
                {
                    if (GUILayout.Button("Generate .editorconfig File"))
                    {
                        var path = AssetDatabase.GetAssetPath(this.editorConfigTemplate.objectReferenceValue);
                        var guid = AssetDatabase.AssetPathToGUID(path);

                        MenuItemTools.GenerateFile("editor config", ".editorconfig", guid);
                    }
                }
            }
        }

        private void DrawOverrideTemplateFiles(int foldoutId)
        {
            using (new FoldoutScope(foldoutId, "Override Template Files", out bool visible))
            {
                if (visible == false)
                {
                    return;
                }

                using (new IndentLevelScope(1))
                using (new LabelWidthScope(250))
                {
                    EditorGUILayout.PropertyField(this.overrideTemplateFiles);
                    EditorGUILayout.PropertyField(this.templateMonoBehaviour);
                    EditorGUILayout.PropertyField(this.templatePlayableAsset);
                    EditorGUILayout.PropertyField(this.templatePlayableBehaviour);
                    EditorGUILayout.PropertyField(this.templateStateMachineBehaviour);
                    EditorGUILayout.PropertyField(this.templateSubStateMachineBehaviour);
                    EditorGUILayout.PropertyField(this.templateEditorTestScript);
                }
            }
        }

        private void DrawAnalyzers(int foldoutId)
        {
            using (new FoldoutScope(foldoutId, "Rosyln Analyzers", out bool visible))
            {
                if (visible == false)
                {
                    return;
                }

                using (new IndentLevelScope(2))
                {
                    EditorGUILayout.PropertyField(this.analyzers);
                }
            }
        }

        private void DrawPackageMapperSettings(int foldoutId)
        {
            using (new FoldoutScope(foldoutId, "Package Mapper", out bool visible))
            {
                if (visible == false)
                {
                    return;
                }

                using (new IndentLevelScope(2))
                {
                    EditorGUILayout.PropertyField(this.packageMapperRepositories);
                }
            }
        }

        private void DrawGuidFixerSettings(int foldoutId)
        {
            using (new FoldoutScope(foldoutId, "GUID Fixer", out bool visible))
            {
                if (visible == false)
                {
                    return;
                }

                using (new IndentLevelScope(2))
                {
                    EditorGUILayout.PropertyField(this.guidFixerSettings);
                }
            }
        }

        private void DrawIgnoreFiles(int foldoutId)
        {
            using (new FoldoutScope(foldoutId, "Source Control Ignore File", out bool visible))
            {
                if (visible == false)
                {
                    return;
                }

                using (new IndentLevelScope(1))
                {
                    EditorGUILayout.PropertyField(this.sourceControlType);
                    EditorGUILayout.Space(5);

                    var sourceControl = (LostCoreSettings.SourceControlType)this.sourceControlType.intValue;

                    if (sourceControl == LostCoreSettings.SourceControlType.Git)
                    {
                        EditorGUILayout.PropertyField(this.ignoreTemplateGit);
                    }
                    else if (sourceControl == LostCoreSettings.SourceControlType.Collab)
                    {
                        EditorGUILayout.PropertyField(this.ignoreTemplateCollab);
                    }
                    else if (sourceControl == LostCoreSettings.SourceControlType.Plastic)
                    {
                        EditorGUILayout.PropertyField(this.ignoreTemplatePlastic);
                        EditorGUILayout.PropertyField(this.plasticAutoSetFileCasingError);
                        EditorGUILayout.PropertyField(this.plasticAutoSetYamlMergeToolPath);
                    }
                    else if (sourceControl == LostCoreSettings.SourceControlType.Perforce)
                    {
                        EditorGUILayout.PropertyField(this.ignoreTemplateP4);
                        EditorGUILayout.PropertyField(this.p4IgnoreFileName);
                        EditorGUILayout.PropertyField(this.autosetP4IgnoreEnvironmentVariable);
                    }

                    EditorGUILayout.Space(5);

                    if (GUILayout.Button("Generate Ignore File"))
                    {
                        this.lostLibrarySettings.GenerateSourceControlIgnoreFile();
                    }
                }
            }
        }
    }
}
