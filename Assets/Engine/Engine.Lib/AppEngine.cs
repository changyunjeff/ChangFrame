using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Engine.Lib
{
    
    public class AppEngine : MBehaviour
    {
        
        public enum EngineDefaultConfigs
        {
            AssetBundleExt,
            ProductRelPath,
            AssetBundleBuildRelPath, // FromRelPath

            StreamingBundlesFolderName,
        }

        private static AppConfigs _engineConfigs;
        
        public static AppConfigs PreLoadConfigs(bool forceReload = false)
        {
            if (_engineConfigs != null && !forceReload)
                return _engineConfigs;

            string configContent = null;
            var textAsset = Resources.Load<TextAsset>("AppConfigs");
            if (textAsset!=null)
                configContent = textAsset.text;

            _engineConfigs = new AppConfigs(configContent);
            return _engineConfigs;
        }

        public static string GetConfig(EngineDefaultConfigs cfg)
        {
            return GetConfig("Engine", cfg.ToString());
        }

        public static string GetConfig(string section, string key, bool showLog = true)
        {
            PreLoadConfigs();
            var value = _engineConfigs.GetConfig(section, key, false);
            if (value == null)
            {
                if(showLog)
                    Debug.LogErrorFormat("Cannot Get Config, Section: {0}, key: {1}", section, key);
            }
            return value;
        }
    }
}
