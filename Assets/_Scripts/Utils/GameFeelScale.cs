using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFeelScale : MonoBehaviour
{
    [SerializeField] private Transform sprite = null;
    [SerializeField] private float smoothAmount = 10.0f;

    private Vector3 initialScale = Vector3.zero;
    private Vector3 targetScale = Vector3.zero;
    private Vector3 currentScale = Vector3.zero;

    private void Start() {
        initialScale = new Vector3(sprite.transform.localScale.x, sprite.transform.localScale.y, 1);
        targetScale = initialScale;
    }

    private void Update() {
        targetScale = Vector2.Lerp(targetScale, initialScale, Time.deltaTime * smoothAmount);
        currentScale = Vector2.Lerp(sprite.transform.localScale, targetScale, Time.deltaTime * smoothAmount);
        sprite.localScale = new Vector3(currentScale.x, currentScale.y, 1);
    }   

    public void ChangeScale(Vector2 newScale) {
        targetScale = new Vector3(newScale.x, newScale.y, 1);
    }
}
