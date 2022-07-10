using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    [field: SerializeField] public Rigidbody2D rig { get; private set; }

    [Header("--- MOVE STATE PROPERTIES ---")]
    public float maxVelocity = 10.0f;
    public float acelleration = 50.0f;
    public float desacelleration = 50.0f;
    public float gravityForce = 10.0f;

    [Header("--- JUMP PROPERTIES ---")]
    public float jumpForce = 100.0f;
    public float maxJumpHold = 2.0f;

    private void Start() {
        SwitchState(new PlayerMoveState(this));
    }

    protected override void Update() {
        base.Update();
    }

    public void Gravity(float deltaTime) {
        rig.AddForce(gravityForce * deltaTime * Vector2.down, ForceMode2D.Force);
    }  
}
