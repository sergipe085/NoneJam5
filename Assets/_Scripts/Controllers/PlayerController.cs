using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller
{
    public static PlayerController Instance = null;

    [Header("--- ACTION COMPONENTS ---")]
    [SerializeField] private Mover mover = null;
    [SerializeField] private Attacker attacker = null;
    [SerializeField] private Dasher dasher = null;
    [SerializeField] private Health health = null;

    private Vector2 moveInput = Vector2.zero;
    private Vector2 lookDirection = Vector2.zero;
    private int lastXLook = 1;
    private bool jumpInput = false;
    private bool attackInput = false;
    private bool dashInput = false;
    private bool lastOnGround = false;

    [Header("--- ANIMATION ---")]
    private int isRunningHash = Animator.StringToHash("isRunning");
    private int isAirHash = Animator.StringToHash("isAir");
    private int isAirUpHash = Animator.StringToHash("isAirUp");
    private int isAirDownHash = Animator.StringToHash("isAirDown");
    private int attackHash = Animator.StringToHash("attack");

    [Header("--- GAME FEEL ---")]
    [SerializeField] private Vector2 landScale = Vector2.zero;
    [SerializeField] private Vector2 jumpScale = Vector2.zero;
    [SerializeField] private GameObject attackEffectPrefab = null;

    private float currentDashTime = 0.0f;

    private void Awake() {
        if (Instance) {
            Destroy(this);
        } else {
            Instance = this;
        }
    }

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

        LookDirection();
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
            lookDirection = moveInput;
            
        }
        else {
            mover.Break();
            lookDirection.y = 0;
        }

        HandleMovementAnimation();
    }

    private void HandleMovementAnimation() {
        anim.SetBool(isRunningHash, moveInput.x != 0);
        spriteRenderer.flipX = lookDirection.x != 1;
    }

    private void HandleJump() {
        if (!mover) return;

        if (jumpInput && lastOnGround) {
            mover.Jump();
            scale.ChangeScale(jumpScale);
        }

        if (mover.IsJumping && !Input.GetKey(InputMap.Instance.actionDictionary[ACTION.Jump])) {
            if (mover.GetVelocity().y > 0.2f) mover.BreakJump();
            else mover.StopJump();
        }

        HandleJumpAnimations();
    }

    private void HandleJumpAnimations() {
        if (!lastOnGround) {
            anim.SetBool(isAirHash, true);
            if (rig.velocity.y > 0.1f) {
                anim.SetBool(isAirUpHash, true);
                anim.SetBool(isAirDownHash, false);
            }
            else {
                anim.SetBool(isAirUpHash, false);   
                anim.SetBool(isAirDownHash, true);
            }
        }
    }

    private void HandleAttack() {
        if (!attacker) return;

        if (attackInput) {
            Vector2 attackDirection = new Vector2(lookDirection.y == 0 ? lookDirection.x : 0, lookDirection.y);

            Hitbox hitbox = attacker.Attack(attackDirection, 2);
            GameObject attackEffect = Instantiate(attackEffectPrefab, transform);
            attackEffect.transform.position = hitbox.transform.position;
            attackEffect.transform.up = attackDirection;
            attackEffect.transform.localRotation = Quaternion.Euler(0f, 0f, attackEffect.transform.eulerAngles.z + 90f);
        }
    }

    private void HandleDash() {
        if (!dasher) return;

        if (dashInput) {
            dasher.Dash(lookDirection);
        }
    }

    private void LookDirection() {
        if (moveInput.magnitude != 0) {
            lookDirection = moveInput;
            if (moveInput.x != 0) lastXLook = (int)Mathf.Sign(moveInput.x);
        } else {
            lookDirection.y = 0;
            lookDirection.x = lastXLook;
        }
    }

    private void Land() {
        scale.ChangeScale(landScale);
        HandleLandAnimation();
    }

    private void HandleLandAnimation() {
        anim.SetBool(isAirHash, false);
    }

    private void CheckLand() {
        bool isGround = OnGround();
        if (!lastOnGround && isGround && rig.velocity.y < 0.1f) {
            Land();
        }
        lastOnGround = isGround;
    }
}
