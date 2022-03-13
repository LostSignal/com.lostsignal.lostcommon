//-----------------------------------------------------------------------
// <copyright file="HideLabelPropertyDrawer.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Lost
{
    using UnityEditor;
    using UnityEngine;

    [CustomPropertyDrawer(typeof(HideLabelAttribute))]
    public class HideLabelPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.isExpanded == false)
            {
                property.isExpanded = true;
            }

            EditorGUI.PropertyField(position, property, GUIContent.none);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label);
        }
    }
}
