using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentSlotUI : ItemSlotUI
{
    public EquipmentType equipmentType;

    void OnValidate() {
        gameObject.name = "Equipment Slot - " + equipmentType.ToString();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        PlayerInventory.Instance.UnequipItem(item.data as EquipmentData);
        CleanUpSlot();
    }
}
