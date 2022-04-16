//-----------------------------------------------------------------------
// <copyright file="LostEditor.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Lost
{
    using System;
    using System.Collections.Generic;
    using Lost.EditorGrid;
    using UnityEditor;

    public class LostEditor : Editor
    {
        private Dictionary<string, SerializedProperty> serializedProperties = new();

        protected virtual void OnEnable() => this.serializedProperties.Clear();

        protected virtual void OnDisable() => this.serializedProperties.Clear();

        protected void DrawProperty(string propertyName)
        {
            if (this.serializedProperties.TryGetValue(propertyName, out SerializedProperty prop) == false)
            {
                prop = this.serializedObject.FindProperty(propertyName);
                this.serializedProperties.Add(propertyName, prop);
            }

            EditorGUILayout.PropertyField(prop);
        }

        protected void Foldout(string name, Action action)
        {
            int id = HashCode.Combine(this.target.GetInstanceID(), name);

            using (new FoldoutScope(id, name, out bool visible))
            {
                if (visible)
                {
                    action.Invoke();
                }
            }
        }
    }
}
