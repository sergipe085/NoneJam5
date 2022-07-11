using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackStateLaunchEnemy : BossBaseState
{
    [SerializeField] private Bullet bulletPrefab = null;
    [SerializeField] private float shootPerSecond = 5.0f;
    private float currentTime = 0.0f;

    private Rigidbody2D rig = null;

    private Health target = null;
    private Vector2 initialPosition = Vector2.zero;

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
    }

    public override void Update() {
        base.Update();

        if (!isActive) return;

        rig.velocity = Vector2.zero;

        currentTime += Time.deltaTime;

        float shootTime = 1.0f / shootPerSecond;
        if (currentTime >= shootTime) {
            Shoot();
            currentTime = 0.0f;
        }
    }

    private void Shoot() {
        Bullet bullet = ObjectPool.Instance.SpawnBullet(transform.position + Vector3.up / 2);
        bullet.Initialize(new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)), 5.0f);
        Hitbox hitbox = bossController.GetAttacker().AttachedAttack((att) => ObjectPool.Instance.DestroyBullet(bullet), bullet.transform);
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
