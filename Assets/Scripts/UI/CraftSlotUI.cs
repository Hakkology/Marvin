using UnityEngine.EventSystems;

public class CraftSlotUI : ItemSlotUI {
    
    void OnEnable() {
        UpdateSlot(item);
    }
    
    public override void OnPointerDown(PointerEventData eventData)
    {
        EquipmentData craftData = item.data as EquipmentData;

        PlayerInventory.Instance.CanCraft(craftData, craftData.craftingMaterials);
    }
}