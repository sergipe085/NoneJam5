using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attackable : MonoBehaviour
{
    public GameObject owner = null;
    public event Action GetAttackEvent = null;  

    public virtual void GetAttack(int damage, Vector2 position, bool isUp = false) {
        GetAttackEvent?.Invoke();
    }
}
