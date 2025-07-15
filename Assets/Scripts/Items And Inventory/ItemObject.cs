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
        if (itemData is ItemData_Equiment equipItem && equipItem.equimentType == EquimentType.Boom)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                InputSystemMovement player = playerObj.GetComponent<InputSystemMovement>();
                if (player != null)
                {
                    player.AddBoom();
                    Debug.Log($"🔥 Đã nhặt Boom! ");
                }
            }
            SFXManager.Instance.PlayOneShot(collectClip);
            Destroy(gameObject);
            return;
        }

        if (!Inventory.instance.CanAddItem() && itemData.itemType == ItemType.Equipment)
            return;
        Inventory.instance.AddItem(itemData);
        SFXManager.Instance.PlayOneShot(collectClip);
        Destroy(gameObject);
    }
}