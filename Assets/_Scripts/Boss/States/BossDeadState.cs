using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDeadState : BossBaseState
{
    Vector2 initialPosition;

    private void Start() {
        initialPosition = transform.position;
    }

    public override void Update() {
        base.Update();

        if (!isActive) return;

        transform.position = Vector2.Lerp(transform.position, initialPosition, 3.0f * Time.deltaTime);
    }
}
