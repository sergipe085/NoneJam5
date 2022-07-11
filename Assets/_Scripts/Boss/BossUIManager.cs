using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossUIManager : MonoBehaviour
{
    [SerializeField] private GameObject BossUI = null;
    [SerializeField] private TMPro.TextMeshProUGUI bossName = null;
    [SerializeField] private Slider bossHealthSlider = null;

    private BossController currentBoss = null;

    private void Start() {
        ClearBossUI();
        GameManager.Instance.OnStartBossEvent += GameManager_OnStartBoss;
    }

    private void GameManager_OnStartBoss(BossController bossController) {
        BossUI.SetActive(true);
        currentBoss = bossController;
        bossName.text = currentBoss.gameObject.name;
        UpdateHealth();
        currentBoss.GetHealth().OnTakeDamage += UpdateHealth;
    }

    private void UpdateHealth() {
        bossHealthSlider.value = currentBoss.GetHealth().GetHealthPercentage();
    }

    private void ClearBossUI() {
        BossUI.SetActive(false);
        bossName.text = "";
    }
}
