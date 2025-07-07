using UnityEngine.EventSystems;

namespace Assets.Scripts.UI
{
    public class Ui_CraftSlot : UI_ItemSlot
    {
        protected override void Start()
        {
            base.Start();
        }
        public void SetUpCraftSlot(ItemData_Equiment _data)
        {
            if (_data == null) return;
            item.itemData = _data;
            itemImage.sprite = _data.itemIcon;
            itemText.text = _data.itemName;
        }
        private void OnEnable()
        {
            UpdateSlot(item);
        }
        public override void OnPointerDown(PointerEventData eventData)
        {
            ui.craftWindow.SetupCraftWindow(item.itemData as ItemData_Equiment);
        }





    }
}
