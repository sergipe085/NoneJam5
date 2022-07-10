using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller
{
    [SerializeField] private Rigidbody2D rig = null;

    [SerializeField] private Mover moverComponent = null;
    [SerializeField] private Attacker attackerComponent = null;
    [SerializeField] private Dasher dasher = null;

    [SerializeField] private LayerMask groundLayer;

    private Vector2 moveInput = Vector2.zero;
    private bool jumpInput = false;
    private bool attackInput = false;
    private bool dashInput = false;
    private bool isInAir = false;

    private float currentDashTime = 0.0f;

    private void Update() {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        jumpInput = Input.GetKeyDown(KeyCode.Space);
        attackInput = Input.GetKeyDown(KeyCode.J);
        dashInput = Input.GetKeyDown(KeyCode.K);
        isInAir = !OnGround();

        if (dasher && dasher.IsDashing)  {
            moverComponent.StopJump();
            return;
        }

        HandleDash();
        HandleJump();
        HandleAttack();

        Debug.Log(OnGround());
    }

    private void FixedUpdate() {
        if (dasher && dasher.IsDashing) return;

        HandleMovement();

        moverComponent.Gravity();
    }  

    private void HandleMovement() {
        if (!moverComponent) return;

        if (moveInput.x != 0.0f) {
            moverComponent.Move(moveInput.x);
        }
        else {
            moverComponent.Break();
        }
    }

    private void HandleJump() {
        if (!moverComponent) return;

        if (jumpInput && !isInAir) {
            moverComponent.Jump();
        }

        if (moverComponent.IsJumping && !Input.GetKey(KeyCode.Space)) {
            if (moverComponent.GetVelocity().y > 0.2f) moverComponent.BreakJump();
            else moverComponent.StopJump();
        }
    }

    private void HandleAttack() {
        if (!attackerComponent) return;

        if (attackInput) {
            attackerComponent.Attack(moveInput);
        }
    }

    private void HandleDash() {
        if (!dasher) return;

        if (dashInput) {
            dasher.Dash(moveInput);
        }
    }

    private bool OnGround() {
        return Physics2D.OverlapBox(transform.position, new Vector2(1f, 0.5f), 0, groundLayer);
    }
}
