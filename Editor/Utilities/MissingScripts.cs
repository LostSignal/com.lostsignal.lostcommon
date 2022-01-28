//-----------------------------------------------------------------------
// <copyright file="MissingScripts.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Lost
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public static class MissingScripts
    {
        private static readonly List<Component> ComponentsCache = new List<Component>();

        [MenuItem("Tools/Lost/Find All Bad Components In Project", priority = 44)]
        public static void FindAllBadComponentsInProjectMenuItem()
        {
            var log = new StringBuilder();

            foreach (var badPrefab in FindAllProjectPrefabsWithMissingScripts().Where(x => PackageCacheUtil.IsInPackageCache(x) == false))
            {
                var children = badPrefab.GetChildrenRecursively();
                bool isRootAffected = DoesGameObjectHaveBadComponent(badPrefab);
                bool areChildrenAffected = children.Any(x => DoesGameObjectHaveBadComponent(x));

                if (isRootAffected && areChildrenAffected == false)
                {
                    log.AppendLine($"Prefab {AssetDatabase.GetAssetPath(badPrefab)} has bad component on root game object.");
                }
                else
                {
                    log.AppendLine($"Prefab {AssetDatabase.GetAssetPath(badPrefab)} has bad component on the following game objects:");
                    
                    // Checking root 
                    if (isRootAffected)
                    {
                        Append(badPrefab);
                    }

                    // Checking Children
                    foreach (var child in children.Where(x => DoesGameObjectHaveBadComponent(x)))
                    {
                        Append(child);
                    }
                }

                Debug.Log(log, badPrefab);
                log.Clear();
            }

            void Append(GameObject gameObject)
            {
                log.Append("    ");
                log.AppendLine(gameObject.GetFullName());
            }

            static bool DoesGameObjectHaveBadComponent(GameObject gameObject)
            {
                ComponentsCache.Clear();
                gameObject.GetComponents(ComponentsCache);
                return ComponentsCache.Any(x => x == null);
            }
        }

        [MenuItem("Tools/Lost/Find All Bad Components In Scene", priority = 45)]
        public static void FindAllBadComponentsInActiveSceneMenuItem()
        {
            foreach (var gameObject in FindAllGameObjectsInSceneWithMissingScripts())
            {
                Debug.Log($"GameObject {gameObject.GetFullName()} has bad component.", gameObject);
            }
        }

        public static IEnumerable<GameObject> FindAllProjectPrefabsWithMissingScripts()
        {
            return AssetDatabaseUtil.GetAllProjectPrefabs()
                .Where(x => x.GetComponentsInChildren<Component>(true).Contains(null));
        }

        public static IEnumerable<GameObject> FindAllGameObjectsInSceneWithMissingScripts()
        {
            return GameObject.FindObjectsOfType<GameObject>(true)
                .Where(x => x.GetComponents<Component>().Contains(null));
        }

        public static IEnumerable<GameObject> FindAllGameObjectsInSceneWithMissingScripts(Scene scene)
        {
            return FindAllGameObjectsInSceneWithMissingScripts()
                .Where(x => x.scene == scene);
        }
    }
}
