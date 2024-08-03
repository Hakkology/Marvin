using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSlotUI : ItemSlotUI
{
    public EquipmentType equipmentType;

    void OnValidate() {
        gameObject.name = "Equipment Slot - " + equipmentType.ToString();
    }
}
