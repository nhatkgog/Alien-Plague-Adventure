using UnityEngine;

public class ItemObject : MonoBehaviour
{

    [SerializeField] private ItemData itemData;

    private void OnValidate()
    {
        GetComponent<SpriteRenderer>().sprite = itemData.itemIcon;
        gameObject.name = "Item object -" + itemData.itemName;
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && collider.GetComponent<InputSystemMovement>() != null != null)
        {
            Inventory.instance.AddItem(itemData);
            Destroy(gameObject);
        }

    }
}
