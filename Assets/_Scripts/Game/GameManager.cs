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

    protected override void Awake() {
        base.Awake();
        currentBoss.GetHealth().OnDie += OnCurrentBossDie;
        currentBoss.SetCurrentLevel(PlayerPrefs.GetInt("bossCurrentLevel"));
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

        PlayerPrefs.SetInt("bossCurrentLevel", currentBoss.GetCurrentLevel());
        PlayerPrefs.Save();

        if (currentBoss.IsDefeated()) {
            Debug.Log("WIN");
            StartCoroutine(PlayerWinEnumerator());
        }
    }

    private IEnumerator PlayerWinEnumerator() {
        GameSceneManager.Instance.LoadScene("MenuScene");
        yield break;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Backspace)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            if (IsBattling() || currentBoss.IsDefeated() || BossUIManager.Instance.isChanging) return;

            StartBoss();
        }

        if (Input.GetKeyDown(KeyCode.H)) {
            PlayerPrefs.SetInt("bossCurrentLevel", 0);
            currentBoss.SetCurrentLevel(0);
        }
    }
}
