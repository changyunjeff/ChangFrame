using System;
using UnityEngine;
using Application = UnityEngine.Device.Application;

namespace Engine.Lib
{
    public abstract class MonoSingleton<T> : MBehaviour where T : MonoSingleton<T>
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    if (IsApplicationQuited)
                    {
                        return null;
                    }

                    var objects = FindObjectsOfType(typeof(T));
                    if (objects.Length > 0)
                    {
                        if (objects.Length > 1)
                            Debug.LogWarning(objects.Length + " " + typeof(T).Name + "s were found! Make sure to have only one at a time!");

                        _instance = (T)System.Convert.ChangeType(objects[0], typeof(T));
                        return _instance;
                    }

                    if (_instance == null && Application.isPlaying)
                    {
                        var go = new GameObject("Instance_" + typeof(T), typeof(T));
                        go.hideFlags = HideFlags.DontSave;
                        _instance = go.GetComponent<T>();
                        _instance.gameObject.SetActive(true);
                        _instance.enabled = true;
                    }
                }
                return _instance;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            if (_instance == null)
            {
                _instance = this as T;
            }
            else if (_instance != this)
            {
                Debug.LogError("Multiple " + typeof(T).Name + " in scene. please fix it.", gameObject);
                enabled = false;
                if (Application.isPlaying)
                    KTool.SafeDelete(this);
                return;
            }
#if UNITY_EDITOR
            if (Application.isPlaying)
                UnityEditor.SceneVisibilityManager.instance.Show(gameObject, false);
#endif
            
            DontDestroyOnLoad(gameObject);
        }

        public static void DestroyInstance()
        {
            if (!KTool.IsNull(_instance))
            {
                KTool.SafeDelete(_instance.gameObject);
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (_instance == this)
            {
                _instance = null;
            }
        }
    }
}