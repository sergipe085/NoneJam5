using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

// REFAZER TODA A LOGICA DO BOSS E DO GAME MANAGER QND A JAM ACABAR
public class BossController : Controller
{
    public static BossController Instance = null;
    
    [Header("--- GERAL ---")]
    [SerializeField] private GameObject lightContainer = null;
    [SerializeField] private List<int> maxHealthLevels = new();

    [Header("--- COMPONENTS ---")]
    [SerializeField] private Collider2D col = null;
    [SerializeField] private Health health = null;
    [SerializeField] private Attacker attacker = null;

    [Header("--- STATES ---")]
    [SerializeField] private List<BossBaseState> attackStates = new();
    [SerializeField] private BossBaseState idleState = null;

    [SerializeField] private BossBaseState currentState = null;
    private BossStateEnum currentStateEnum = BossStateEnum.NONE;

    public event Action startBattleEvent = null;

    public event Action BeforeStartBoss = null;
    public event Action StartBossTutorial = null;

    protected void Awake() {
        if (Instance) {
            Destroy(this.gameObject);
        }
        else {
            Instance = this;
        }
    }
    
    private void Start() {
        if (currentLevel == 0) {
            SwitchState(BossStateEnum.TUTORIAL);
            Debug.Log("SHOW");
        }
        else if (currentLevel >= attackStates.Count) {
            SwitchState(BossStateEnum.DEAD);
            BeforeStartBoss?.Invoke();
        }
        else {
            health.Reset();
            SwitchState(BossStateEnum.DEAD);
            BeforeStartBoss?.Invoke();
        }

        if (!IsDefeated()) {
            health.ChangeMaxHealth(maxHealthLevels[GetCurrentLevel()]);
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
        GameEffectManager.Instance.DistortionPulse(0.3f, 50.0f);
    }

    public virtual void SwitchState(BossBaseState newState, BossBaseState nextState) {
        health.canLoseHealth = true;
        currentState?.Exit();
        currentState = newState;

        if (newState) {
            currentState.SetNextState(nextState);
        }

        currentState?.Enter(this);
    }

    public virtual void SwitchState(BossStateEnum bossStateEnum) {
        currentStateEnum = bossStateEnum;
        health.canLoseHealth = true;

        switch(bossStateEnum) {
            case BossStateEnum.IDLE:
                SwitchState(idleState, null);
                break;
            case BossStateEnum.ATTACKING:
                int index = UnityEngine.Random.Range(0, Mathf.Clamp(currentLevel + 1, 1, attackStates.Count));
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
            case BossStateEnum.TUTORIAL:
                lightContainer.SetActive(false);
                rig.isKinematic = false;
                rig.velocity = Vector2.zero;
                anim.SetBool("Dead", true);
                health.SetHealth(0);
                col.enabled = true;
                health.canLoseHealth = false;
                StartBossTutorial?.Invoke();
                break;
        }
    }

    public void StartBattle() {
        SwitchState(idleState, attackStates[Mathf.Clamp(currentLevel, 0, attackStates.Count - 1)]);
        startBattleEvent?.Invoke();
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

public enum BossStateEnum { IDLE, ATTACKING, DEAD, NONE, TUTORIAL }
