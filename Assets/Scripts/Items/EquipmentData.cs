using UnityEngine;
using System.Collections.Generic;


public enum EquipmentType{
    Weapon,
    Armor,
    Amulet,
    Flask
}

[CreateAssetMenu(fileName = "Items", menuName = "Items/EquipmentData", order = 0)]
public class EquipmentData : ItemData {
    
    public EquipmentType equipmentType;
    public ItemEffect[] itemEffects;
    [Header("Major Stats")]
    public int strength;
    public int agility;
    public int intelligence;
    public int vitality;

    [Header("Physical Offensive Stats")]
    public int damage;
    public int criticalChance; // agility + chance * .01f
    public int criticalDamageMultiplier; // strength + multipler * .01f

    [Header("Magical Offensive Stats")]
    public int fireDamage;
    public int iceDamage;
    public int lightningDamage;


    [Header("Defensive Stats")]
    public int maxHealth;
    public int armor; // damage - armor
    public int evasion; // evasion + agility * .01f
    public int magicResistance;

    [Header("Craft Requirements")]
    public List<InventoryItem> craftingMaterials;

    public void ExecuteItemEffect(){
        foreach (var item in itemEffects)
        {
            item.ExecuteEffect();
        }
    }

    public void AddModifiers() {
        PlayerStats playerStats = PlayerManager.Instance.player.GetComponent<PlayerStats>();

        playerStats.strength.AddModifier(strength);
        playerStats.agility.AddModifier(agility);
        playerStats.intelligence.AddModifier(intelligence);
        playerStats.vitality.AddModifier(vitality);
        playerStats.maxHealth.AddModifier(maxHealth);

        playerStats.damage.AddModifier(damage);
        playerStats.criticalChance.AddModifier(criticalChance); 
        playerStats.criticalDamageMultiplier.AddModifier(criticalDamageMultiplier);

        playerStats.fireDamage.AddModifier(fireDamage);
        playerStats.iceDamage.AddModifier(iceDamage);
        playerStats.lightningDamage.AddModifier(lightningDamage);

        playerStats.armor.AddModifier(armor);
        playerStats.evasion.AddModifier(evasion);
        playerStats.magicResistance.AddModifier(magicResistance);
    }

    public void RemoveModifiers() {
        PlayerStats playerStats = PlayerManager.Instance.player.GetComponent<PlayerStats>();

        playerStats.strength.RemoveModifier(strength);
        playerStats.agility.RemoveModifier(agility);
        playerStats.intelligence.RemoveModifier(intelligence);
        playerStats.vitality.RemoveModifier(vitality);
        playerStats.maxHealth.RemoveModifier(maxHealth);

        playerStats.damage.RemoveModifier(damage);
        playerStats.criticalChance.RemoveModifier(criticalChance);
        playerStats.criticalDamageMultiplier.RemoveModifier(criticalDamageMultiplier);

        playerStats.fireDamage.RemoveModifier(fireDamage);
        playerStats.iceDamage.RemoveModifier(iceDamage);
        playerStats.lightningDamage.RemoveModifier(lightningDamage);

        playerStats.armor.RemoveModifier(armor);
        playerStats.evasion.RemoveModifier(evasion);
        playerStats.magicResistance.RemoveModifier(magicResistance);
    }
}