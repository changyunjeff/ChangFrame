using System;
using System.IO;
using IniParser;
using IniParser.Model;
using IniParser.Parser;

namespace Engine.Lib
{
    public class AppConfigs
    {
        // @字符串 表示 强制不转译
        public static string DefaultConfig = @"
            [Engine]
            ProductRelPath = Product
            AssetBundleBuildRelPath = Product/Bundles
            StreamingBundlesFolderName = Bundles
            AssetBundleExt = .bytes
            IsEditorLoadAsset = 1
            IsLoadAssetBundle = 1
            IsEditorEncrypt = 0
            IsChinaMainland = 0

            [Engine.I18N]
            Languages = zh_CN,en_US,zh_TW
            I18N = zh_CN

            [Engine.Setting]
            SettingResourcesPath = Setting
            SettingSourcePath = Product/SettingSourcePath
            SettingCompiledPath = Product/Setting
            SettingNames = SettingCompare

            [Engine.Lua]
            LuaPath = Lua
            AssetBundleExt = .bytes

            [Engine.Behavior]
            BehaviorResourcesPath = Behavior
            BehaviorSourcePath = Product/SettingSourcePath/Behavior/exported
            BehaviorCompiledPath = Product/Behavior
        ";

        private readonly IniData _configData;

        public AppConfigs(string stringConfig)
        {
            var parser = new IniDataParser();
            _configData = parser.Parse(DefaultConfig);

            if (!string.IsNullOrEmpty(stringConfig))
            {
                if (!string.IsNullOrEmpty(stringConfig.Trim()))
                {
                    var newConfigs = parser.Parse(stringConfig);
                    _configData.Merge(newConfigs);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stringConfig"></param>
        public void MergeConfig(string stringConfig)
        {
            var parser = new IniDataParser();
            if (!string.IsNullOrEmpty(stringConfig))
            {
                if (!string.IsNullOrEmpty(stringConfig.Trim()))
                {
                    var newConfigs = parser.Parse(stringConfig);
                    _configData.Merge(newConfigs);
                }
            }
        }

        public string GetConfig(string section, string key, bool throwErr = true)
        {
            var sectionData = _configData[section];
            if (sectionData == null)
            {
                if (throwErr)
                    throw new Exception("Not Fount section from ini config: " + section);
                return null;
            }

            var value = sectionData[key];
            if (value == null)
            {
                if (throwErr)
                    throw new Exception(string.Format("Not found section:`{0}`, key:`{1}` config", section, key));
                return null;
            }

            return value;
        }

        public SectionCollection GetSections()
        {
            return _configData.Sections;
        }

        public bool ContainSection(string section)
        {
            return _configData[section] != null;
        }

        public bool ContainsKey(string section, string key)
        {
            if (!ContainSection(section))
                return false;
            return _configData[section][key] != null;
        }

        public void SetConfig(string section, string key, string value)
        {
            _configData[section][key] = value;
        }

        public void Save(string filePath)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(_configData.ToString());
                fs.Write(bytes, 0, bytes.Length);
                fs.Flush();
            }
        }

        public string IniToString()
        {
            return _configData.ToString();
        }
    }
}