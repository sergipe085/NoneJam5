using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] private GameObject owner = null;

    private void OnTriggerEnter2D(Collider2D col) {
        Attackable other = col.GetComponent<Attackable>();

        if (!other) return;

        if (other.owner == owner || col.gameObject == owner) return;

        if (other) {
            other.GetAttack(1, transform.position, true);
        }
    }
}
