using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFeelScale : MonoBehaviour
{
    [SerializeField] private float smoothAmount = 10.0f;

    private Vector3 initialScale = Vector2.zero;
    private Vector3 targetScale = Vector2.zero;

    private void Start() {
        initialScale = transform.localScale;
    }

    private void Update() {
        targetScale = Vector2.Lerp(targetScale, initialScale, Time.deltaTime * smoothAmount);
        transform.localScale = Vector2.Lerp(transform.localScale, targetScale, Time.deltaTime * smoothAmount);
    }

    public void ChangeScale(Vector2 newScale) {
        targetScale = new Vector3(newScale.x, newScale.y, 1);
    }
}
