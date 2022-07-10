using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    private GameObject owner = null;

    public void Initialize(GameObject _owner, float size) {
        this.owner = _owner;
        transform.localScale = new Vector2(size, size);
    }
}
