using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private bool parallaxX = false;
    [SerializeField] private bool parallaxY = false;
    private Vector2 offset = Vector2.zero;

    [SerializeField] private float moveXAmount = 0.0f;
    [SerializeField] private float moveYAmount = 0.0f;
    [SerializeField] private SpriteRenderer rend = null;

    private Vector2 initialPos = Vector2.zero;

    private void Start() {
        initialPos = transform.position;
    }

    private void Update() {
        if (parallaxX) offset.x = CameraController.Instance.GetRelativePosition().x;
        if (parallaxY) offset.y = CameraController.Instance.GetRelativePosition().y;
        if (rend) rend.material.SetTextureOffset("_MainTex", new Vector2(offset.x * moveXAmount, 0f));

        transform.position = new Vector2(initialPos.x, initialPos.y + offset.y * -moveYAmount);
    }
}
