using UnityEngine;
using Application = UnityEngine.Device.Application;

namespace Engine.Lib
{
    public class KTool
    {
        public static string NormalizePathSeparator(string path)
        {
            return path.Replace("\\", "/");
        }

        public static void SafeDelete(UnityEngine.Object instance, bool immediate = false)
        {
            if (instance != null)
            {
                var gameObject = instance as GameObject;

                if (gameObject != null)
                {
                    gameObject.SetActive(false);

                    if (!(gameObject.transform is RectTransform))
                    {
                        gameObject.transform.parent = null;
                    }
                }

                if (!Application.isPlaying || immediate)
                {
                    UnityEngine.Object.DestroyImmediate(instance);
                }
                else
                {
                    UnityEngine.Object.Destroy(instance);
                }
            }
        }

        public static bool IsNull(object obj)
        {
            var unityObj = obj as UnityEngine.Object;
            if (!ReferenceEquals(unityObj, null))
            {
                return unityObj == null;
            }

            return obj == null;
        }
    }
}