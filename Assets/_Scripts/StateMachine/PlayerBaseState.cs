using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState : BaseState
{
    protected PlayerStateMachine stateMachine = null;

    public PlayerBaseState(StateMachine _stateMachine) : base(_stateMachine) {
        stateMachine = _stateMachine as PlayerStateMachine;
    }

    // public override void Enter() {
    //     throw new System.NotImplementedException();
    // }

    // public override void Exit() {
    //     throw new System.NotImplementedException();
    // }

    // public override void Tick() {
    //     throw new System.NotImplementedException();
    // }
}
