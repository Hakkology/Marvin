using UnityEngine;

public class EnemyStats : EntityStats {

    private Enemy enemy;

    [Header("Level Details")]
    [SerializeField] private int level = 1;

    [Range(0f, 1f)]
    [SerializeField] private float percentageModifier = .4f;
    protected override void Start()
    {
        ApplyLevelModifiers();

        base.Start();
        enemy = GetComponent<Enemy>();
    }

    private void ApplyLevelModifiers() {
        // Major Stats
        Modify(strength);
        Modify(agility);
        Modify(intelligence);
        Modify(vitality);

        // Physical Offensive Stats
        Modify(damage);
        Modify(criticalChance);
        Modify(criticalDamageMultiplier);

        // Magical Offensive Stats
        Modify(fireDamage);
        Modify(iceDamage);
        Modify(lightningDamage);

        // Defensive Stats
        Modify(maxHealth);
        Modify(armor);
        Modify(evasion);
        Modify(magicResistance);
    }

    private void Modify(Stat _stat){
        for (int i = 0; i < level; i++)
        {
            float modifier = _stat.GetValue() * percentageModifier;

            _stat.AddModifier(Mathf.RoundToInt(modifier));
        }
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
        //enemy.DamageEffect();
    }
    protected override void Die()
    {
        base.Die();
        enemy.Death();
    }
}