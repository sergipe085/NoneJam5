using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dasher : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rig = null;

    [SerializeField] private float dashForce = 10.0f;
    [SerializeField] private float dashBreakForce = 10.0f;
    [SerializeField] private float dashLength = 10.0f;

    public bool IsDashing { get; private set; } = false;
    private float currentDashTime = 0.0f;

    private void Update() {
        if (IsDashing && currentDashTime < dashLength) {
            currentDashTime += Time.deltaTime;
        }
        else if (IsDashing) {
            StopDash();
        }
    }

    public void Dash(Vector2 direction) {
        if (direction.magnitude != 0) return;

        rig.velocity = Vector2.zero;
        rig.AddForce(direction.normalized * dashForce, ForceMode2D.Impulse);
        IsDashing = true;
    }

    private void StopDash() {
        IsDashing = false;
        currentDashTime = 0.0f;
        rig.AddForce(new Vector2(0f, -rig.velocity.y) * dashBreakForce, ForceMode2D.Force);
    }
}
