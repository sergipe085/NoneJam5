using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputMap : Singleton<InputMap>
{
    public Dictionary<ACTION, KeyCode> actionDictionary = new();
    public ActionMapping actionMapping = null;

    protected override void Awake() {
        base.Awake();

        UpdateInputMapping();
    }

    public void UpdateInputMapping() {
        foreach(ActionMap actionMap in actionMapping.actionList) {
            actionDictionary.Add(actionMap.action, actionMap.keyCode);
        }
    }
}

public enum ACTION { Jump, Attack, Dash, Move_Left, Move_Right }
