using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : MonoBehaviour
{
    [SerializeField] private Hitbox hitboxPrefab = null;

    public void Attack(Vector2 direction) {
        direction.Normalize();
        Hitbox hitbox = Instantiate(hitboxPrefab, transform.position + Vector3.up / 2 + (Vector3)direction, Quaternion.identity);
        hitbox.Initialize(this.gameObject, 2.0f);
        Destroy(hitbox.gameObject, 0.1f);
    }

    public Hitbox AttachedAttack() {
        Hitbox hitbox = Instantiate(hitboxPrefab, transform);
        hitbox.Initialize(this.gameObject, 2.0f);
        hitbox.transform.localPosition += Vector3.up / 2;
        return hitbox;
    }
}
