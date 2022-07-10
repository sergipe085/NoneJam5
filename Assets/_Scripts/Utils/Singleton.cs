using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance = null;

    protected virtual void Awake() {
        if (Instance) {
            Destroy(this);
        }
        else {
            Instance = this as T;
        }
    }
}
