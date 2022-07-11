using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : Controller
{
    [Header("--- COMPONENTS ---")]
    [SerializeField] private Health health = null;
    [SerializeField] private Attacker attacker = null;

    [Header("--- STATES ---")]
    [SerializeField] private List<BossBaseState> attackStates = new();
    [SerializeField] private BossBaseState idleState = null;

    private BossBaseState currentState = null;

    
    private void Start() {
        SwitchState(BossStateEnum.IDLE);
    }

    private void OnEnable() {
        health.GetAttackEvent += GetAttack;
    }

    private void OnDisable() {
        health.GetAttackEvent -= GetAttack;
    }

    private void Update() {
        if (GameManager.Instance.IsBattling()) return;

        float distanceToPlayer = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
        if (distanceToPlayer <= 5.0f) {
            GameManager.Instance.StartBoss(this);
        }
    }

    protected virtual void GetAttack() {
        scale.ChangeScale(new Vector2(2.5f, 2.5f));
    }

    public virtual void SwitchState(BossBaseState newState) {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter(this, OnExitState);
    }

    public virtual void SwitchState(BossStateEnum bossStateEnum) {
        switch(bossStateEnum) {
            case BossStateEnum.IDLE:
                SwitchState(idleState);
                break;
            case BossStateEnum.ATTACKING:
                SwitchState(attackStates[Random.Range(0, attackStates.Count)]);
                break;
        }
    }

    private void OnExitState() {
        currentState = null;
    }

    public Attacker GetAttacker() {
        return attacker;
    }

    public Health GetHealth() {
        return health;
    }
}

public enum BossStateEnum { IDLE, ATTACKING }
