using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Mover moverComponent = null;
    [SerializeField] private Attacker attackerComponent = null;

    private Vector2 moveInput = Vector2.zero;
    private bool jumpInput = false;
    private bool attackInput = false;
    
    private bool isJumping = false;

    private void Update() {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        jumpInput = Input.GetKeyDown(KeyCode.Space);
        attackInput = Input.GetKeyDown(KeyCode.J);

        HandleJump();
        HandleAttack();
    }

    private void FixedUpdate() {
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

        if (jumpInput) {
            isJumping = true;
            moverComponent.Jump();
        }

        if (isJumping && !Input.GetKey(KeyCode.Space)) {
            if (moverComponent.GetVelocity().y > 0.2f) moverComponent.BreakJump();
            else isJumping = false;
        }
    }

    private void HandleAttack() {
        if (!attackerComponent) return;

        if (attackInput) {
            attackerComponent.Attack(moveInput);
        }
    }
}
