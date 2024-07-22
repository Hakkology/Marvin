using UnityEngine;

[CreateAssetMenu(fileName = "Items", menuName = "Items/ItemData", order = 0)]
public class ItemData : ScriptableObject {
    
    public string ItemName;
    public Sprite Icon;
}