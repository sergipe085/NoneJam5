using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BossController : Controller
{
    public static BossController Instance = null;
    
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

    private void Awake() {
        if (Instance) {
            Destroy(this.gameObject);
        }
        else {
            Instance = this;
        }
    }
    
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

    protected virtual void GetAttack() {
        scale.ChangeScale(new Vector2(2.5f, 2.5f));
    }

    public virtual void SwitchState(BossBaseState newState, BossBaseState nextState) {
        currentState?.Exit();
        currentState = newState;

        if (newState) {
            currentState.SetNextState(nextState);
        }

        currentState?.Enter(this);
    }

    public virtual void SwitchState(BossStateEnum bossStateEnum) {
        currentStateEnum = bossStateEnum;

        switch(bossStateEnum) {
            case BossStateEnum.IDLE:
                SwitchState(idleState, null);
                break;
            case BossStateEnum.ATTACKING:
                int index = Random.Range(0, Mathf.Clamp(currentLevel + 1, 1, attackStates.Count));
                SwitchState(attackStates[index], null);
                break;
            case BossStateEnum.DEAD:
                break;
            case BossStateEnum.NONE:
                collider.enabled = false;
                break;
        }
    }

    public void StartBattle() {
        SwitchState(idleState, attackStates[Mathf.Clamp(currentLevel, 0, attackStates.Count - 1)]);
    }

    public void Initialize() {
        light.intensity = 1;
        rig.isKinematic = true;
        health.Reset();
    }

    private void OnDie() {
        if (currentStateEnum == BossStateEnum.DEAD) return;

        GameEffectManager.Instance.DistortionPulse(0.8f, 30.0f);

        currentState?.Exit();
        currentState = null;

        SwitchState(BossStateEnum.DEAD);
        
        light.intensity = 0;
        rig.isKinematic = false;
    }

    public bool IsDefeated() {
        return currentLevel == attackStates.Count && currentStateEnum == BossStateEnum.DEAD;
    }

    public BossStateEnum GetBossStateEnum() {
        return currentStateEnum;
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
