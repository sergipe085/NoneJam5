using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    protected BaseState currentState;

    protected virtual void Update() {
        currentState.Tick(Time.deltaTime);
    }

    protected virtual void FixedUpdate() {
        currentState?.FixedTick(Time.deltaTime);
    }

    public void SwitchState(BaseState newState) {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }
}
