using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : MonoBehaviour
{
    [SerializeField] private Transform hitboxPrefab = null;

    public void Attack(Vector2 direction) {
        direction.Normalize();
        Transform hitbox = Instantiate(hitboxPrefab, transform.position + (Vector3)direction, Quaternion.identity);
    }
}
