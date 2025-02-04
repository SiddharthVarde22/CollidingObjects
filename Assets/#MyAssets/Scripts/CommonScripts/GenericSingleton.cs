using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericSingleton<T> : MonoBehaviour where T : GenericSingleton<T>
{
    protected static T Instance;

    protected virtual void Awake()
    {
        if(Instance == null)
        {
            Instance = (T)this;
        }
        else
        {
            Debug.LogError(gameObject.name + " - instance already exists");
        }
    }

    protected virtual void OnDestroy()
    {
        Instance = null;
    }
}
