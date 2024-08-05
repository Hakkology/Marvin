using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObjectTrigger : MonoBehaviour
{
    private ItemObject itemObject => GetComponentInParent<ItemObject>();

    void OnTriggerEnter2D(Collider2D other) {
        var inventory = other.GetComponent<PlayerInventory>();
        if (inventory != null)
        {
            itemObject.PickupItem(inventory);
        }
    }
}
