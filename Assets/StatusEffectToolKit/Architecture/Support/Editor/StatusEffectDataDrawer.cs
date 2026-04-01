/*
 * Description: Draws the status effect data on the inspector.  Shared between the tool window and the inspector
 */
#if UNITY_EDITOR
using Service.Framework.StatusSystem;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Support.Editor
{
    [CustomPropertyDrawer(typeof(StatusEffectsData))]
    public class StatusEffectDataDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            float line = EditorGUIUtility.singleLineHeight;
            float space = EditorGUIUtility.standardVerticalSpacing;

            //create the rect for the foldout
            Rect rect = new Rect(position.x, position.y, position.width, line);

            //create the foldout for the header
            property.isExpanded = EditorGUI.Foldout(rect, property.isExpanded, label, true);

            //draw default values
            StatusEffectToolContextHandler.SyncStatusEffectDefaultValues(property);

            //while the foldout is not expanded, do not draw anything else
            if (!property.isExpanded)
            {
                EditorGUI.EndProperty();
                return;
            }
            EditorGUI.indentLevel++;

            //draw the first child field
            rect.y += line + space;

            //determine whether we are drawing to the tool window or the inspector
            IEnumerable<SerializedProperty> props = InspectorDrawContext.IsToolWindow
                ? StatusEffectToolContextHandler.GetStatusEffectVisibleProperties(property)
                : StatusEffectToolContextHandler.GetInspectorProperties(property);

            //draw all those properties
            EditorDrawHelper.DrawProperties(ref rect, space, props);

            EditorGUI.indentLevel--;
            EditorGUI.EndProperty();
        }

        /// <summary>
        /// Calculates the height needed to draw the property
        /// </summary>
        /// <param name="property"></param>
        /// <param name="label"></param>
        /// <returns></returns>
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float line = EditorGUIUtility.singleLineHeight;
            float space = EditorGUIUtility.standardVerticalSpacing;

            //start with the header line
            float height = line;

            StatusEffectToolContextHandler.SyncStatusEffectDefaultValues(property);

            //if not expanded, only return the height for the header line
            if (!property.isExpanded)
            {
                return height;
            }
            //add some space for organization
            height += space;

            //determine whether we are drawing to the tool window or the inspector
            IEnumerable<SerializedProperty> props = InspectorDrawContext.IsToolWindow
               ? StatusEffectToolContextHandler.GetStatusEffectVisibleProperties(property)
               : StatusEffectToolContextHandler.GetInspectorProperties(property);

            //add the height for each of those visible properties
            height = EditorDrawHelper.CalculatePropertiesHeight(height, space, props);
            return height;
        }
    }
}
#endif