using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossBaseState : MonoBehaviour
{
    protected BossController bossController = null;

    public bool isActive = false;

    public virtual void Enter(BossController _bossController) {
        this.bossController = _bossController;
        isActive = true;
    }

    public virtual void Update() {
        if (!isActive) return;
    }

    public virtual void Exit() {
        isActive = false;
    }
}
