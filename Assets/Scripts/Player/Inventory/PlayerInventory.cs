using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour {
    
    public static PlayerInventory Instance { get; private set;}
    public List<InventoryItem> inventoryItems;
    public Dictionary<ItemData, InventoryItem> inventoryItemsDict;

    void Awake() {
        if (Instance == null)
            Instance = this;
        
        else
            Destroy(gameObject);
    }

    void Start() {
        inventoryItems = new List<InventoryItem>();
        inventoryItemsDict = new Dictionary<ItemData, InventoryItem> ();
    }

    public void AddItem(ItemData item){
        if (inventoryItemsDict.TryGetValue(item, out InventoryItem value)) value.AddStack();
        else {
            InventoryItem newItem = new InventoryItem(item);
            inventoryItems.Add(newItem);
            inventoryItemsDict.Add(item, newItem);
        }
    }

    public void RemoveItem(ItemData item){
        if (inventoryItemsDict.TryGetValue (item, out InventoryItem value)){
            if (value.stackSize <= 1)
            {
                inventoryItems.Remove(value);
                inventoryItemsDict.Remove(item);
            }
            else value.RemoveStack();
        }
    }
}