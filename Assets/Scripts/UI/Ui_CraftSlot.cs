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
            ItemData_Equiment craftData = item.itemData as ItemData_Equiment;
            Inventory.instance.CanCraft(craftData, craftData.crafting);
        }





    }
}
