using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    [SerializeField] protected bool dontDestroy = true;
    private static T instance;
    public static T Instance
    {
        get { return instance; }

    }
    public static void SetInstance(T t)
    {
        instance = t;
    }

    protected virtual void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = (T)this;

            if (dontDestroy)
            {
                DontDestroyOnLoad(this);
            }
        }
    }

    public static bool IsInitialized
    {
        get { return instance != null; }
    }

    protected virtual void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

}
