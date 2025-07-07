using System.Collections.Generic;
using UnityEngine;

public class PlayerItemDrop : ItemDrop
{
    [Header("Player's drop")]
    [SerializeField] private float chanceToLooseItems;
    [SerializeField] private float chanceToLooseMaterials;


    public override void GenerateDrop()
    {
        Inventory inventory = Inventory.instance;

        List<InventoryItem> itemsToUnequip = new List<InventoryItem>();
        List<InventoryItem> materialsToUnequip = new List<InventoryItem>();


        foreach (InventoryItem item in inventory.GetEquiqmentList())
        {
            if (Random.Range(0, 100) < chanceToLooseItems)
            {
                DropItem(item.itemData);
                itemsToUnequip.Add(item);
            }
        }
        for (int i = 0; i < itemsToUnequip.Count; i++)
        {
            inventory.UnEquipItem(itemsToUnequip[i].itemData as ItemData_Equiment);
        }


        foreach (InventoryItem item in inventory.GetStashList())
        {
            if (Random.Range(0, 100) < chanceToLooseMaterials)
            {
                DropItem(item.itemData);
                materialsToUnequip.Add(item);
            }
        }
        for (int i = 0; i < materialsToUnequip.Count; i++)
        {
            inventory.UnEquipItem(materialsToUnequip[i].itemData as ItemData_Equiment);
        }

    }
}
