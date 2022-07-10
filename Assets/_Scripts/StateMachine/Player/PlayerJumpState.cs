using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    private BaseState moveState = null;
    
    private float currentJumpHold = 0.0f;

    public PlayerJumpState(StateMachine _stateMachine) : base(_stateMachine) {}

    public override void Enter() {
        moveState = new PlayerMoveState(stateMachine);
        moveState.Enter();

        stateMachine.rig.velocity = new Vector2(stateMachine.rig.velocity.x, 0f);
        stateMachine.rig.AddForce(stateMachine.jumpForce * Vector2.up, ForceMode2D.Impulse);
    }

    public override void Tick(float deltaTime) {
        if (Input.GetKey(KeyCode.Space) && currentJumpHold < stateMachine.maxJumpHold) {
            currentJumpHold += deltaTime;
        }
        else {
            stateMachine.Gravity(deltaTime);
        }

        moveState.Tick(deltaTime);
    }

    public override void FixedTick(float deltaTime) {
        moveState.FixedTick(deltaTime);
    }

    public override void Exit() {
        
    }
}
