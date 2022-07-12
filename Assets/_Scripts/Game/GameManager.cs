using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [Header("--- CORE ---")]
    [SerializeField] private BossController currentBoss = null;

    public event Action OnStartBossEvent = null;
    public event Action OnEndBossEvent = null;

    private void Start() {
        currentBoss = BossController.Instance;
        currentBoss.GetHealth().OnDie += OnCurrentBossDie;
    }

    public void StartBoss() {
        currentBoss.Initialize();
        currentBoss.StartBattle();
        OnStartBossEvent?.Invoke();
    }

    public bool IsBattling() {
        if (currentBoss.GetBossStateEnum() != BossStateEnum.DEAD && currentBoss.GetBossStateEnum() != BossStateEnum.NONE) {
            return true;
        }

        return false;
    }

    public BossController GetCurrentBoss() {
        return currentBoss;
    }

    private void OnCurrentBossDie() {
        currentBoss.LevelUp();
        OnEndBossEvent?.Invoke();

        if (currentBoss.IsDefeated()) {
            Debug.Log("WIN");
            StartCoroutine(PlayerWinEnumerator());
        }
    }

    private IEnumerator PlayerWinEnumerator() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        yield break;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Backspace)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            if (IsBattling() || currentBoss.IsDefeated()) return;

            StartBoss();
        }
    }
}
