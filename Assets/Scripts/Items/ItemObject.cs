using UnityEngine;

public class ItemObject : MonoBehaviour {
    [SerializeField] private SpriteRenderer rdr;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ItemData itemData;

    void Start() {
        rdr = GetComponent<SpriteRenderer>();
    }
    void OnValidate() {
        if (itemData == null) return;
        else 
        {
            rdr.sprite = itemData.Icon;
            gameObject.name = "Item - " + itemData.name;
        }
    }

    // void Update() {
    //     if (Input.GetKeyDown(KeyCode.M)) 
    //     {
    //         rb.velocity = velocity;
    //     }
    // }

    public void SetupItem(ItemData itemData, Vector2 velocity){
        this.itemData = itemData;
        UpdateSprite();
        rb.velocity = velocity;
    }

    public void PickupItem(PlayerInventory inventory)
    {
        inventory.AddItem(itemData);
        Destroy(gameObject);
    }

    private void UpdateSprite() {
        if (itemData != null) {
            rdr.sprite = itemData.Icon;
            gameObject.name = "Item - " + itemData.name;
        }
    }
}