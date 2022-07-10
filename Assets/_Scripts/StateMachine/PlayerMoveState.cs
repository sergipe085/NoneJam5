using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerBaseState
{
    private Rigidbody2D rig = null;
    private float maxVelocity = 10.0f;
    private float acelleration = 50.0f;
    private float desacelleration = 50.0f;
    private float gravityForce = 10.0f;

    private float moveInput = 0.0f;
    private bool isJumping = false;

    private float currentJumpHold = 0.0f;

    public PlayerMoveState(StateMachine _stateMachine) : base(_stateMachine) {}

    public override void Enter() {
        Debug.Log("oi");

        rig = stateMachine.rig;
        maxVelocity = stateMachine.maxVelocity;
        acelleration = stateMachine.acelleration;
        desacelleration = stateMachine.desacelleration;
        gravityForce = stateMachine.gravityForce;
    }

    public override void Tick(float deltaTime) {
        moveInput = Input.GetAxisRaw("Horizontal");
        isJumping = Input.GetKey(KeyCode.Space);

        if (Input.GetKeyDown(KeyCode.Space)) {
            Jump();
        }
    }

    public override void FixedTick(float deltaTime) {
        if (moveInput != 0.0f) {
            Move(moveInput, deltaTime);
        }
        else {
            Break(deltaTime);
        }

        stateMachine.Gravity(deltaTime);

        if (isJumping && currentJumpHold < stateMachine.maxJumpHold) {
            currentJumpHold += deltaTime;
        }
        else {
            stateMachine.Gravity(deltaTime);
        }
    }

    public override void Exit() {
        
    }

    private void Move(float direction, float deltaTime) {
        rig.AddForce(acelleration * deltaTime * new Vector2(direction, 0f), ForceMode2D.Impulse);
        Vector2 velocity = new Vector2(rig.velocity.x, 0f);
        velocity = Vector2.ClampMagnitude(velocity, maxVelocity);
        rig.velocity = new Vector2(velocity.x, rig.velocity.y);
    }

    private void Break(float deltaTime) {
        rig.AddForce(desacelleration * deltaTime * new Vector2(-rig.velocity.x, 0f), ForceMode2D.Force);
    } 

    private void Jump() {
        currentJumpHold = 0.0f;
        stateMachine.rig.velocity = new Vector2(stateMachine.rig.velocity.x, 0f);
        stateMachine.rig.AddForce(stateMachine.jumpForce * Vector2.up, ForceMode2D.Impulse);
    }
}
