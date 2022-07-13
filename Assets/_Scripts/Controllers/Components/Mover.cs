using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rig = null;

    [Header("--- MOVEMENT PROPERTIES ---")]
    [SerializeField] private float maxVelocity = 10.0f;
    [SerializeField] private float acelleration = 50.0f;
    [SerializeField] private float desacelleration = 50.0f;
    [SerializeField] private float gravityForce = 10.0f;
    private bool canMove = true;

    [Header("--- JUMP PROPERTIES ---")]
    public float jumpForce = 100.0f;
    public float stopJumpForce = 0.2f;
    public bool IsJumping { get; private set; }

    public void Move(float direction) {
        if (direction == 0f || !canMove) return;

        rig.AddForce(acelleration * Time.deltaTime * new Vector2(direction, 0f), ForceMode2D.Impulse);
        LimitVelocity();
    }

    public void Break() {
        rig.AddForce(desacelleration * Time.deltaTime * -new Vector2(rig.velocity.x, 0f), ForceMode2D.Force);
    }

    public void LimitVelocity() {
        Vector2 velocity = new Vector2(rig.velocity.x, 0f);
        velocity = Vector2.ClampMagnitude(velocity, maxVelocity);
        rig.velocity = new Vector2(velocity.x, rig.velocity.y);
    }

    public void Gravity() {
        rig.AddForce(gravityForce * Time.deltaTime * Vector2.down, ForceMode2D.Force);
    }

    public void Jump() {
        rig.velocity = new Vector2(rig.velocity.x, 0f);
        rig.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        IsJumping = true;
    } 

    public void StopJump() {
        IsJumping = false;
    }

    public void BreakJump() {
        rig.AddForce(Vector2.down * stopJumpForce, ForceMode2D.Impulse);
    }

    public Vector2 GetVelocity() {
        return rig.velocity;
    }

    public void StopMove() {
        canMove = false;
        rig.isKinematic = true;
        rig.velocity = Vector2.zero;
    }
    public void ReturnMove() {
        canMove = true;
        rig.isKinematic = false;
    }

    public void SetIsJumping(bool isJumping) {
        IsJumping = isJumping;
    }
}
