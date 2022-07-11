using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossBaseState : MonoBehaviour
{
    protected BossController bossController = null;
    private Action OnExitState = null;

    public bool isActive = false;

    public virtual void Enter(BossController _bossController, Action _OnExitState) {
        this.bossController = _bossController;
        this.OnExitState = _OnExitState;
        isActive = true;
    }

    public virtual void Update() {
        if (!isActive) return;
    }

    public virtual void Exit() {
        OnExitState.Invoke();
        isActive = false;
    }
}
