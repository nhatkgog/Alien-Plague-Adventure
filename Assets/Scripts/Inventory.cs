using UnityEngine;
using System.Collections.Generic;


public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public List<InventoryItem> inventoryItems;
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        inventoryItems = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();
    }
    public void AddItem(ItemData item)
    {
        if (inventoryDictionary.TryGetValue(item, out InventoryItem existingItem))
        {
            existingItem.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(item);
            inventoryItems.Add(newItem);
            inventoryDictionary.Add(item, newItem);
        }
    }
    public void RemoveItem(ItemData item)
    {
        if (inventoryDictionary.TryGetValue(item, out InventoryItem existingItem))
        {
            if (existingItem.stackSize <= 1)
            {
                inventoryItems.Remove(existingItem);
                inventoryDictionary.Remove(item);
            }
            else
            {
                existingItem.RemoveStack();
            }
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ItemData item = inventoryItems[inventoryItems.Count - 1].itemData;
            RemoveItem(item);
        }
    }
}
