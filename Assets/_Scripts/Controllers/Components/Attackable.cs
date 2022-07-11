using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attackable : MonoBehaviour
{
    public event Action GetAttackEvent = null;  

    public virtual void GetAttack(int damage, Vector2 position) {
        GetAttackEvent?.Invoke();
    }
}
