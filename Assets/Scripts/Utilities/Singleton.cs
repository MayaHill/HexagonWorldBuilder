using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    protected static T _instance = null;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject();
                obj.name = typeof(T).Name;
                _instance = obj.AddComponent<T>();
                Debug.LogWarning("Created new Singleton Object " + typeof(T).Name);
            }
            return _instance;
        }
    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }
}
