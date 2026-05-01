/*
 * Description: Handles higher level logic for drawing the tool window, saving data when using the tool, etc.
 */
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Service.Framework.Tools;

namespace Support.Editor
{

    public class StatusEffectToolWindow : EditorWindow
    {
        private StatusEffectSettings toolSettings;
        private SerializedObject serializedSettings;

        private StatusEffectSettingsContent statusEffectSettingsContent;

        private Vector2 scroll;

        //open the window with default values
        [MenuItem("Tools/Status Effect Tool")]
        public static void Open()
        {
            StatusEffectToolWindow window = GetWindow<StatusEffectToolWindow>("Status Effect Tool");
            window.minSize = new Vector2(320, 140);
            window.Show();
        }

        private void OnEnable()
        {
            toolSettings = StatusEffectEditorAsset.GetOrCreateAsset();
            //prep to draw
            serializedSettings = new SerializedObject(toolSettings);
            //initialize a content renderer
            statusEffectSettingsContent = new StatusEffectSettingsContent(serializedSettings);
        }

        private void OnGUI()
        {
            if (toolSettings == null)
            {
                EditorGUILayout.HelpBox("Settings asset is missing.", MessageType.Error);
                if (GUILayout.Button("Generate Settings"))
                {
                    OnEnable();
                }
                return;
            }

            serializedSettings.Update();


            scroll = EditorGUILayout.BeginScrollView(scroll);

            //draw all the content
            statusEffectSettingsContent.DrawStatusEffectConfig();
            statusEffectSettingsContent.DrawStatusLevel();

            //add a little bit of space at the bottom of the scroll view to make it easier to see the last bit of content
            EditorGUILayout.Space(10);
            EditorGUILayout.EndScrollView();

            if (serializedSettings.ApplyModifiedProperties())
            {
                //use set dirty so we can save the settings and prevent them from undoing them with Ctl-Z (undo)
                EditorUtility.SetDirty(toolSettings);
                AssetDatabase.SaveAssets();
            }
        }
    }
}
#endif
