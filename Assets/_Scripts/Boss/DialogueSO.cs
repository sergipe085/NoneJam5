using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue_", menuName = "ScriptableObjects/Dialogue")]
public class DialogueSO : ScriptableObject
{
    public List<string> dialogues = new();
    public string tutorialDialogue = "";
}
