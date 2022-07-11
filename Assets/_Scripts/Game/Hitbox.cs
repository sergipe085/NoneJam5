using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    private GameObject owner = null;
    private Action<Attackable> OnHit;

    public void Initialize(GameObject _owner, float size, Action<Attackable> _OnHit) {
        this.owner = _owner;
        transform.localScale = new Vector2(size, size);
        this.OnHit = _OnHit;
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject == owner) return;

        Attackable other = col.GetComponent<Attackable>();
        if (other) {
            other.GetAttack(1, transform.position);
            OnHit?.Invoke(other);
        }
    }
}
