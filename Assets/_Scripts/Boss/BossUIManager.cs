using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class BossUIManager : Singleton<BossUIManager>
{
    [SerializeField] private GameObject bossUI = null;
    [SerializeField] private TMPro.TextMeshProUGUI bossName = null;
    [SerializeField] private Slider bossHealthSlider = null;

    private BossController currentBoss = null;

    public bool isChanging = false;

    public event Action StopHiding = null;

    private void Start() {
        ClearBossUI();

        GameManager.Instance.OnStartBossEvent += GameManager_OnStartBoss;
        GameManager.Instance.OnEndBossEvent += GameManager_OnEndBoss;

        DOTween.Init(DOTween.defaultAutoKill);
    }

    private void GameManager_OnStartBoss() {
        currentBoss = BossController.Instance;
        currentBoss.GetHealth().OnTakeDamage += (pos) => UpdateHealth();
        currentBoss.GetHealth().OnReset += UpdateHealth;

        StartCoroutine(ShowBossUI(currentBoss.gameObject.name));
    }

    private void GameManager_OnEndBoss() {
        currentBoss = BossController.Instance;
        currentBoss.GetHealth().OnTakeDamage -= (pos) => UpdateHealth();
        currentBoss.GetHealth().OnReset -= UpdateHealth;

        StartCoroutine(HideBossUI());
    }

    private void UpdateHealth() {
        bossHealthSlider.value = currentBoss.GetHealth().GetHealthPercentage();
    }

    private void ClearBossUI() {
        bossUI.SetActive(false);
    }

    private IEnumerator HideBossUI() {
        isChanging = true;

        yield return new WaitForSeconds(0.4f);

        for(int i = bossName.text.Length - 1; i >= 0; i--) {
            if (i < bossName.text.Length) bossName.text = bossName.text.Remove(i);

            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(0.2f);

        bossHealthSlider.transform.DOScaleX(0, 0.8f).OnComplete(() => {
            bossUI.SetActive(false);
            isChanging = false;
            StopHiding?.Invoke();
        });
    }

    private IEnumerator ShowBossUI(string _bossName) {
        isChanging = true;

        bossUI.SetActive(true);
        bossHealthSlider.value = 0.0f;
        bossName.text = "";

        bossHealthSlider.transform.DOScaleX(1, 0.8f);

        foreach(char a in _bossName + " (Level " + (currentBoss.GetCurrentLevel() + 1) +")") {
            bossName.text += a;
            yield return new WaitForSeconds(0.1f);
        }

        float b = 0;
        while (b < currentBoss.GetHealth().GetHealthPercentage() - 0.01f) {
            b = Mathf.Lerp(b, currentBoss.GetHealth().GetHealthPercentage(), Time.deltaTime * 4.0f);
            bossHealthSlider.value = b;
            yield return null;
        }
        bossHealthSlider.value = currentBoss.GetHealth().GetHealthPercentage();

        isChanging = false;
    }
}
