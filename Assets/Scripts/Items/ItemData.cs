using UnityEngine;

public enum ItemType
{
    Material,
    Equipment
}


[CreateAssetMenu(fileName = "Items", menuName = "Items/ItemData", order = 0)]
public class ItemData : ScriptableObject {
    
    public ItemType itemType;
    public string ItemName;
    public Sprite Icon;
}