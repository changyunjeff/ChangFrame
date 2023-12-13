using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MBehaviour : MonoBehaviour
{
    public static event System.Action<MBehaviour> StaticDestroyEvent;
    public event System.Action<MBehaviour> DestroyEvent;
    
    protected virtual void Awake()
    {
        //添加自己到统一更新管理器中去
        if (!Application.isPlaying) return;
    }
    
    private static bool _isApplicationQuited = false; // 全局标记, 程序是否退出状态
    
    public static bool IsApplicationQuited
    {
        get { return _isApplicationQuited; }
    }

    public static System.Action ApplicationQuitEvent;

    public void OnApplicationQuit()
    {
        if (!_isApplicationQuited)
            Debug.Log("OnApplicationQuit!");
        _isApplicationQuited = true;
        ApplicationQuitEvent?.Invoke();
    }

    protected bool IsDestroyed = false;
    
    protected virtual void OnDestroy()
    {
        IsDestroyed = true;
        DestroyEvent?.Invoke(this);
        StaticDestroyEvent?.Invoke(this);
    }
}
