
/*
Description: Draws to the status level box in the tool
*/

#if UNITY_EDITOR
using Service.Framework.Tools;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Support.Editor
{
    [CustomPropertyDrawer(typeof(StatusEffectSettings.StatusLevelConfig))]
    public class StatusLevelConfigDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            float line = EditorGUIUtility.singleLineHeight;
            float space = EditorGUIUtility.standardVerticalSpacing;

            Rect rect = new Rect(position.x, position.y, position.width, line);

            property.isExpanded = EditorGUI.Foldout(rect, property.isExpanded, label, true);

            //draw default values
            StatusEffectToolContextHandler.SyncStatusEffectDefaultValues(property);

            if (!property.isExpanded)
            {
                EditorGUI.EndProperty();
                return;
            }

            EditorGUI.indentLevel++;
            rect.y += line + space;

            IEnumerable<SerializedProperty> props = StatusEffectToolContextHandler.GetStatusLevelSettingsVisibleProperties(property);

            EditorDrawHelper.DrawProperties(ref rect, space, props);

            SerializedProperty statusEffects = property.FindPropertyRelative(nameof(StatusEffectSettings.StatusLevelConfig.statusEffectsData));

            if (statusEffects != null)
            {
                EditorDrawHelper.CalculatePropertyHeight(ref rect, space, statusEffects);
            }
            EditorGUI.indentLevel--;
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float line = EditorGUIUtility.singleLineHeight;
            float space = EditorGUIUtility.standardVerticalSpacing;
            float height = line;

            //draw default values
            StatusEffectToolContextHandler.SyncStatusEffectDefaultValues(property);

            if (!property.isExpanded)
            {
                return height;
            }
            height += space;

            IEnumerable<SerializedProperty> props = StatusEffectToolContextHandler.GetStatusLevelSettingsVisibleProperties(property);

            height = EditorDrawHelper.CalculatePropertiesHeight(height, space, props);

            SerializedProperty statusEffects = property.FindPropertyRelative(nameof(StatusEffectSettings.StatusLevelConfig.statusEffectsData));
            if (statusEffects != null)
            {
                height += EditorGUI.GetPropertyHeight(statusEffects, true) + space;
            }
            return height;
        }
    }
}
#endif