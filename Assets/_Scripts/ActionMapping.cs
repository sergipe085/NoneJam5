using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActionMapping", menuName = "ScriptableObjects/ActionMapping")]
public class ActionMapping : ScriptableObject
{
    public List<ActionMap> actionList = new();
}

[Serializable]
public struct ActionMap {
    public ACTION action;
    public KeyCode keyCode;
}
