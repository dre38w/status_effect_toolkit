/*
 * Description: Ensures the settings asset exists in editor. 
 * Its exsistence is important so we can load it during runtime.
 */
using UnityEditor;
using UnityEngine;


namespace Service.Framework.Tools
{
    public static class StatusEffectEditorAsset
    {

#if UNITY_EDITOR

        public static StatusEffectSettings GetOrCreateAsset()
        {
            //load the settings asset
            StatusEffectSettings settings = AssetDatabase.LoadAssetAtPath<StatusEffectSettings>(StatusEffectSettingsPathData.ASSET_PATH);

            //create it if it wasn't found
            if (settings == null)
            {
                //create a directory if it does not exist
                System.IO.Directory.CreateDirectory(StatusEffectSettingsPathData.ASSET_DIRECTORY);

                settings = ScriptableObject.CreateInstance<StatusEffectSettings>();

                AssetDatabase.CreateAsset(settings, StatusEffectSettingsPathData.ASSET_PATH);
                AssetDatabase.SaveAssets();
            }
            return settings;
#else
                return null;
#endif
        }
    }
}
