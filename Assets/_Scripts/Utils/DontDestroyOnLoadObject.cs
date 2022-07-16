using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyOnLoadObject : MonoBehaviour
{
    private void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnEnable() {
        Debug.Log("ENABLE");
    }
}
