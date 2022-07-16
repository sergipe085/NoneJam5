using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private Slider playerHelthSlider = null;
    [SerializeField] private RectTransform playerImage = null;
    [SerializeField] private Transform playerHealth = null;

    private void Start() {
        PlayerController.Instance.GetHealth().OnHealthChanged += PlayerController_OnHealthChanged;
        GameManager.Instance.OnStartBossEvent += ShowPlayerHealthVisual;
        GameManager.Instance.OnEndBossEvent += HidePlayerHealthVisual;
        GameManager.Instance.OnPlayerDieEvent += ClearUI;
        PlayerController.Instance.GetHealth().OnTakeDamage += (pos) => playerImage.transform.DOShakeScale(0.2f);

        UpdateHealthVisual();
        HidePlayerHealthVisual();
    }

    private void PlayerController_OnHealthChanged() {
        UpdateHealthVisual();
    }

    private void UpdateHealthVisual() {
        playerHelthSlider.DOValue(PlayerController.Instance.GetHealth().GetHealthPercentage(), 0.3f).SetEase(Ease.OutCubic);
    }

    private void ShowPlayerHealthVisual() {
        playerHealth.gameObject.SetActive(true);
        playerHealth.DOLocalMoveY(0, 1.0f).From(1000).SetEase(Ease.OutCubic);
    }

    private void HidePlayerHealthVisual() {
        playerHealth.DOLocalMoveY(1000, 1.0f).From(0).SetEase(Ease.OutCubic).OnComplete(() => playerHealth.gameObject.SetActive(false));
    }

    private void ClearUI() {
        gameObject.SetActive(false);
    }
}
