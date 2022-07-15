using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : Attackable
{
    [Header("--- HEALTH PROPERTIES ---")]
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private int currentHealth = 0;

    [Header("--- GAME FEEL ---")]
    [SerializeField] private ParticleSystem damageParticle = null;

    public event Action<Vector2> OnTakeDamage = null;
    public event Action OnDie = null;
    public event Action OnReset = null;

    public event Action OnHealthChanged = null;

    public bool canLoseHealth = true;

    private void Awake() {
        Reset();
    }

    public override void GetAttack(int damage, Vector2 position, bool isUp = false) {
        base.GetAttack(damage, position, isUp);

        if (damageParticle) {
            ParticleSystem particleSystem = Instantiate(damageParticle, transform.position, Quaternion.identity);
            particleSystem.Play();
        }

        if (IsDead()){
            return;
        }

        if (canLoseHealth) {
            currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
            OnHealthChanged?.Invoke();
            CheckDead();
        }
        
        OnTakeDamage?.Invoke(isUp ? (Vector2)transform.position + Vector2.down : position);
        
    }

    private void CheckDead() {
        if (IsDead()) {
            Die();
        }
    }

    public bool IsDead() {
        return currentHealth <= 0;
    }

    private void Die() {
        OnDie?.Invoke();
        //Destroy(gameObject);
    }
    
    public float GetHealthPercentage() {
        return 1.0f * currentHealth / maxHealth;
    }

    public int GetCurrentHealth() {
        return currentHealth;
    }

    public void SetHealth(int newHealth) {
        currentHealth = Mathf.Clamp(newHealth, 0, maxHealth);
    }

    public void Reset() {
        currentHealth = maxHealth;
        OnReset?.Invoke();
        OnHealthChanged?.Invoke();
    }

    public void ChangeMaxHealth(int newMaxHealth) {
        maxHealth = newMaxHealth;
        Reset();
    }
}
