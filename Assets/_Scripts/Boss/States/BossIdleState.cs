using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIdleState : BossBaseState
{
    private Vector2 initialPosition = Vector2.zero;

    private Rigidbody2D rig = null;
    private GameObject player = null;

    private float timeToChange = 1.0f;
    private float currentTime = 0.0f;

    private void Start() {
        initialPosition = transform.position;
        player = PlayerController.Instance.gameObject;
    }

    public override void Enter(BossController _bossController) {
        base.Enter(_bossController);

        rig = bossController.GetRigidbody2D();
        bossController.GetCollider().enabled = true;

        Initialize();
    }

    private void Initialize() {
        rig.velocity = Vector2.zero;
        rig.isKinematic = true;
        currentTime = 0.0f;
    }


    public override void Update() {
        base.Update();

        if (!isActive) return;

        transform.position = Vector2.Lerp(transform.position, initialPosition, 3.0f * Time.deltaTime);

        currentTime += Time.deltaTime;
        if (currentTime >= timeToChange) {
            if (nextState) bossController.SwitchState(nextState, null);
            else bossController.SwitchState(BossStateEnum.ATTACKING);
        }
    }

    public override void Exit() {
        base.Exit();
        rig.isKinematic = false;
    }
}
