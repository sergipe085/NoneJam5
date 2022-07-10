using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    public PlayerAttackState(StateMachine _stateMachine) : base(_stateMachine) {}

    public override void Enter() {
        GameObject obj = new GameObject("test");
        obj.transform.position = stateMachine.transform.position + Vector3.right;
        stateMachine.SwitchState(new PlayerMoveState(stateMachine));
    }

    public override void Tick(float deltaTime) {
        
    }

    public override void FixedTick(float deltaTime) {
        
    }

    public override void Exit() {
       
    }
}
