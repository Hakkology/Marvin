using UnityEngine;

public class ItemObject : MonoBehaviour {
    [SerializeField] private ItemData itemData;
    private SpriteRenderer sr;
    private PlayerInventory inventory;

    void Awake() {
        inventory = PlayerInventory.Instance;
    }

    void OnValidate() {
         GetComponent<SpriteRenderer>().sprite = itemData.Icon;
         gameObject.name = "Item - " + itemData.name;
    }

    void OnTriggerEnter2D(Collider2D other) {
        var player = other.GetComponent<Player>();
        if (player != null){
            inventory.AddItem(itemData);
            Destroy(gameObject);
        }
    }
}