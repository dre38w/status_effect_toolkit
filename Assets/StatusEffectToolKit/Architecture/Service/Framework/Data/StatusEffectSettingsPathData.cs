/*
 * Description: Data class that holds references to paths/directories for important files 
 */

namespace Service.Framework
{
    public class StatusEffectSettingsPathData
    {
        //the full path to the location of the SO asset
        public const string ASSET_PATH = "Assets/Resources/StatusEffectsTool/StatusEffectSettings.asset";
        //the folder directory where the SO asset lives
        public const string ASSET_DIRECTORY = "Assets/Resources/StatusEffectsTool";
        //the location of the SO in the Resources folder
        //this path is used to load during runtime
        public const string RESOURCE_PATH = "StatusEffectsTool/StatusEffectSettings";
    }
}