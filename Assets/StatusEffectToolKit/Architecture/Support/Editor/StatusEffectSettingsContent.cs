/*
 * Description: Handle drawing the tool window content
 */
#if UNITY_EDITOR
using Service.Framework.Tools;
using UnityEditor;

namespace Support.Editor
{
    public class StatusEffectSettingsContent
    {
        private readonly SerializedProperty statusLevelConfigsProp;
        private readonly SerializedProperty statusEffectManagerConfigProp;

        /// <summary>
        /// Get the serialized content to draw
        /// </summary>
        /// <param name="serializedSettings"></param>
        public StatusEffectSettingsContent(SerializedObject serializedSettings)
        {
            statusEffectManagerConfigProp = serializedSettings.FindProperty(nameof(StatusEffectSettings.statusEffectManagerConfig));
            statusLevelConfigsProp = serializedSettings.FindProperty(nameof(StatusEffectSettings.statusLevels));
        }

        /// <summary>
        /// Draw the content when using the window tool
        /// </summary>
        public void DrawStatusEffectConfig()
        {
            using (InspectorDrawContext.ToolWindowScope())
            {
                EditorGUILayout.PropertyField(statusEffectManagerConfigProp, true);
                EditorGUILayout.Space(10);
            }
        }

        public void DrawStatusLevel()
        {
            using (InspectorDrawContext.ToolWindowScope())
            {
                EditorGUILayout.PropertyField(statusLevelConfigsProp, true);
            }
        }
    }
}
#endif