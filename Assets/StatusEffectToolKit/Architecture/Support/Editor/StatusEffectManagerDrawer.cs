/*
 * Description: Draws to the status effect selection box in the tool
 */
#if UNITY_EDITOR
using Service.Framework.StatusSystem;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Support.Editor
{
    [CustomPropertyDrawer(typeof(StatusEffectManager.ConfigFields))]
    public class StatusEffectManagerDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            GUIContent statusEffectSettingsLabel = new GUIContent("Status Effect Selection Settings", label.tooltip);
            property.isExpanded = EditorGUILayout.Foldout(property.isExpanded, statusEffectSettingsLabel, true);

            StatusEffectToolContextHandler.SyncManagerConfigDefaultValues(property);

            if (!property.isExpanded)
            {
                EditorGUI.EndProperty();
                return;
            }

            EditorGUI.indentLevel++;

            //draw a box around the fields for organization
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            IEnumerable<SerializedProperty> props = StatusEffectToolContextHandler.GetEffectSelectionVisibleProperties(property);
            foreach (SerializedProperty prop in props)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(prop.displayName, GUILayout.ExpandWidth(true));

                //if drawing a bool, position the field to the far right.  unique case since bool toggles are tiny
                if (prop.propertyType == SerializedPropertyType.Boolean)
                {
                    EditorGUILayout.PropertyField(prop, GUIContent.none, true, GUILayout.Width(30));
                }
                //auto position other datatype fields
                else
                {
                    EditorGUILayout.PropertyField(prop, GUIContent.none, true, GUILayout.ExpandWidth(true));
                }
                EditorGUILayout.EndHorizontal();
            }
            //close the box
            EditorGUILayout.EndVertical();

            EditorGUI.indentLevel--;
            EditorGUI.EndProperty();
        }
    }
}
#endif