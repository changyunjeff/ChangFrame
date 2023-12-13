using System;
using UnityEditor;
using UnityEngine;
using Application = UnityEngine.Device.Application;

namespace Engine.Editor
{
    public class BuilderWindow : EditorWindow
    {
        [MenuItem("ABBuilder/Builder Panel")]
        static void Open()
        {
            GetWindow<BuilderWindow>("BuilderWindow", true);
        }

        private static string _unityEditorEditorUserBuildSettingsActiveBuildTarget;
        
        public static string UnityEditor_EditorUserBuildSettings_activeBuildTarget
        {
            get
            {
                if (Application.isPlaying && !string.IsNullOrEmpty(_unityEditorEditorUserBuildSettingsActiveBuildTarget))
                {
                    return _unityEditorEditorUserBuildSettingsActiveBuildTarget;
                }

                var assemblies = AppDomain.CurrentDomain.GetAssemblies();

                foreach (var assembly in assemblies)
                {
                    if (assembly.GetName().Name == "UnityEditor")
                    {
                        Type lockType = assembly.GetType("UnityEditor.EditorUserBuildSettings");
                        var p = lockType.GetProperty("activeBuildTarget");
                        var em = p.GetGetMethod().Invoke(null, new object[] { }).ToString();
                        _unityEditorEditorUserBuildSettingsActiveBuildTarget = em;
                        return _unityEditorEditorUserBuildSettingsActiveBuildTarget;
                    }
                }
                
                return null;
            }
        }


        public static string GetBuildPlatformName()
        {
            string buildPlatformName = "Windows";   // default

            if (Application.isEditor)
            {
                var buildTarget = UnityEditor_EditorUserBuildSettings_activeBuildTarget;
                switch (buildTarget)
                {
                    case "StandaloneOSXIntel":
                    case "StandaloneOSXIntel64":
                    case "StandaloneOSXUniversal":
                    case "StandaloneOSX":
                        buildPlatformName = "MacOS";
                        break;
                    case "StandaloneWindows": // UnityEditor.BuildTarget.StandaloneWindows:
                    case "StandaloneWindows64": // UnityEditor.BuildTarget.StandaloneWindows64:
                        buildPlatformName = "Windows";
                        break;
                    case "Android": // UnityEditor.BuildTarget.Android:
                        buildPlatformName = "Android";
                        break;
                    case "iPhone": // UnityEditor.BuildTarget.iPhone:
                    case "iOS":
                        buildPlatformName = "iOS";
                        break;
                    default:
                        Debug.Assert(false);
                        break;
                }
            }
            else
            {
                switch (Application.platform)
                {
                    case RuntimePlatform.OSXPlayer:
                        buildPlatformName = "MacOS";
                        break;
                    case RuntimePlatform.Android:
                        buildPlatformName = "Android";
                        break;
                    case RuntimePlatform.IPhonePlayer:
                        buildPlatformName = "iOS";
                        break;
                    case RuntimePlatform.WindowsPlayer:
                        buildPlatformName = "Windows";
                        break;
                    default:
                        Debug.Assert(false);
                        break;
                }
            }

            return buildPlatformName;
        }
    }
}