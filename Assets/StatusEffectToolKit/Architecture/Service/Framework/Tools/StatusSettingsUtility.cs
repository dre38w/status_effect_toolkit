/*
 * Description: Loads the settings asset for runtime use
 */
using UnityEngine;

namespace Service.Framework.Tools
{
    public static class StatusSettingsUtility
    {
        private static StatusEffectSettings statusEffectSettings;
        public static StatusEffectSettings StatusEffectSettings
        {
            get
            {
                if (statusEffectSettings == null)
                {
                    //try to load file
                    statusEffectSettings = Resources.Load<StatusEffectSettings>(StatusEffectSettingsPathData.RESOURCE_PATH);

                    //if file was not loaded, then it doesn't exist.  so create it
                    if (statusEffectSettings == null)
                    {
                        Debug.LogWarning($"StatusEffectSettings was not found at path Resources/{StatusEffectSettingsPathData.RESOURCE_PATH}." +
                            $"Generating path and an empty StatusEffectSettings file.  Please perform initial setup in the Status Effect Tool.");
                        statusEffectSettings = StatusEffectEditorAsset.GetOrCreateAsset();
                    }
                }
                else
                {
                    return statusEffectSettings;
                }

                return statusEffectSettings;
            }
        }
    }
}