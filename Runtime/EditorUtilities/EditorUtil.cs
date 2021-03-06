//-----------------------------------------------------------------------
// <copyright file="EditorUtil.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#if UNITY

namespace Lost
{
    using System.Diagnostics;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public static class EditorUtil
    {
        public static T GetOrAddComponent<T>(MonoBehaviour behaviour)
            where T : Component
        {
            if (Application.isPlaying)
            {
                UnityEngine.Debug.LogError($"EditorUtil.GetOrAddComponent<{typeof(T).Name}>() called at runtime!");
            }

            var component = behaviour.GetComponent<T>();

            if (component)
            {
                return component;
            }

            var result = behaviour.gameObject.AddComponent<T>();

            EditorUtil.SetDirty(behaviour);

            return result;
        }

        public static void SetIfNull<T>(MonoBehaviour monoBehaviour, ref T field, string guid)
            where T : ScriptableObject
        {
            if (Application.isPlaying)
            {
                return;
            }

            if (field == null)
            {
                T result = EditorUtil.GetAssetByGuid<T>(guid);

                if (result != null)
                {
                    field = result;
                    EditorUtil.SetDirty(monoBehaviour);
                }
            }
        }

        public static void SetIfNull<T>(MonoBehaviour monoBehaviour, ref T field, bool ignoreChildren = false)
            where T : Component
        {
            if (Application.isPlaying)
            {
                return;
            }

            if (field == null)
            {
                T result = null;

                if (ignoreChildren)
                {
                    result = monoBehaviour.GetComponent<T>();
                }
                else
                {
                    result = monoBehaviour.GetComponentInChildren<T>();
                }

                if (result != null)
                {
                    field = result;
                    EditorUtil.SetDirty(monoBehaviour);
                }
            }
        }

        public static T GetAssetByGuid<T>(string guid)
            where T : UnityEngine.Object
        {
            if (Application.isEditor == false)
            {
                UnityEngine.Debug.LogError("Trying to call EditorUtil.GetAssetByGuid from a build!");
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

        [Conditional("UNITY_EDITOR")]
        public static void SetDirty(UnityEngine.Object target)
        {
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(target);
#endif
        }

        public static void SaveAll()
        {
            SaveProject();
            SaveScenes();
        }

        public static void SaveProjectAndScene(Component component)
        {
            SaveProject();
            SaveScene(component);
        }

        public static void SaveProjectAndScene(GameObject gameObject)
        {
            SaveProject();
            SaveScene(gameObject);
        }

        public static void SaveProjectAndScene(Scene scene)
        {
            SaveProject();
            SaveScene(scene);
        }

        public static void SaveProject()
        {
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.SaveAssets();
#endif
        }

        public static void SaveScenes()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExecuteMenuItem("File/Save");
#endif
        }

        public static void SaveScene(Component component)
        {
            SaveScene(component.gameObject.scene);
        }

        public static void SaveScene(GameObject gameObject)
        {
            SaveScene(gameObject.scene);
        }

        public static void SaveScene(Scene scene)
        {
#if UNITY_EDITOR
            UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
#endif
        }
    }
}

#endif
