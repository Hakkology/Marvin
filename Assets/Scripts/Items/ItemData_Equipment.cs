using UnityEngine;


public enum EquipmentType{
    Weapon,
    Armor,
    Amulet,
    Flask
}

[CreateAssetMenu(fileName = "Items", menuName = "Items/EquipmentData", order = 0)]
public class EquipmentData : ItemData {
    
    public EquipmentType equipmentType;
}