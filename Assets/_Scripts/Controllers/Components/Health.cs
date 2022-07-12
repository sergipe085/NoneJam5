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
    public event Action OnReset = null;

    private void Start() {
        currentHealth = maxHealth;
    }

    public override void GetAttack(int damage, Vector2 position) {
        base.GetAttack(damage, position);

        if (IsDead()){
            return;
        }

        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
        OnTakeDamage?.Invoke(position);
        
        CheckDead();
    }

    private void CheckDead() {
        if (IsDead()) {
            Die();
        }
    }

    private bool IsDead() {
        return currentHealth <= 0;
    }

    private void Die() {
        OnDie?.Invoke();
        //Destroy(gameObject);
    }
    
    public float GetHealthPercentage() {
        return 1.0f * currentHealth / maxHealth;
    }

    public void SetHealth(int newHealth) {
        currentHealth = Mathf.Clamp(newHealth, 0, maxHealth);
    }

    public void Reset() {
        currentHealth = maxHealth;
        OnReset?.Invoke();
    }
}
