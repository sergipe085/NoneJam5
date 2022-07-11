using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffect : MonoBehaviour
{
    public void Finish() {
        Destroy(this.gameObject);
    }
}
