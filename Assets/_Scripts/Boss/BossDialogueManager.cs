using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDialogueManager : MonoBehaviour
{
    [SerializeField] private DialogueSO dialogueSO = null;
    [SerializeField] private GameObject dialogueUI = null;
    [SerializeField] private TMPro.TextMeshProUGUI text = null;

    private void Awake() {
        GameManager.Instance.OnEndBossEvent += ShowDialogue;
        GameManager.Instance.OnStartBossEvent += HideDialogue;
        BossController.Instance.BeforeStartBoss += ShowDialogue;
        BossController.Instance.StartBossTutorial += ShowTutorialDialogue;
    }

    private void ShowTutorialDialogue() {
        dialogueUI.SetActive(true);
        text.text = "Tuto. Treine o quanto quiser e depois aperte (E) para batalhar comigo";
    }

    private void ShowDialogue() {
        dialogueUI.SetActive(true);

        if (BossController.Instance.IsDefeated()) {
            ShowPlayerWin();
            return;
        }

        int curLevel = BossController.Instance.GetCurrentLevel();
        if (dialogueSO.dialogues[curLevel] != null) {
            text.text = dialogueSO.dialogues[curLevel];
        }
    }

    private void ShowPlayerWin() {
        dialogueUI.SetActive(true);
        text.text = "Oh, voce me derrotou!! Foi uma boa luta. Aperte (E) para ir ao menu.";
    }

    private void HideDialogue() {
        dialogueUI.SetActive(false);
        text.text = "";
    }
}
