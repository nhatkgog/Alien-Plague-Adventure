using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemText;

    public InventoryItem item;

    public void UpdateSlot(InventoryItem _newItem)
    {
        item = _newItem;
        itemImage.color = Color.white;
        if (item != null)
        {
            itemImage.sprite = item.itemData.itemIcon;
            if (item.stackSize > 1)
            {
                itemText.text = item.stackSize.ToString();
            }
            else
            {
                itemText.text = "";
            }
        }
    }

    public void CleanUpSlot()
    {
        item = null;
        itemImage.sprite = null;
        itemImage.color = Color.clear;
        itemText.text = "";
    }
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (item == null || item.itemData.itemType != ItemType.Equipment) return;

        var equipment = item.itemData as ItemData_Equiment;
        if (equipment == null) return;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        var playerScript = player.GetComponent<InputSystemMovement>();
        if (playerScript == null) return;

        switch (equipment.equimentType)
        {
            case EquimentType.Weapon:
                InputSystemMovement.damage += equipment.bonusDamage;
                break;

            case EquimentType.Armor:
                playerScript.IncreaseMaxHealth(equipment.bonusHP);
                break;

        }

        // Equip once — this will internally remove it from inventory
        Inventory.instance.EquipItem(equipment);

        Inventory.instance.UpdateSlotUI();
    }

    public void TriggerClick()
    {
        OnPointerDown(null);
    }







}
