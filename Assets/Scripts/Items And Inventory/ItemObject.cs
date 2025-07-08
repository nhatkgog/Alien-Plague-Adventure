using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ItemData itemData;

    [Header("SFX")]
    [SerializeField] private AudioClip collectClip;
    
    private void SetUpVisual()
    {
        if (itemData == null)
            return;

        GetComponent<SpriteRenderer>().sprite = itemData.itemIcon;
        gameObject.name = "Item object -" + itemData.itemName;
    }

    public void SetUpItem(ItemData _itemData, Vector2 _velocity)
    {
        itemData = _itemData;
        rb.linearVelocity = _velocity;
        SetUpVisual();
    }

    public void PickupItem()
    {
        if (!Inventory.instance.CanAddItem() && itemData.itemType == ItemType.Equipment)
            return;
        Inventory.instance.AddItem(itemData);
        SFXManager.Instance.PlayOneShot(collectClip);
        Destroy(gameObject);
    }
}
