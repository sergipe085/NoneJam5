using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public void Initialize(float size) {
        transform.localScale = new Vector2(size, size);
        Destroy(this.gameObject, 0.1f);
    }

    private void OnTriggerEnter2D(Collider2D col) {
        Attackable other = col.GetComponent<Attackable>();
        other?.GetAttack(1, transform.position);
    }
}
