using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [Header("--- CORE ---")]
    [SerializeField] private BossController currentBoss = null;

    public event Action<BossController> OnStartBossEvent = null;
    public event Action OnEndBossEvent = null;

    public void StartBoss(BossController boss) {
        currentBoss = boss;
        currentBoss.SwitchState(BossStateEnum.ATTACKING);
        currentBoss.GetHealth().OnDie += OnCurrentBossDie;
        OnStartBossEvent?.Invoke(boss);
    }

    public bool IsBattling() {
        return currentBoss != null;
    }

    public BossController GetCurrentBoss() {
        return currentBoss;
    }

    private void OnCurrentBossDie() {
        currentBoss = null;
        OnEndBossEvent?.Invoke();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Backspace)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
