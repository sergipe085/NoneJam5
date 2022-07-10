using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState
{
    protected StateMachine baseStateMachine = null;

    public BaseState(StateMachine _stateMachine) {
        this.baseStateMachine = _stateMachine;
    }

    public abstract void Enter();
    public abstract void Tick(float deltaTime);
    public abstract void FixedTick(float deltaTime);
    public abstract void Exit();
}
