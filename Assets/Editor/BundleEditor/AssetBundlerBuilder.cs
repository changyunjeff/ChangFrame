using System.Collections;
using System.Collections.Generic;
using System.IO;
using Engine.Lib;
using UnityEngine;

namespace Engine.Editor
{
    public class AssetBundleBuilder
    {
        public AssetBundleBuilder()
        {
            InitDirs();
        }

        public string GetExportBundlePath()
        {
            string basePath = Path.GetFullPath(AppEngine.GetConfig(AppEngine.EngineDefaultConfigs.AssetBundleBuildRelPath));
            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }
            string targetPath = Path.Combine(basePath, BuilderWindow.GetBuildPlatformName());
            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }

            return KTool.NormalizePathSeparator(targetPath);
        }

        void InitDirs()
        {
            GetExportBundlePath();
        }
    }
}
