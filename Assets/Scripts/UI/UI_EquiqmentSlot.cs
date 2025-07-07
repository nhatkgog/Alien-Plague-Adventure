using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EquipmentSlot : UI_ItemSlot
{
    public EquimentType equimentType;
    private void OnValidate()
    {
        gameObject.name = "Equeipment Slot - " + equimentType.ToString();
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (item == null || item.itemData == null)
        {
            Debug.LogWarning("Equipment slot clicked, but item is null.");
            return;
        }
        Inventory.instance.UnEquipItem(item.itemData as ItemData_Equiment);
        Inventory.instance.EquipItem(item.itemData as ItemData_Equiment);
        CleanUpSlot();
    }
}