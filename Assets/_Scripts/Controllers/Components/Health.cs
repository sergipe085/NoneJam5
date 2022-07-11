using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : Attackable
{
    [Header("--- HEALTH PROPERTIES ---")]
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private int currentHealth = 0;

    public event Action<Vector2> OnTakeDamage = null;
    public event Action OnDie = null;

    private void Start() {
        currentHealth = maxHealth;
    }

    public override void GetAttack(int damage, Vector2 position) {
        base.GetAttack(damage, position);

        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
        OnTakeDamage?.Invoke(position);
        
        CheckDead();
    }

    private void CheckDead() {
        if (currentHealth <= 0) {
            Die();
        }
    }

    private void Die() {
        OnDie?.Invoke();
        Destroy(gameObject);
    }
    
    public float GetHealthPercentage() {
        return 1.0f * currentHealth / maxHealth;
    }
}
