using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [Header("--- COMPONENTS ---")]
    [SerializeField] protected Animator anim = null;
    [SerializeField] protected Rigidbody2D rig = null;
    [SerializeField] protected GameFeelScale scale = null;
    [SerializeField] protected SpriteRenderer spriteRenderer = null;

    [Header("--- GERAL ---")]
    [SerializeField] protected ControllerProperties properties = null;
    [SerializeField] protected LayerMask groundLayer;
    protected int currentLevel = 0;

    public void AddForce(Vector2 direction, float force) {
        rig.AddForce(force * direction, ForceMode2D.Impulse);
    }

    public Rigidbody2D GetRigidbody2D() {
        return rig;
    }

    public GameFeelScale GetGameFeelScale() {
        return scale;
    }

    public bool OnGround() {
        return Physics2D.OverlapBox(transform.position, new Vector2(1f, 0.5f), 0, groundLayer);
    }
}
