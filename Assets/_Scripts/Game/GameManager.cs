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

    public void StartBoss(BossController boss) {
        Debug.Log("Start Boss " + boss.gameObject.name);
        currentBoss = boss;
        currentBoss.SwitchState(BossStateEnum.ATTACKING);
        OnStartBossEvent?.Invoke(boss);
    }

    public bool IsBattling() {
        return currentBoss != null;
    }

    public BossController GetCurrentBoss() {
        return currentBoss;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Backspace)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
