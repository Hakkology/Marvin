using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EntityStats : MonoBehaviour {

    [Header("References")]
    private EntityFX entityFX;
    private Entity entity;

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

    [SerializeField] private float ailmentDuration = 4;
    private float ailmentTimer;
    private float ignitedDamageCooldown = 1.2f;
    private float igniteDamageTimer;
    private int igniteDamage;
    private int shockDamage;
    [SerializeField] private GameObject shockPrefab;
    private float slowPercetange = .2f;

    public int currentHealth;
    public Action onHealthChanged;
    public bool isDead {get; private set;}

    void Awake() {
        entityFX = GetComponent<EntityFX>();
        entity = GetComponent<Entity>();
    }

    protected virtual void Start() {
        criticalDamageMultiplier.SetDefaultValue(150);
        currentHealth = GetMaxHealthValue();
    }
    protected virtual void Update() {
        ailmentTimer -= Time.deltaTime;
        igniteDamageTimer -= Time.deltaTime;

        if (ailmentTimer < 0){
            isIgnited = false;
            isChilled = false;
            isShocked = false;
        }
        
        if(igniteDamageTimer < 0 && isIgnited){
            DecreaseHealth(igniteDamage);
            igniteDamageTimer = ignitedDamageCooldown;
            if (currentHealth <= 0 && !isDead) 
                Die();
        }
    }
    public virtual void TakeDamage(int _damage) {

        DecreaseHealth(_damage);
        GetComponent<Entity>().DamageEffect();
        if (currentHealth <= 0 && !isDead) 
            Die();
    }

    protected virtual void DecreaseHealth(int _amount){
        currentHealth -= _amount;

        if (onHealthChanged != null)
            onHealthChanged();
    }

    public virtual void DoDamage(EntityStats _targetStats)
    {
        if (TargetEvasionCheck(_targetStats))
            return;

        int totalDamage = damage.GetValue() + strength.GetValue();

        totalDamage = TargetArmorCheck(_targetStats, totalDamage);

        if (TargetCriticalDamageCheck())
            ApplyCriticalDamage(totalDamage);
        
        _targetStats.TakeDamage(totalDamage);
    }

    #region Magical Damage and Ailments
    public virtual void DoMagicalDamage(EntityStats _targetStats)
    {
        int elementalDamage = ApplyElementalDamage(_targetStats);
        int totalMagicalDamage = elementalDamage + intelligence.GetValue();

        totalMagicalDamage = TargetMagicResistCheck(_targetStats, totalMagicalDamage);

        _targetStats.TakeDamage(totalMagicalDamage);
    }

    private int TargetMagicResistCheck(EntityStats _targetStats, int totalMagicalDamage)
    {
        float totalMagicResistance = _targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3);
        totalMagicalDamage *= Mathf.RoundToInt( (100-totalMagicResistance) / 100);
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
        
        int maxDamage = Mathf.Max(fire, ice, lightning);
        if (maxDamage <= 0)
            return;

        List<Action> possibleActions = new List<Action>();

        if (fire == maxDamage) {
            possibleActions.Add(() => {
                _targetStats.ApplyAilments(true, false, false);
                _targetStats.ApplyIgniteDamage(Mathf.RoundToInt(fire * .1f)); 
            });
        }
        if (ice == maxDamage) 
            possibleActions.Add(() => {
                _targetStats.ApplyAilments(false, true, false);
                _targetStats.entity.SlowEntityBy(slowPercetange, ailmentDuration);
                });
        
        if (lightning == maxDamage) 
            possibleActions.Add(() => {
                _targetStats.ApplyAilments(false, false, true);
                _targetStats.ApplyShockDamage(Mathf.RoundToInt(lightning * .6f));
                });
        

        if (possibleActions.Count > 0) {
            int randomIndex = UnityEngine.Random.Range(0, possibleActions.Count);
            possibleActions[randomIndex]();
        }
    }

    public void ApplyAilments(bool _ignite, bool _chill, bool _shock){
        bool canApplyIgnite = !isIgnited && !isChilled && !isShocked;
        bool canApplyChill = !isIgnited && !isChilled && !isShocked;
        bool canApplyShock = !isIgnited && !isChilled;
        
        if (_ignite && canApplyIgnite)
            ApplyBurn();
        
        if (_chill && canApplyChill)
            ApplyChill();
        
        if (_shock && canApplyShock){

            if(!isShocked)
                ApplyShock();
            else
            {
                if (GetComponent<Player>() != null)
                    return;

                ApplyLightningStrike();
            }
        }
    }

    public void ApplyBurn()
    {
        ailmentTimer = ailmentDuration;
        entityFX.IgniteFXFor(ailmentDuration);
        isIgnited = true;
    }
    public void ApplyIgniteDamage(int _damage) => igniteDamage = _damage;

    public void ApplyChill()
    {
        ailmentTimer = ailmentDuration;
        entityFX.ChillFxFor(ailmentDuration);
        isChilled = true;
    }
    public void ApplyShockDamage(int _damage) => shockDamage = _damage;
    public void ApplyShock()
    {
        ailmentTimer = ailmentDuration;
        entityFX.ShockFXFor(ailmentDuration);
        isShocked = true;
    }

    private void ApplyLightningStrike()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (var hit in colliders)
        {
            Enemy enemy = hit.GetComponent<Enemy>();

            if (enemy != null && Vector2.Distance(transform.position, hit.transform.position) > 1)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = enemy.transform;
                }
            }

            if (closestEnemy == null)
                closestEnemy = transform;

        }

        if (closestEnemy != null)
        {
            EntityStats enemyStats = closestEnemy.GetComponent<EntityStats>();
            GameObject newShockStrike = Instantiate(shockPrefab, transform.position, Quaternion.identity);
            newShockStrike.GetComponent<ThunderStrikeController>().SetupThunderStrike(shockDamage, enemyStats);
        }
    }

    #endregion

    #region Stat Calculations
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

    public int GetMaxHealthValue() => maxHealth.GetValue() + vitality.GetValue() * 5;

    #endregion

    protected virtual void Die()
    {
        isDead = true;
    }
}