using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteoro : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rig = null;

    public void Shoot(Vector2 direction, float speed) {
        rig.velocity = direction.normalized * speed;
    }
}
