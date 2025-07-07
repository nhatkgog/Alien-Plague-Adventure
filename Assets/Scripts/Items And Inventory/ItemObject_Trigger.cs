using UnityEngine;

public class ItemObject_Trigger : MonoBehaviour
{
    private ItemObject myItemObject => GetComponentInParent<ItemObject>();
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            var movement = collider.GetComponent<InputSystemMovement>();
            if (movement != null && !movement.isDead)
            {
                myItemObject.PickupItem();
            }
        }
    }

}
