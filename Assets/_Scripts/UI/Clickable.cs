using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Clickable : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private Vector3 hoverScale = Vector3.zero;
    [SerializeField] private float animDuration = 0.1f;

    private void Awake() {
        DOTween.Init();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        transform.DOPunchScale(hoverScale, 0.1f);
        SoundManager.Instance.PlayButtonSound();
    }
}
