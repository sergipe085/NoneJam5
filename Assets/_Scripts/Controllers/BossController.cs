using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BossController : Controller
{
    public static BossController Instance = null;
    
    [Header("--- GERAL ---")]
    [SerializeField] private GameObject lightContainer = null;

    [Header("--- COMPONENTS ---")]
    [SerializeField] private Collider2D col = null;
    [SerializeField] private Health health = null;
    [SerializeField] private Attacker attacker = null;

    [Header("--- STATES ---")]
    [SerializeField] private List<BossBaseState> attackStates = new();
    [SerializeField] private BossBaseState idleState = null;

    [SerializeField] private BossBaseState currentState = null;
    private BossStateEnum currentStateEnum = BossStateEnum.NONE;

    protected void Awake() {
        if (Instance) {
            Destroy(this.gameObject);
        }
        else {
            Instance = this;
        }
    }
    
    private void Start() {
        if (currentLevel >= attackStates.Count) {
            SwitchState(BossStateEnum.DEAD);
        }
        else {
            SwitchState(BossStateEnum.NONE);
        }
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
                lightContainer.SetActive(false);
                rig.isKinematic = false;
                rig.velocity = Vector2.zero;
                anim.SetBool("Dead", true);
                health.SetHealth(0);
                col.enabled = true;
                break;
            case BossStateEnum.NONE:
                col.enabled = false;
                break;
        }
    }

    public void StartBattle() {
        SwitchState(idleState, attackStates[Mathf.Clamp(currentLevel, 0, attackStates.Count - 1)]);
    }

    public void Initialize() {
        lightContainer.SetActive(true);
        rig.isKinematic = true;
        health.Reset();
        anim.SetBool("Dead", false);
    }

    private void OnDie() {
        GameEffectManager.Instance.DistortionPulse(0.8f, 30.0f);

        currentState?.Exit();
        currentState = null;

        Debug.Log("Dead");

        SwitchState(BossStateEnum.DEAD);
        col.enabled = true;
    }

    public bool IsDefeated() {
        return currentLevel == attackStates.Count && currentStateEnum == BossStateEnum.DEAD;
    }

    public int GetCurrentLevel() {
        return currentLevel;
    }

    public void SetCurrentLevel(int level) {
        currentLevel = Mathf.Clamp(level, 0, attackStates.Count);
    }

    public void LevelUp() {
        currentLevel = Mathf.Clamp(currentLevel + 1, 0, attackStates.Count);
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
        return col;
    }

    public ControllerProperties GetProperties() {
        return properties;
    }
}

public enum BossStateEnum { IDLE, ATTACKING, DEAD, NONE }
