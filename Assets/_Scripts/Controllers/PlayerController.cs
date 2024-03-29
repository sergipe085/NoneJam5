using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : Controller
{
    public static PlayerController Instance = null;

    [Header("--- ACTION COMPONENTS ---")]
    [SerializeField] private Mover mover = null;
    [SerializeField] private Attacker attacker = null;
    [SerializeField] private Dasher dasher = null;

    private Vector2 moveInput = Vector2.zero;
    private Vector2 lookDirection = Vector2.zero;
    private int lastXLook = 1;
    private bool jumpInput = false;
    private bool attackInput = false;
    private bool dashInput = false;
    private bool lastOnGround = false;
    private bool canDash = true;
    private bool canAttack = true;

    [Header("--- ANIMATION ---")]
    private int isRunningHash = Animator.StringToHash("isRunning");
    private int isAirHash = Animator.StringToHash("isAir");
    private int isAirUpHash = Animator.StringToHash("isAirUp");
    private int isAirDownHash = Animator.StringToHash("isAirDown");
    private int attackHash = Animator.StringToHash("attack");

    [Header("--- GERAL ---")]
    [SerializeField] private GameObject attackEffectPrefab = null;
    public event Action OnDieEvent = null;

    private void OnEnable() {
        health.OnTakeDamage += HandleTakeDamage;
        health.OnDie += OnDie;
    }

    private void OnDisable() {
        health.OnTakeDamage -= HandleTakeDamage;
        health.OnDie -= OnDie;
    }

    private void Awake() {
        if (Instance) {
            Destroy(this);
        } else {
            Instance = this;
        }
    }

    protected override void Start() {
        base.Start();

        health.Reset();

        GameManager.Instance.OnStartBossEvent += () => health.Reset();
        GameManager.Instance.OnEndBossEvent += () => health.canLoseHealth = false;
    }

    private void Update() {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        jumpInput = Input.GetButtonDown("Jump");
        attackInput = Input.GetButtonDown("Fire1");
        dashInput = Input.GetButtonDown("Fire2");

        if (health.IsDead()) return;

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
            scale.ChangeScale(properties.jumpScale);
        }

        if (mover.IsJumping && !Input.GetButton("Jump")) {
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

        if (attackInput && canAttack) {
            canAttack = false;
            StartCoroutine(CanAttackTimer(0.2f));

            Vector2 attackDirection = new Vector2(lookDirection.y == 0 ? lookDirection.x : 0, lookDirection.y);

            Hitbox hitbox = attacker.Attack((att) => {
                if (attackDirection != Vector2.down) return;

                rig.velocity = Vector2.zero;
                rig.AddForce(-attackDirection * 15.0f, ForceMode2D.Impulse);
                mover.SetIsJumping(false);
                GameEffectManager.Instance.DistortionPulse(0.2f, 50.0f);
            }, attackDirection, 2);
            GameObject attackEffect = Instantiate(attackEffectPrefab, transform);
            attackEffect.transform.position = hitbox.transform.position;
            attackEffect.transform.up = attackDirection;
            attackEffect.transform.localRotation = Quaternion.Euler(0f, 0f, attackEffect.transform.eulerAngles.z + 90f);
        }
    }

    private IEnumerator CanAttackTimer(float time) {
        yield return new WaitForSeconds(time);
        canAttack = true;
    }

    private void HandleDash() {
        if (!dasher) return;

        if (lastOnGround) canDash = true;

        if (dashInput && (canDash)) {
            dasher.Dash(lookDirection);
            canDash = false;
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
        scale.ChangeScale(properties.landScale);
        HandleLandAnimation();
        canDash = true;
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

    private void HandleTakeDamage(Vector2 position) {
        mover.StopMove();
        StartCoroutine(TakeDamage(position));
    }

    private IEnumerator TakeDamage(Vector2 position) {
        GameEffectManager.Instance.DistortionPulse(properties.ldTakeDamageForce, properties.ldTakeDamageSpeed);

        yield return new WaitForSeconds(properties.stopTimeDuration);

        mover.ReturnMove();
        dasher.Dash((Vector2)transform.position - position, properties.dashTakeDamageForce, properties.dashTakeDamageLength);
    }

    private void OnDie() {
        rig.simulated = false;
        rig.velocity = Vector2.zero;

        GameEffectManager.Instance.VignetteIntensityLerp(1.0f, 2.0f, null);
        GameEffectManager.Instance.VignetteIntensityLerp(1.0f, 2.0f, () => GameSceneManager.Instance.LoadScene("GameScene"));
        OnDieEvent?.Invoke();
        GameManager.Instance.PlayerDie();
    }
}
