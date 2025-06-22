using UnityEngine.EventSystems;

namespace Assets.Scripts.UI
{
    public class Ui_CraftSlot : UI_ItemSlot
    {
        private void OnEnable()
        {
            UpdateSlot(item);
        }
        public override void OnPointerDown(PointerEventData eventData)
        {
            if (item?.itemData is ItemData_Equiment craft_data && craft_data.crafting != null)
            {
                Inventory.instance.CanCraft(craft_data, craft_data.crafting);
            }
        }





    }
}
