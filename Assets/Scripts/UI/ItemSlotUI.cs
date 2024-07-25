using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class ItemSlotUI : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemText;

    public InventoryItem item;
    public void UpdateSlot(InventoryItem newItem)
    {
        this.item = newItem;

        if (item != null)
        {
            itemImage.sprite = item.data.Icon;

            if (item.stackSize > 1)
            {
                itemText.text = item.stackSize.ToString();
            }
            else
            {
                itemText.text = "";
            }
        }
    }

}
