
/*
Description: Helper class to better organize repeat code
*/

#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Support.Editor
{
    public static class EditorDrawHelper
    {
        /// <summary>
        /// Draw multiple properties
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="space"></param>
        /// <param name="props"></param>
        public static void DrawProperties(ref Rect rect, float space, IEnumerable<SerializedProperty> props)
        {
            foreach (SerializedProperty prop in props)
            {
                if (prop == null)
                {
                    continue;
                }

                float height = EditorGUI.GetPropertyHeight(prop, true);
                rect.height = height;

                EditorGUI.PropertyField(rect, prop, true);
                rect.y += height + space;
            }
        }

        /// <summary>
        /// Calculate multiple properties' height
        /// </summary>
        /// <param name="baseHeight"></param>
        /// <param name="space"></param>
        /// <param name="props"></param>
        /// <returns></returns>
        public static float CalculatePropertiesHeight(float baseHeight, float space, IEnumerable<SerializedProperty> props)
        {
            float height = baseHeight;

            foreach (SerializedProperty prop in props)
            {
                if (prop == null)
                {
                    continue;
                }
                height += EditorGUI.GetPropertyHeight(prop, true) + space;
            }
            return height;
        }

        /// <summary>
        /// Calculate a single property's height
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="space"></param>
        /// <param name="prop"></param>
        public static void CalculatePropertyHeight(ref Rect rect, float space, SerializedProperty prop)
        {
            if (prop == null)
            {
                return;
            }

            float height = EditorGUI.GetPropertyHeight(prop, true);
            rect.height = height;

            EditorGUI.PropertyField(rect, prop, true);
            rect.y += height + space;
        }
    }
}
#endif