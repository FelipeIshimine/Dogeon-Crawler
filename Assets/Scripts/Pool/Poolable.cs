using System;
using UnityEngine;
using System.Collections;

public class Poolable : MonoBehaviour
{
    public event Action OnQueue;
    public event Action OnDequeue;

    public string key;
    public string Key
    {
        get
        {
            if (string.IsNullOrEmpty(key))
                key = gameObject.name;
            return key;
        }
        set => key = value;
    }
    private bool _isPooled;
    public bool IsPooled
    {
        get => _isPooled;
        set
        {
            _isPooled = value;
            if(_isPooled) OnQueue?.Invoke();
            if(!_isPooled) OnDequeue?.Invoke();
        }
    }
}