using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T:MonoSingleton<T>
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            _instance = FindObjectOfType(typeof(T)) as T;
            if (_instance == null)
            {
                Debug.Log("new instance T:"+ typeof(T));
                GameObject go = new GameObject();
                _instance = go.AddComponent<T>();
                go.name = typeof(T).ToString();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }

    }
}
