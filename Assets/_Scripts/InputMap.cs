using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputMap : Singleton<InputMap>
{
    public Dictionary<ACTION, KeyCode> actionDictionary = new();
    public List<ActionMap> actionList = new();

    protected override void Awake() {
        base.Awake();

        foreach(ActionMap actionMap in actionList) {
            actionDictionary.Add(actionMap.action, actionMap.keyCode);
        }
    }

    [Serializable]
    public struct ActionMap {
        public ACTION action;
        public KeyCode keyCode;
    }
}

public enum ACTION { Jump, Attack, Dash }
