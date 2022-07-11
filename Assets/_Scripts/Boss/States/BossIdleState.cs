using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIdleState : BossBaseState
{
    private Vector2 initialPosition = Vector2.zero;

    private Rigidbody2D rig = null;
    private GameObject player = null;

    private void Start() {
        initialPosition = transform.position;
        player = PlayerController.Instance.gameObject;
    }

    public override void Enter(BossController _bossController, Action _OnExitState) {
        base.Enter(_bossController, _OnExitState);

        rig = bossController.GetRigidbody2D();

        Initialize();
    }

    private void Initialize() {
        rig.velocity = Vector2.zero;
        rig.isKinematic = true;
    }


    public override void Update() {
        base.Update();

        if (!isActive) return;

        transform.position = Vector2.Lerp(transform.position, initialPosition, 3.0f * Time.deltaTime);
    }

    public override void Exit() {
        base.Exit();
        rig.isKinematic = false;
    }
}
