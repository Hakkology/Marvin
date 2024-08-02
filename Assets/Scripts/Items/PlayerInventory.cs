using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour {
    
    public static PlayerInventory Instance { get; private set;}
    public List<InventoryItem> inventoryItems;
    public Dictionary<ItemData, InventoryItem> inventoryItemsDict;

    public List<InventoryItem> stashItems;
    public Dictionary<ItemData, InventoryItem> stashItemsDict;

    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;

    private ItemSlotUI[] InventoryItemSlot; 
    private ItemSlotUI[] StashItemSlot;

    void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start() {
        inventoryItems = new List<InventoryItem>();
        inventoryItemsDict = new Dictionary<ItemData, InventoryItem> ();

        stashItems = new List<InventoryItem> ();
        stashItemsDict = new Dictionary<ItemData, InventoryItem> ();

        InventoryItemSlot = inventorySlotParent.GetComponentsInChildren<ItemSlotUI>();
        StashItemSlot = stashSlotParent.GetComponentsInChildren<ItemSlotUI>();
    }

    public void AddItem(ItemData item)
    {
        if (item.itemType == ItemType.Equipment)
            AddToInventory(item);
        else if (item.itemType == ItemType.Material)
            AddToStash(item);
    }

    private void AddToInventory(ItemData item)
    {
        if (inventoryItemsDict.TryGetValue(item, out InventoryItem value)) 
            value.AddStack();
        else
        {
            InventoryItem newItem = new InventoryItem(item);
            inventoryItems.Add(newItem);
            inventoryItemsDict.Add(item, newItem);
        }
        UpdateInventoryUI();
    }

    private void AddToStash(ItemData item)
    {
        if (stashItemsDict.TryGetValue(item, out InventoryItem value)) 
            value.AddStack();
        else
        {
            InventoryItem newItem = new InventoryItem(item);
            stashItems.Add(newItem);
            stashItemsDict.Add(item, newItem);
        }
        UpdateStashUI();
    }

    public void RemoveInventoryItem(ItemData item)
    {
        if (inventoryItemsDict.TryGetValue (item, out InventoryItem value)){
            if (value.stackSize <= 1)
            {
                inventoryItems.Remove(value);
                inventoryItemsDict.Remove(item);
            }
            else value.RemoveStack();
        }

        UpdateInventoryUI();
    }

    public void RemoveStashItem(ItemData item)
    {
        if (stashItemsDict.TryGetValue (item, out InventoryItem value)){
            if (value.stackSize <= 1)
            {
                stashItems.Remove(value);
                stashItemsDict.Remove(item);
            }
            else value.RemoveStack();
        }

        UpdateStashUI();
    }

    private void UpdateInventoryUI()
    {
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            InventoryItemSlot[i].UpdateSlot(inventoryItems[i]);
        }
    }

    private void UpdateStashUI()
    {
        for (int i = 0; i < stashItems.Count; i++)
        {
            StashItemSlot[i].UpdateSlot(stashItems[i]);
        }
    }
}