using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class BossDialogueManager : MonoBehaviour
{
    [SerializeField] private DialogueSO dialogueSO = null;
    [SerializeField] private RectTransform dialogueUI = null;
    [SerializeField] private TMPro.TextMeshProUGUI text = null;

    private Coroutine showDialogueCoroutine = null;
    private string currentMsg = null;

    private void Start() {
        DOTween.Init();
    }

    private void Awake() {
        // GameManager.Instance.OnEndBossEvent += ShowDialogue;
        GameManager.Instance.OnStartBossEvent += HideDialogue;
        BossController.Instance.BeforeStartBoss += ShowDialogue;
        BossController.Instance.StartBossTutorial += ShowTutorialDialogue;

        BossUIManager.Instance.StopHiding += ShowDialogue;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            if (showDialogueCoroutine != null) {
                StopCoroutine(showDialogueCoroutine);
                BossUIManager.Instance.isChanging = false;
                text.text = currentMsg;
            }
        }
    }

    private void ShowTutorialDialogue() {
        CallCoroutine(dialogueSO.tutorialDialogue);
        //"Move (wasd, seta) \nPula (z, l, espaco) \nDash - (k, x, mouse1, lshift) \nAtaca (j, c, mouse0).\nTreine o quanto quiser e depois aperte (E) para batalhar comigo."
    }

    private void ShowDialogue() {
        if (BossController.Instance.IsDefeated()) {
            ShowPlayerWin();
            return;
        }

        int curLevel = BossController.Instance.GetCurrentLevel();
        if (dialogueSO.dialogues[curLevel] != null) {
            CallCoroutine(dialogueSO.dialogues[curLevel]);
        }
    }

    private void ShowPlayerWin() {
        CallCoroutine("Oh, voce me derrotou!! Foi uma boa luta. Aperte (E) para ir ao menu.");
    }

    public void HideDialogue() {
        StartCoroutine(HideDialogEnumerator());
    }

    private void CallCoroutine(string msg) {
        showDialogueCoroutine = StartCoroutine(ShowDialogEnumerator(msg));
        currentMsg = msg;
    }

    private IEnumerator ShowDialogEnumerator(string message) {

        BossUIManager.Instance.isChanging = true;
        dialogueUI.gameObject.SetActive(false);
        text.text = "";

        yield return new WaitForSeconds(0.5f);

        dialogueUI.DOLocalMoveY(-190, 1.0f).SetEase(Ease.OutCubic).From(-1000f);
        dialogueUI.gameObject.SetActive(true);

        yield return new WaitForSeconds(1.0f);

        string curText = "";
        foreach (char a in message) {
            curText += a;
            text.text = curText;
            yield return new WaitForSeconds(0.04f);
        }

        BossUIManager.Instance.isChanging = false;
    }

    private IEnumerator HideDialogEnumerator() {
        dialogueUI.transform.DOLocalMoveY(-1000f, 1.0f).SetEase(Ease.OutCubic).OnComplete(() => { 
            dialogueUI.gameObject.SetActive(false);
            text.text = "";
        });
        yield return null;
    }
}
