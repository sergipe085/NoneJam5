using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTestState : BossBaseState
{
    [SerializeField] private float dashForce = 20.0f;

    private Vector2 targetPosition = Vector2.zero;
    private Vector2 initialPosition = Vector2.zero;

    private Rigidbody2D rig = null;

    private bool attacking = false;
    private bool stopAttack = false;

    private Health target = null;
    private Hitbox hitbox = null;

    private bool hitted = false;

    private void Start() {
        initialPosition = transform.position;
    }

    public override void Enter(BossController _bossController, Action _OnExitState) {
        base.Enter(_bossController, _OnExitState);

        target = PlayerController.Instance.GetComponent<Health>();

        rig = bossController.GetRigidbody2D();
        Initialize();
    }

    private void Initialize() {
        if (!target) {
            bossController.SwitchState(BossStateEnum.IDLE);
            return;
        }

        attacking = false;
        stopAttack = false;
        hitted = false;
        Stop();
        targetPosition = initialPosition + new Vector2(UnityEngine.Random.Range(-5f, 5f), UnityEngine.Random.Range(4f, 6f));
    }

    public override void Update() {
        base.Update();

        if (!isActive) return;

        if (stopAttack || bossController.OnGround()) {
            Break();
        }

        if (attacking) return;

        transform.position = Vector2.Lerp(transform.position, targetPosition, 3.0f * Time.deltaTime);

        float currentDistance = Vector2.Distance(transform.position, targetPosition);
        if (currentDistance < 0.3f) {
            StartCoroutine(Attack());
        }
    }

    public override void Exit() {
        base.Exit();
    }

    private IEnumerator Attack() {
        attacking = true;
        rig.isKinematic = false;
        bossController.GetCollider().enabled = true;
        Vector2 playerPosition = PlayerController.Instance.transform.position;
        rig.AddForce(dashForce * (playerPosition - (Vector2)transform.position).normalized, ForceMode2D.Impulse);

        hitbox = bossController.GetAttacker().AttachedAttack((att) => StartCoroutine(OnHit()));

        yield return new WaitForSeconds(0.5f);

        if (hitted) yield break;

        stopAttack = true;
        if (hitbox) Destroy(hitbox.gameObject);

        yield return new WaitForSeconds(0.5f);

        Initialize();
    }

    private IEnumerator OnHit() {
        hitted = true;
        Destroy(hitbox.gameObject);
        Stop();

        yield return new WaitForSeconds(0.1f);

        Initialize();
    }

    private void Break() {
        rig.AddForce(500.0f * Time.deltaTime * -rig.velocity, ForceMode2D.Force);
    }

    private void Stop() {
        rig.isKinematic = true;
        rig.velocity = Vector2.zero;
        bossController.GetCollider().enabled = false;
    }
}
