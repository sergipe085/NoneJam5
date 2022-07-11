using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BossController : Controller
{
    [Header("--- GERAL ---")]
    [SerializeField] private Light2D light = null;

    [Header("--- COMPONENTS ---")]
    [SerializeField] private Collider2D collider = null;
    [SerializeField] private Health health = null;
    [SerializeField] private Attacker attacker = null;

    [Header("--- STATES ---")]
    [SerializeField] private List<BossBaseState> attackStates = new();
    [SerializeField] private BossBaseState idleState = null;

    [SerializeField] private BossBaseState currentState = null;
    private BossStateEnum currentStateEnum = BossStateEnum.NONE;

    
    private void Start() {
        SwitchState(BossStateEnum.NONE);
    }

    private void OnEnable() {
        health.GetAttackEvent += GetAttack;
        health.OnDie += OnDie;
    }

    private void OnDisable() {
        health.GetAttackEvent -= GetAttack;
        health.OnDie -= OnDie;
    }

    private void Update() {
        if (GameManager.Instance.IsBattling() || currentStateEnum == BossStateEnum.DEAD) return;

        float distanceToPlayer = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
        if (distanceToPlayer <= 5.0f) {
            GameManager.Instance.StartBoss(this);
            SwitchState(BossStateEnum.IDLE);
        }
    }

    protected virtual void GetAttack() {
        scale.ChangeScale(new Vector2(2.5f, 2.5f));
    }

    public virtual void SwitchState(BossBaseState newState) {
        Debug.Log(newState);

        currentState?.Exit();
        currentState = newState;
        currentState?.Enter(this);
    }

    public virtual void SwitchState(BossStateEnum bossStateEnum) {
        currentStateEnum = bossStateEnum;

        switch(bossStateEnum) {
            case BossStateEnum.IDLE:
                SwitchState(idleState);
                break;
            case BossStateEnum.ATTACKING:
                int index = Random.Range(0, attackStates.Count);
                SwitchState(attackStates[index]);
                break;
            case BossStateEnum.DEAD:
                break;
            case BossStateEnum.NONE:
                break;
        }
    }

    private void OnDie() {
        currentState.isActive = false;
        GameEffectManager.Instance.DistortionPulse(0.8f, 30.0f);
        light.intensity = 0;
        rig.isKinematic = false;
        this.enabled = false;
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

    public Collider2D GetCollider() {
        return collider;
    }

    public ControllerProperties GetProperties() {
        return properties;
    }
}

public enum BossStateEnum { IDLE, ATTACKING, DEAD, NONE }
