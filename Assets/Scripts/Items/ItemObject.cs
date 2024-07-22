using UnityEngine;

public class ItemObject : MonoBehaviour {
    [SerializeField] private ItemData itemData;
    private SpriteRenderer sr;

    void Awake() {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = itemData.Icon;
    }

    void OnTriggerEnter2D(Collider2D other) {
        var player = other.GetComponent<Player>();
        if (player != null){
            Debug.Log(player.name + " picked up the item" + itemData.name);
            Destroy(gameObject);
        }
        
    }
}