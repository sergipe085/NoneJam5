using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : MonoBehaviour
{
    [SerializeField] private Hitbox hitboxPrefab = null;

    public Hitbox Attack(Vector2 direction, float size) {
        direction.Normalize();
        Hitbox hitbox = Instantiate(hitboxPrefab, (transform.position + Vector3.up / 2) + (Vector3)direction * size / 2, Quaternion.identity);
        hitbox.Initialize(this.gameObject, size, null);
        Destroy(hitbox.gameObject, 0.1f);
        return hitbox;
    }

    public Hitbox AttachedAttack() {
        Hitbox hitbox = Instantiate(hitboxPrefab, transform);
        hitbox.Initialize(this.gameObject, 1.3f, null);
        hitbox.transform.localPosition += Vector3.up / 2;
        return hitbox;
    }
}
