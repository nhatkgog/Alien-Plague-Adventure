using UnityEngine;

public class ItemObject : MonoBehaviour
{

    [SerializeField] private ItemData itemData;

    [Header("SFX")]
    [SerializeField] private AudioClip collectClip;

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
            SFXManager.Instance.PlayOneShot(collectClip);
            Destroy(gameObject);
        }

    }
}
