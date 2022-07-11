using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ControllerProperties_", menuName = "ScriptableObjects/ControllerProperties")]
public class ControllerProperties : ScriptableObject
{
    [Header("--- MOVEMENT ---")]
    public Vector2 landScale = Vector2.zero;
    public Vector2 jumpScale = Vector2.zero;
    
    [Header("--- TAKE DAMAGE ---")]
    public float stopTimeDuration = 0.1f;
    public float ldTakeDamageSpeed = 30.0f;
    public float ldTakeDamageForce = 0.6f;
    public float dashTakeDamageForce = 18.0f;
    public float dashTakeDamageLength = 0.1f;

    [Header("--- ATTACK ---")]
    public float stopAttackTime = 0.0f;
}
