using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackStateMeteoro : BossBaseState
{
    [SerializeField] private Bullet meteoroPrefab = null;
    [SerializeField] private float shootPerSecond = 5.0f;
    private float currentShootTime = 0.0f;

    private Rigidbody2D rig = null;

    private Health target = null;

    private float attackDuration = 5.0f;
    private float currentTime = 0.0f;

    private float upLevelTime = 4.0f;
    private float currentupLevelTime = 0.0f;

    private Hitbox hitbox;

    private Vector2 initialPosition = Vector2.zero;

    public override void Enter(BossController _bossController) {
        base.Enter(_bossController);

        target = PlayerController.Instance.GetComponent<Health>();

        rig = bossController.GetRigidbody2D();

        initialPosition = transform.position;

        Initialize();
    }

    private void Initialize() {
        if (!target) {
            bossController.SwitchState(BossStateEnum.IDLE);
            return;
        }

        currentTime = 0.0f;
        bossController.GetCollider().enabled = true;

        hitbox = bossController.GetAttacker().AttachedAttack(null, transform, 1.2f);

        Init();
    }

    public override void Update() {
        base.Update();

        if (!isActive) return;

        currentTime += Time.deltaTime;
        currentShootTime += Time.deltaTime;
        currentupLevelTime += Time.deltaTime;

        if (currentTime >= attackDuration) {
            bossController.SwitchState(BossStateEnum.ATTACKING);
        }

        if (currentupLevelTime >= upLevelTime) {
            shootPerSecond += 0.5f;
            bossController.GetHealth().GetAttack(3, transform.position);
            currentupLevelTime = 0.0f;
        }

        float shootTime = 1.0f / shootPerSecond;
        if (currentShootTime >= shootTime) {
            StartCoroutine(LaunchMeteoro());
            currentShootTime = 0.0f;
        }

        if (!target) {
            Initialize();
        }

        transform.position = Vector2.Lerp(transform.position, new Vector2(initialPosition.x, 2.3f + Mathf.Sin(Time.time) * 0.5f), 4.0f * Time.deltaTime);
    }

    public override void Exit() {
        base.Exit();
        if (hitbox) Destroy(hitbox.gameObject);
        rig.gravityScale = 1;
        bossController.GetCollider().isTrigger = false;
    }

    private void Init() {
        rig.isKinematic = false;
        rig.gravityScale = 0;
        bossController.GetCollider().isTrigger = true;
    }

    private IEnumerator LaunchMeteoro() {
        Bullet meteoro = Instantiate(meteoroPrefab, (Vector2)transform.position + new Vector2(Random.Range(-4, 5), 0), Quaternion.identity);

        yield return new WaitForSeconds(0.8f);

        Vector2 diretion = target.transform.position - meteoro.transform.position;
        meteoro.Initialize(diretion.normalized, 20.0f);
        meteoro.owner = this.gameObject;
        Hitbox hitbox = bossController.GetAttacker().AttachedAttack((att) => ObjectPool.Instance.DestroyBullet2(meteoro), meteoro.transform, 1.0f, true);

        Destroy(meteoro.gameObject, 1f);
    }
}
