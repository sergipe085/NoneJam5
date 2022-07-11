using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField] private Health health = null;
    [SerializeField] private GameFeelScale gameFeelScale = null;

    private BossState currentState = BossState.IDLE;
    
    private void OnEnable() {
        health.GetAttackEvent += GetAttack;
    }

    private void OnDisable() {
        health.GetAttackEvent -= GetAttack;
    }

    protected virtual void Update() {
        switch(currentState) {
            case BossState.IDLE:
                IdleState();
                break;
        }
    }

    protected virtual void SwitchState(BossState newState) {
        currentState = newState;
    }

    protected virtual void GetAttack() {
        gameFeelScale.ChangeScale(new Vector2(0.5f, 1.5f));
    }

    protected virtual void IdleState() {

    }

    protected enum BossState { IDLE }
}
