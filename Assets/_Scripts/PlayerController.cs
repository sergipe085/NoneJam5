using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rig = null;

    [SerializeField] private float maxVelocity = 10.0f;
    [SerializeField] private float acelleration = 50.0f;
    [SerializeField] private float desacelleration = 50.0f;
    private float moveInput = 0.0f;

    private void Update() {
        moveInput = Input.GetAxisRaw("Horizontal");

        if (moveInput != 0.0f) {
            Move(moveInput);
        }
        else {
            Break();
        }
    }

    private void Move(float direction) {
        rig.AddForce(acelleration * Time.deltaTime * new Vector2(direction, 0f), ForceMode2D.Impulse);
        Vector2 velocity = new Vector2(rig.velocity.x, 0f);
        velocity = Vector2.ClampMagnitude(velocity, maxVelocity);
        rig.velocity = new Vector2(velocity.x, rig.velocity.y);
    }

    private void Break() {
        rig.AddForce(desacelleration * Time.deltaTime * -rig.velocity, ForceMode2D.Force);
    }   
}
