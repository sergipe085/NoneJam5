using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class BossDialogueManager : MonoBehaviour
{
    [SerializeField] private DialogueSO dialogueSO = null;
    [SerializeField] private GameObject dialogueUI = null;
    [SerializeField] private TMPro.TextMeshProUGUI text = null;

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

    private void ShowTutorialDialogue() {
        StartCoroutine(ShowDialogEnumerator(dialogueSO.tutorialDialogue));
        //"Move (wasd, seta) \nPula (z, l, espaco) \nDash - (k, x, mouse1, lshift) \nAtaca (j, c, mouse0).\nTreine o quanto quiser e depois aperte (E) para batalhar comigo."
    }

    private void ShowDialogue() {
        if (BossController.Instance.IsDefeated()) {
            ShowPlayerWin();
            return;
        }

        int curLevel = BossController.Instance.GetCurrentLevel();
        if (dialogueSO.dialogues[curLevel] != null) {
            StartCoroutine(ShowDialogEnumerator(dialogueSO.dialogues[curLevel]));
        }
    }

    private void ShowPlayerWin() {
        StartCoroutine(ShowDialogEnumerator("Oh, voce me derrotou!! Foi uma boa luta. Aperte (E) para ir ao menu."));
    }

    public void HideDialogue() {
        StartCoroutine(HideDialogEnumerator());
    }

    private IEnumerator ShowDialogEnumerator(string message) {

        BossUIManager.Instance.isChanging = true;
        dialogueUI.SetActive(false);
        text.text = "";

        yield return new WaitForSeconds(0.5f);

        dialogueUI.transform.DOMoveY(200, 1.0f).From(-1000f);
        dialogueUI.SetActive(true);

        yield return new WaitForSeconds(1.0f);

        string curText = "";
        foreach (char a in message + "\nAperte(E) para continuar...") {
            curText += a;
            text.text = curText;
            yield return new WaitForSeconds(0.04f);
        }

        BossUIManager.Instance.isChanging = false;
    }

    private IEnumerator HideDialogEnumerator() {
        dialogueUI.transform.DOMoveY(-1000f, 1.0f).OnComplete(() => { 
            dialogueUI.SetActive(false);
            text.text = "";
        });
        yield return null;
    }
}
