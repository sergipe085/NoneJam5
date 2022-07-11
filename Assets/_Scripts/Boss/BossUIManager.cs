using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BossUIManager : MonoBehaviour
{
    [SerializeField] private GameObject bossUI = null;
    [SerializeField] private TMPro.TextMeshProUGUI bossName = null;
    [SerializeField] private Slider bossHealthSlider = null;

    private BossController currentBoss = null;

    private void Start() {
        ClearBossUI();

        GameManager.Instance.OnStartBossEvent += GameManager_OnStartBoss;
        GameManager.Instance.OnEndBossEvent += GameManager_OnEndBoss;

        DOTween.Init(DOTween.defaultAutoKill);
    }

    private void GameManager_OnStartBoss(BossController bossController) {
        currentBoss = bossController;
        currentBoss.GetHealth().OnTakeDamage += (pos) => UpdateHealth();

        StartCoroutine(ShowBossUI(currentBoss.gameObject.name));
    }

    private void GameManager_OnEndBoss() {
        StartCoroutine(HideBossUI());
    }

    private void UpdateHealth() {
        bossHealthSlider.value = currentBoss.GetHealth().GetHealthPercentage();
    }

    private void ClearBossUI() {
        bossUI.SetActive(false);
    }

    private IEnumerator HideBossUI() {
        yield return new WaitForSeconds(0.4f);

        for(int i = bossName.text.Length - 1; i >= 0; i--) {
            bossName.text = bossName.text.Remove(i);
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(0.2f);

        bossHealthSlider.transform.DOScaleX(0, 0.8f).OnComplete(ClearBossUI);
    }

    private IEnumerator ShowBossUI(string _bossName) {
        bossUI.SetActive(true);
        bossHealthSlider.value = 0.0f;
        bossName.text = "";

        foreach(char a in _bossName) {
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
    }
}
