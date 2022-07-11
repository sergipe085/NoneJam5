using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    private GameObject owner = null;

    public void Initialize(GameObject _owner, float size) {
        this.owner = _owner;
        transform.localScale = new Vector2(size, size);
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject == owner) return;

        Attackable other = col.GetComponent<Attackable>();
        other?.GetAttack(1, transform.position);
    }
}
