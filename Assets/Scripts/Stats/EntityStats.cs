using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EntityStats : MonoBehaviour {
    [Header("Major Stats")]
    public Stat strength;
    public Stat agility;
    public Stat intelligence;
    public Stat vitality;

    [Header("Physical Offensive Stats")]
    public Stat damage;
    public Stat criticalChance; // agility + chance * .01f
    public Stat criticalDamageMultiplier; // strength + multipler * .01f

    [Header("Magical Offensive Stats")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightningDamage;

    [Header("Defensive Stats")]
    public Stat maxHealth;
    public Stat armor; // damage - armor
    public Stat evasion; // evasion + agility * .01f
    public Stat magicResistance;

    public bool isIgnited; // does damage over time
    public bool isShocked; // %20 decrease armor, slow
    public bool isChilled; // reduce accuracy by %20

    private float ignitedTimer;
    private float chilledTimer;
    private float shockedTimer;
    private float ignitedDamageCooldown = 1f;
    private float igniteDamageTimer;



    [SerializeField] private int currentHealth;
    protected virtual void Start() {
        criticalDamageMultiplier.SetDefaultValue(150);
        currentHealth = maxHealth.GetValue();
    }
    protected virtual void Update() {
        ignitedTimer -= Time.deltaTime;
        igniteDamageTimer -= Time.deltaTime;

        if (ignitedTimer < 0)
            isIgnited = false;

        if (chilledTimer < 0)
            isChilled = false;
        
        if (shockedTimer < 0)
            isShocked = false;
        
        if(igniteDamageTimer < 0 && isIgnited){
            Debug.Log("Burning damage on" + this.name);
            igniteDamageTimer = ignitedDamageCooldown;
        }
    }
    public virtual void TakeDamage(int _damage) {
        currentHealth -= _damage;

        Debug.Log(" " + _damage);

        if (currentHealth <= 0) 
            Die();
    }

    public virtual void DoDamage(EntityStats _targetStats)
    {
        if (TargetEvasionCheck(_targetStats))
            return;

        int totalDamage = damage.GetValue() + strength.GetValue();

        totalDamage = TargetArmorCheck(_targetStats, totalDamage);

        if (TargetCriticalDamageCheck())
            ApplyCriticalDamage(totalDamage);
        
        DoMagicalDamage(_targetStats);
    }

    public virtual void DoMagicalDamage(EntityStats _targetStats)
    {
        int elementalDamage = ApplyElementalDamage(_targetStats);
        int totalMagicalDamage = elementalDamage + intelligence.GetValue();

        totalMagicalDamage = TargetMagicResistCheck(_targetStats, totalMagicalDamage);

        _targetStats.TakeDamage(totalMagicalDamage);
    }

    private int TargetMagicResistCheck(EntityStats _targetStats, int totalMagicalDamage)
    {
        totalMagicalDamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3);
        totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);
        return totalMagicalDamage;
    }

    private int ApplyElementalDamage(EntityStats _targetStats){

        int _fireDamage = fireDamage.GetValue();
        int _frostDamage = iceDamage.GetValue();
        int _shockDamage = lightningDamage.GetValue();

        int elementalDamage = _fireDamage + _frostDamage + _shockDamage;
        CheckAilments(_fireDamage, _frostDamage, _shockDamage, _targetStats);
        return elementalDamage;
    }

    private void CheckAilments(int fire, int ice, int lightning, EntityStats _targetStats){
        
        if (Mathf.Max(fire, ice, lightning) <= 0)
            return;
            
        int maxDamage = Mathf.Max(fire, ice, lightning);
        List<int> damages = new List<int> { fire, ice, lightning };
        List<Action> ailmentsActions = new List<Action> {
            () => _targetStats.ApplyAilments(true, false, false),   // Fire
            () => _targetStats.ApplyAilments(false, true, false),  // Ice
            () => _targetStats.ApplyAilments(false, false, true)   // Lightning
        };

        var indices = damages.Select((value, index) => new { value, index })
                            .Where(x => x.value == maxDamage)
                            .Select(x => x.index)
                            .ToList();

        if (indices.Count > 0) {
            int randomIndex = UnityEngine.Random.Range(0, indices.Count); 
            ailmentsActions[indices[randomIndex]](); 
        }
    }

    public void ApplyAilments(bool _ignite, bool _chill, bool _shock){
        if (isIgnited || isChilled || isShocked)
            return;
        
        if (_ignite){
            isIgnited = _ignite;
            ignitedTimer = 8;
        }

        if (_chill){
            isChilled = _chill;
            chilledTimer = 2;
        }

        if (isShocked){
            isShocked = _shock;
            shockedTimer = 2;
        }
    }

    private bool TargetCriticalDamageCheck()
    {
        int totalCriticalChance = criticalChance.GetValue() + agility.GetValue();
        if (UnityEngine.Random.Range(0, 100) < totalCriticalChance)
            return true;
        
        return false;
    }

    private float ApplyCriticalDamage(float _totalDamage){
        float totalCriticalDamageMultiplier = (criticalDamageMultiplier.GetValue() + strength.GetValue()) * .01f;
        return Mathf.RoundToInt(_totalDamage * totalCriticalDamageMultiplier);
    }

    private int TargetArmorCheck(EntityStats _targetStats, int totalDamage)
    {
        if(_targetStats.isChilled)
            totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() * .8f);
        else
            totalDamage -= _targetStats.armor.GetValue();
        
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }

    private bool TargetEvasionCheck(EntityStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        if (isShocked)
            totalEvasion += 20;
        
        if (UnityEngine.Random.Range(0, 100) < totalEvasion)
            return true;
        
        return false;
    }

    protected virtual void Die()
    {
        
    }
}