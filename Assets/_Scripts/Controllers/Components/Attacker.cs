using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : MonoBehaviour
{
    [SerializeField] private Hitbox hitboxPrefab = null;

    public Hitbox Attack(Action<Attackable> onHit, Vector2 direction, float size) {
        direction.Normalize();
        Hitbox hitbox = Instantiate(hitboxPrefab, transform);
        hitbox.transform.position = (transform.position + Vector3.up / 2) + (Vector3)direction * size / 2;
        hitbox.Initialize(this.gameObject, size, onHit);
        Destroy(hitbox.gameObject, 0.1f);
        return hitbox;
    }

    public Hitbox AttachedAttack(Action<Attackable> onHit, Transform _transform, float size) {
        Hitbox hitbox = Instantiate(hitboxPrefab, _transform);
        hitbox.Initialize(this.gameObject, size, onHit);
        hitbox.transform.localPosition += Vector3.up / 2;
        return hitbox;
    }
}
