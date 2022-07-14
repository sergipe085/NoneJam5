using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackStateFollow : BossBaseState
{
    [SerializeField] private float maxVelocity = 10.0f;
    [SerializeField] private float speed = 10.0f;

    private Rigidbody2D rig = null;

    private Health target = null;

    private float attackDuration = 5.0f;
    private float currentTime = 0.0f;

    private Hitbox hitbox;

    private bool attackinEffect = false;

    public override void Enter(BossController _bossController) {
        base.Enter(_bossController);

        target = PlayerController.Instance.GetComponent<Health>();

        rig = bossController.GetRigidbody2D();
        Initialize();
    }

    private void Initialize() {
        if (!target) {
            bossController.SwitchState(BossStateEnum.IDLE);
            return;
        }

        currentTime = 0.0f;
        bossController.GetCollider().enabled = true;

        hitbox = bossController.GetAttacker().AttachedAttack((att) => StartCoroutine(OnHit()), transform, 1.2f);

        Init();
    }

    public override void Update() {
        base.Update();

        if (!isActive) return;

        currentTime += Time.deltaTime;

        if (currentTime >= attackDuration) {
            bossController.SwitchState(BossStateEnum.ATTACKING);
        }

        if (!target) {
            Initialize();
        }

        if (attackinEffect) return;

        Vector2 direction = ((Vector2)transform.position - (Vector2)target.transform.position).normalized;

        if (Mathf.RoundToInt(Vector2.Angle(direction, rig.velocity)) == 90) {
            rig.velocity -= rig.velocity * Time.deltaTime * speed;
        }
        else {
            rig.velocity += Time.deltaTime * direction * -speed;
            rig.velocity = Vector2.ClampMagnitude(rig.velocity, maxVelocity);
        }
    }

    public override void Exit() {
        base.Exit();
        if (hitbox) Destroy(hitbox.gameObject);
        rig.gravityScale = 1;
        bossController.GetCollider().isTrigger = false;
    }

    private IEnumerator OnHit() {
        if (hitbox) Destroy(hitbox.gameObject);
        Stop();
        attackinEffect = true;

        yield return new WaitForSeconds(bossController.GetProperties().stopAttackTime);
        
        Init();
        rig.gravityScale = 1;
        bossController.GetCollider().isTrigger = false;

        yield return new WaitForSeconds(0.3f);

        attackinEffect = false;

        yield return new WaitForSeconds(0.3f);

        if (isActive) {
            Initialize();

        }
    }

    private void Break() {
        rig.AddForce(500.0f * Time.deltaTime * -rig.velocity, ForceMode2D.Force);
    }

    private void Stop() {
        if (!isActive) return;

        rig.isKinematic = true;
        rig.velocity = Vector2.zero;
        bossController.GetCollider().isTrigger = true;
    }

    private void Init() {
        rig.isKinematic = false;
        rig.gravityScale = 0;
        bossController.GetCollider().isTrigger = true;
    }
}
