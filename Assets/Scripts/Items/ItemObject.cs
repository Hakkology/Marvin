using UnityEngine;

public class ItemObject : MonoBehaviour {
    [SerializeField] private ItemData itemData;

    void OnValidate() {
        if (itemData != null) {
            var spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null) {
                spriteRenderer.sprite = itemData.Icon;
                gameObject.name = "Item - " + itemData.name;
            } else {
                Debug.LogWarning("SpriteRenderer component not found on the same GameObject as ItemObject.");
            }
        } else {
            
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        var inventory = other.GetComponent<PlayerInventory>();
        if (inventory != null){
            inventory.AddItem(itemData);
            Destroy(gameObject);
        }
    }
}