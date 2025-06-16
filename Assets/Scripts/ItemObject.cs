using UnityEngine;

public class ItemObject : MonoBehaviour
{
    private SpriteRenderer sr;
    [SerializeField] private ItemData itemData;
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = itemData.itemIcon;
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") != null)
        {
            Inventory.instance.AddItem(itemData);
            Destroy(gameObject);
        }

    }
}
