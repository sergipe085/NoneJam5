using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackStateLaunchEnemy : BossBaseState
{
    [SerializeField] private Bullet bulletPrefab = null;
    [SerializeField] private float shootPerSecond = 5.0f;
    private float currentShootTime = 0.0f;

    private Rigidbody2D rig = null;

    private Health target = null;
    private Vector2 initialPosition = Vector2.zero;

    private float attackDuration = 5.0f;
    private float currentTime = 0.0f;

    private void Start() {
        initialPosition = transform.position;
    }

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

        currentShootTime = 0.0f;
        currentTime = 0.0f;
        bossController.GetCollider().enabled = true;
    }

    public override void Update() {
        base.Update();

        if (!isActive) return;

        rig.velocity = Vector2.zero;

        currentShootTime += Time.deltaTime;
        currentTime += Time.deltaTime;

        float shootTime = 1.0f / shootPerSecond;
        if (currentShootTime >= shootTime) {
            Shoot();
            currentShootTime = 0.0f;
        }

        if (currentTime >= attackDuration) {
            bossController.SwitchState(BossStateEnum.ATTACKING);
        }

        transform.position = Vector2.Lerp(transform.position, initialPosition + new Vector2(Mathf.Cos(currentTime), Mathf.Sin(currentTime)) * 5.0f, 10.0f * Time.deltaTime);
    }

    private void Shoot() {
        Bullet bullet = ObjectPool.Instance.SpawnBullet2(transform.position + Vector3.up / 2);

        if (!bullet) return;

        bullet.Initialize(new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)), 5.0f);
        bullet.owner = this.gameObject;
        Hitbox hitbox = bossController.GetAttacker().AttachedAttack((att) => ObjectPool.Instance.DestroyBullet2(bullet), bullet.transform, 0.5f, true);
    }

    public override void Exit() {
        base.Exit();
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
