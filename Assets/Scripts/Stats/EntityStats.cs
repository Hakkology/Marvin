using System;
using UnityEngine;

public class EntityStats : MonoBehaviour {
    public Stat strength;
    public Stat damage;
    public Stat maxHealth;
    [SerializeField] private int currentHealth;

    void Start() {
        currentHealth = maxHealth.GetValue();
    }

    public virtual void TakeDamage(int _damage) {
        currentHealth -= _damage;

        if (currentHealth <= 0) 
            Die();
    }

    public virtual void DoDamage(EntityStats _targetStats){
        int totalDamage = damage.GetValue() + strength.GetValue();
        _targetStats.TakeDamage(totalDamage);
    }

    protected virtual void Die()
    {
        throw new NotImplementedException();
    }
}