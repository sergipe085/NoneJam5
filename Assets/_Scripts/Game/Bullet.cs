using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rig = null;

    public void Initialize(Vector2 direction, float speed) {
        rig.velocity = direction.normalized * speed;
    }
}
