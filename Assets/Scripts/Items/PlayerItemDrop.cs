using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemDrop : ItemDrop
{
    [Header("Player's drop")]
    [SerializeField] private float chanceToLoseItems;

    public override void GenerateDrop()
    {
        PlayerInventory inventory = PlayerInventory.Instance;
        List<InventoryItem> currentEquipment = inventory.GetEquipmentItems();
        List<InventoryItem> itemsToUnequip = new List<InventoryItem>();

        foreach (var item in currentEquipment)
        {
            if (Random.Range(0,100) <= chanceToLoseItems)
            {
                DropItem(item.data);
                itemsToUnequip.Add(item);
            }
        }

        for (int i = 0; i < itemsToUnequip.Count; i++)
        {
            inventory.UnequipItem(itemsToUnequip[i].data as EquipmentData);
        }
    }
}
