using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    private Vector2 initialPosition = Vector2.zero;
    
    private void Start() {
        initialPosition = transform.position;
    }

    public Vector2 GetRelativePosition() {
        return (Vector2)transform.position - initialPosition;
    }
}
