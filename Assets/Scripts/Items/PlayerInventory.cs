using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour {
    
    public static PlayerInventory Instance { get; private set;}

public List<InventoryItem> equipmentItems = new List<InventoryItem>();
    public Dictionary<EquipmentData, InventoryItem> equipmentItemsDict = new Dictionary<EquipmentData, InventoryItem>();

    public List<InventoryItem> inventoryItems = new List<InventoryItem>();
    public Dictionary<ItemData, InventoryItem> inventoryItemsDict = new Dictionary<ItemData, InventoryItem>();

    public List<InventoryItem> stashItems = new List<InventoryItem>();
    public Dictionary<ItemData, InventoryItem> stashItemsDict = new Dictionary<ItemData, InventoryItem>();


    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;
    [SerializeField] private Transform equipmentSlotParent;

    private ItemSlotUI[] InventoryItemSlot; 
    private ItemSlotUI[] StashItemSlot;
    private EquipmentSlotUI[] EquipmentItemSlot;

    void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        InitializeSlots();
    }

    private void InitializeSlots()
    {
        InventoryItemSlot = inventorySlotParent.GetComponentsInChildren<ItemSlotUI>();
        StashItemSlot = stashSlotParent.GetComponentsInChildren<ItemSlotUI>();
        EquipmentItemSlot = equipmentSlotParent.GetComponentsInChildren<EquipmentSlotUI>();
    }

    public void EquipItem(ItemData _item)
    {
        if (!(_item is EquipmentData equipment)) return;
        EquipmentData newEquipment = _item as EquipmentData;
        InventoryItem newItem = new InventoryItem(newEquipment);

        EquipmentData oldEquipment = null;

        foreach (KeyValuePair<EquipmentData, InventoryItem> item in equipmentItemsDict)
        {
            if (item.Key.equipmentType == newEquipment.equipmentType)
            {
                oldEquipment = item.Key;
            }
        }

        UnequipItem(oldEquipment);

        equipmentItems.Add(newItem);
        equipmentItemsDict.Add(newEquipment, newItem);
        UpdateEquipmentUI();

        RemoveInventoryItem(_item);
    }

    private void UnequipItem(EquipmentData oldEquipment)
    {
        if (oldEquipment != null)
        {
            if (equipmentItemsDict.TryGetValue(oldEquipment, out InventoryItem value))
            {
                equipmentItems.Remove(value);
                equipmentItemsDict.Remove(oldEquipment);
                AddToInventory(oldEquipment);
            }
        }
    }

    public void AddItem(ItemData item) {
        switch (item.itemType) {
            case ItemType.Equipment:
                AddToInventory(item);
                break;
            case ItemType.Material:
                AddToStash(item);
                break;
        }
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
        for (int i = 0; i < InventoryItemSlot.Length; i++)
        {
            InventoryItemSlot[i].CleanUpSlot();
        }

        for (int i = 0; i < inventoryItems.Count; i++)
        {
            InventoryItemSlot[i].UpdateSlot(inventoryItems[i]);
        }
    }

    private void UpdateStashUI()
    {
        for (int i = 0; i < StashItemSlot.Length; i++)
        {
            InventoryItemSlot[i].CleanUpSlot();
        }

        for (int i = 0; i < stashItems.Count; i++)
        {
            StashItemSlot[i].UpdateSlot(stashItems[i]);
        }
    }

    void UpdateEquipmentUI()
    {
        for (int i = 0; i < EquipmentItemSlot.Length; i++)
        {
            foreach (KeyValuePair<EquipmentData, InventoryItem> item in equipmentItemsDict)
            {
                if (item.Key.equipmentType == EquipmentItemSlot[i].equipmentType)
                {
                    EquipmentItemSlot[i].UpdateSlot(item.Value);
                }
            }
        }
    }
}