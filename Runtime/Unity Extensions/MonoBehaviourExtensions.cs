//-----------------------------------------------------------------------
// <copyright file="MonoBehaviourExtensions.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#if UNITY

namespace Lost
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public static class MonoBehaviourExtensions
    {
        public static void AssertGetComponent<T>(this MonoBehaviour monoBehaviour, ref T memberVariable)
            where T : Component
        {
            if (monoBehaviour && memberVariable == null)
            {
                memberVariable = monoBehaviour.GetComponent<T>();

                if (memberVariable == null)
                {
                    Debug.LogErrorFormat(monoBehaviour.gameObject, "{0} {1} couldn't find {2} component", monoBehaviour.GetType().Name, GetFullName(monoBehaviour), typeof(T).Name);
                }
                else if (Application.isPlaying)
                {
                    Debug.LogWarningFormat(monoBehaviour.gameObject, "Unneseccasy GetComponent<{0}> call on GameObject {1}.  Should prepopulate this in editor and not at runtime.", typeof(T).Name, GetFullName(monoBehaviour));
                }
                else
                {
#if UNITY_EDITOR
                    UnityEditor.EditorUtility.SetDirty(monoBehaviour);
#endif
                }
            }
        }

        public static void AssertGetComponentInParent<T>(this MonoBehaviour monoBehaviour, ref T memberVariable)
            where T : Component
        {
#if UNITY_EDITOR
            if (monoBehaviour.gameObject.scene.IsValid() == false || UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage() != null)
            {
                return;
            }
#endif

            if (memberVariable == null)
            {
                memberVariable = monoBehaviour.GetComponentInParent<T>();

                if (memberVariable == null)
                {
                    Debug.LogErrorFormat(monoBehaviour.gameObject, "{0} {1} couldn't find {2} component in parent.", monoBehaviour.GetType().Name, GetFullName(monoBehaviour), typeof(T).Name);
                }
                else if (Application.isPlaying)
                {
                    Debug.LogWarningFormat(monoBehaviour.gameObject, "Unneseccasy GetComponentInParent<{0}> call on GameObject {1}.  Should prepopulate this in editor and not at runtime.", typeof(T).Name, GetFullName(monoBehaviour));
                }
                else
                {
#if UNITY_EDITOR
                    UnityEditor.EditorUtility.SetDirty(monoBehaviour);
#endif
                }
            }
        }

        public static void AssertGetComponentInChildren<T>(this MonoBehaviour monoBehaviour, ref T memberVariable)
            where T : Component
        {
#if UNITY_EDITOR
            if (monoBehaviour.gameObject.scene.IsValid() == false || UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage() != null)
            {
                return;
            }
#endif

            if (memberVariable == null)
            {
                memberVariable = monoBehaviour.GetComponentInChildren<T>();

                if (memberVariable == null)
                {
                    Debug.LogErrorFormat(monoBehaviour.gameObject, "{0} {1} couldn't find {2} component in children.", monoBehaviour.GetType().Name, GetFullName(monoBehaviour), typeof(T).Name);
                }
                else if (Application.isPlaying)
                {
                    Debug.LogWarningFormat(monoBehaviour.gameObject, "Unneseccasy GetComponentInChildren<{0}> call on GameObject {1}.  Should prepopulate this in editor and not at runtime.", typeof(T).Name, GetFullName(monoBehaviour));
                }
                else
                {
#if UNITY_EDITOR
                    UnityEditor.EditorUtility.SetDirty(monoBehaviour);
#endif
                }
            }
        }

        public static void LogException(this MonoBehaviour monoBehaviour, Exception ex)
        {
            Debug.LogError($"{monoBehaviour.GetType().Name} \"{GetFullName(monoBehaviour)}\" has encountered an exception {ex.Message}", monoBehaviour);
            Debug.LogException(ex, monoBehaviour);
        }

        public static void AssertNotNull(this MonoBehaviour monoBehaviour, List<ValidationError> errors, object obj, string nameOfObject)
        {
            if (obj == null)
            {
                string message = $"{monoBehaviour.GetType().Name} \"{GetFullName(monoBehaviour)}\" has null object {nameOfObject}";

                if (Application.isPlaying)
                {
                    Debug.LogAssertion(message, monoBehaviour);
                }

                errors?.Add(new ValidationError { AffectedObject = monoBehaviour, Name = message });
            }
        }

        public static void AssertHasValues(this MonoBehaviour monoBehaviour, List<ValidationError> errors, IList collection, string nameOfObject)
        {
            AssertNotNull(monoBehaviour, errors, collection, nameOfObject);

            if (collection.Count == 0)
            {
                string message = $"{monoBehaviour.GetType().Name} \"{GetFullName(monoBehaviour)}\" list {nameOfObject} has no values!";

                if (Application.isPlaying)
                {
                    Debug.LogAssertion(message, monoBehaviour);
                }

                errors?.Add(new ValidationError { AffectedObject = monoBehaviour, Name = message });
            }
            else
            {
                for (int i = 0; i < collection.Count; i++)
                {
                    if (collection[i] == null)
                    {
                        string message = $"{monoBehaviour.GetType().Name} \"{GetFullName(monoBehaviour)}\" list {nameOfObject} has NULL value at index {i}!";

                        if (Application.isPlaying)
                        {
                            Debug.LogAssertion(message, monoBehaviour);
                        }

                        errors?.Add(new ValidationError { AffectedObject = monoBehaviour, Name = message });
                    }
                }
            }
        }

        public static void AssertTrue(this MonoBehaviour monoBehaviour, List<ValidationError> errors, bool obj, string nameOfObject)
        {
            if (obj == false)
            {
                string message = $"{monoBehaviour.GetType().Name} \"{GetFullName(monoBehaviour)}\" has false value {nameOfObject}";

                if (Application.isPlaying)
                {
                    Debug.LogAssertion(message, monoBehaviour);
                }

                errors?.Add(new ValidationError { AffectedObject = monoBehaviour, Name = message });
            }
        }

        public static void AssertFalse(this MonoBehaviour monoBehaviour, List<ValidationError> errors, bool obj, string nameOfObject)
        {
            if (obj == true)
            {
                string message = $"{monoBehaviour.GetType().Name} \"{GetFullName(monoBehaviour)}\" has true value {nameOfObject}";

                if (Application.isPlaying)
                {
                    Debug.LogAssertion(message, monoBehaviour);
                }

                errors?.Add(new ValidationError { AffectedObject = monoBehaviour, Name = message });
            }
        }

        public static void AssertValidString(this MonoBehaviour monoBehaviour, List<ValidationError> errors, string str, string nameOfObject)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                string message = $"{monoBehaviour.GetType().Name} \"{GetFullName(monoBehaviour)}\" has invalid string value {nameOfObject}";

                if (Application.isPlaying)
                {
                    Debug.LogAssertion(message, monoBehaviour);
                }

                errors?.Add(new ValidationError { AffectedObject = monoBehaviour, Name = message });
            }
        }

        public static void DrawGizmoCube(this MonoBehaviour lhs, Color color, float width, float height, Vector2 offset)
        {
#if UNITY_EDITOR
            Gizmos.color = color;
            var parentScale = new Vector2(lhs.gameObject.transform.localToWorldMatrix[0, 0], lhs.gameObject.transform.localToWorldMatrix[1, 1]);
            var localUnits = new Vector2(width, height);
            var worldUnits = new Vector3(localUnits.x * parentScale.x, localUnits.y * parentScale.y, 0);

            Gizmos.DrawWireCube(lhs.gameObject.transform.position + new Vector3(offset.x, offset.y, 0), worldUnits);
#endif
        }

        public static Coroutine ExecuteAtEndOfFrame(this MonoBehaviour lhs, Action action)
        {
            return CoroutineRunner.Instance.StartCoroutine(DelayTillEndOfFrameCoroutine());

            IEnumerator DelayTillEndOfFrameCoroutine()
            {
                yield return WaitForUtil.EndOfFrame;
                action?.Invoke();
            }
        }

        public static Coroutine ExecuteDelayed(this MonoBehaviour lhs, float delayInSeconds, Action action)
        {
            return CoroutineRunner.Instance.StartCoroutine(DelayInSecondsCoroutine());

            IEnumerator DelayInSecondsCoroutine()
            {
                yield return WaitForUtil.Seconds(delayInSeconds);
                action?.Invoke();
            }
        }

        public static Coroutine ExecuteDelayedRealtime(this MonoBehaviour lhs, float delayInRealtimeSeconds, Action action)
        {
            return CoroutineRunner.Instance.StartCoroutine(DelayExecuteRealtimeCoroutine());

            IEnumerator DelayExecuteRealtimeCoroutine()
            {
                yield return WaitForUtil.RealtimeSeconds(delayInRealtimeSeconds);
                action?.Invoke();
            }
        }

        private static string GetFullName(MonoBehaviour monoBehaviour)
        {
            return GetFullName(monoBehaviour.gameObject);
        }

        private static string GetFullName(GameObject gameObject)
        {
            if (gameObject.transform.parent == null)
            {
                return string.Empty;
            }
            else
            {
                string parentName = GetFullName(gameObject.transform.parent.gameObject);

                return string.IsNullOrEmpty(parentName) ? gameObject.name : parentName + "/" + gameObject.name;
            }
        }
    }
}

#endif
