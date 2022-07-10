using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller
{
    [Header("--- COMPONENTS ---")]
    [SerializeField] private Rigidbody2D rig = null;
    [SerializeField] private GameFeelScale scale = null;

    [Header("--- ACTION COMPONENTS ---")]
    [SerializeField] private Mover mover = null;
    [SerializeField] private Attacker attacker = null;
    [SerializeField] private Dasher dasher = null;

    [SerializeField] private LayerMask groundLayer;

    private Vector2 moveInput = Vector2.zero;
    private bool jumpInput = false;
    private bool attackInput = false;
    private bool dashInput = false;
    private bool lastOnGround = false;

    private float currentDashTime = 0.0f;

    private void Update() {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        jumpInput = Input.GetKeyDown(InputMap.Instance.actionDictionary[ACTION.Jump]);
        attackInput = Input.GetKeyDown(KeyCode.J);
        dashInput = Input.GetKeyDown(KeyCode.K);

        CheckLand();

        if (dasher && dasher.IsDashing)  {
            mover.StopJump();
            return;
        }

        HandleDash();
        HandleJump();
        HandleAttack();
    }

    private void FixedUpdate() {
        if (dasher && dasher.IsDashing) return;

        HandleMovement();

        mover.Gravity();
    }  

    private void HandleMovement() {
        if (!mover) return;

        if (moveInput.x != 0.0f) {
            mover.Move(moveInput.x);
        }
        else {
            mover.Break();
        }
    }

    private void HandleJump() {
        if (!mover) return;

        if (jumpInput && lastOnGround) {
            mover.Jump();
            scale.ChangeScale(new Vector2(0.5f, 1.5f));
        }

        if (mover.IsJumping && !Input.GetKey(InputMap.Instance.actionDictionary[ACTION.Jump])) {
            if (mover.GetVelocity().y > 0.2f) mover.BreakJump();
            else mover.StopJump();
        }
    }

    private void HandleAttack() {
        if (!attacker) return;

        if (attackInput) {
            attacker.Attack(moveInput);
        }
    }

    private void HandleDash() {
        if (!dasher) return;

        if (dashInput) {
            dasher.Dash(moveInput);
        }
    }

    private void Land() {
        scale.ChangeScale(new Vector2(1.5f, 0.5f));
    }

    private void CheckLand() {
        bool isGround = OnGround();
        if (!lastOnGround && isGround && rig.velocity.y < -0.5f) {
            Land();
        }
        lastOnGround = isGround;
    }

    private bool OnGround() {
        return Physics2D.OverlapBox(transform.position, new Vector2(1f, 0.5f), 0, groundLayer);
    }
}
