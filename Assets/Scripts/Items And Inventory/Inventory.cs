using System.Collections.Generic;
using UnityEngine;


public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public List<InventoryItem> equiqment;
    public Dictionary<ItemData_Equiment, InventoryItem> equiqmentDictionary;

    public List<InventoryItem> inventoryItems;
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;

    public List<InventoryItem> stashItems;
    public Dictionary<ItemData, InventoryItem> stashDictionary;

    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;
    private UI_ItemSlot[] itemSlots;
    private UI_ItemSlot[] stashSlots;
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
        itemSlots = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();

        stashItems = new List<InventoryItem>();
        stashDictionary = new Dictionary<ItemData, InventoryItem>();
        stashSlots = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>();

        equiqment = new List<InventoryItem>();
        equiqmentDictionary = new Dictionary<ItemData_Equiment, InventoryItem>();
    }
    public void EquipItem(ItemData _item)
    {
        ItemData_Equiment newEquipment = _item as ItemData_Equiment;
        InventoryItem newItem = new InventoryItem(_item);

        ItemData_Equiment oldEquipment = null;
        foreach (KeyValuePair<ItemData_Equiment, InventoryItem> item in equiqmentDictionary)
        {
            if (item.Key.itemType == newEquipment.itemType)
            {
                oldEquipment = item.Key;
            }
        }
        if (oldEquipment != null)
        {
            UnEquipItem(oldEquipment);
        }
        equiqment.Add(newItem);
        equiqmentDictionary.Add(newEquipment, newItem);
        RemoveItem(_item);
    }

    private void UnEquipItem(ItemData_Equiment itemDelete)
    {
        if (equiqmentDictionary.TryGetValue(itemDelete, out InventoryItem existingItem))
        {
            equiqment.Remove(existingItem);
            equiqmentDictionary.Remove(itemDelete);
        }
    }

    public void UpdateSlotUI()
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            itemSlots[i].CleanUpSlot();
        }

        for (int i = 0; i < stashSlots.Length; i++)
        {
            stashSlots[i].CleanUpSlot();
        }

        // Safely fill inventory slots (only up to the available slots)
        int inventoryCount = Mathf.Min(inventoryItems.Count, itemSlots.Length);
        for (int i = 0; i < inventoryCount; i++)
        {
            itemSlots[i].UpdateSlot(inventoryItems[i]);
        }

        // Safely fill stash slots (only up to the available slots)
        int stashCount = Mathf.Min(stashItems.Count, stashSlots.Length);
        for (int i = 0; i < stashCount; i++)
        {
            stashSlots[i].UpdateSlot(stashItems[i]);
        }
    }

    public void AddItem(ItemData _item)
    {
        if (_item.itemType == ItemType.Equipment)
        {
            Debug.Log($"Adding item: {_item.name}, Type: {_item.itemType}");

            AddToInventory(_item);

        }
        else if (_item.itemType == ItemType.Material)
        {
            Debug.Log($"Adding item: {_item.name}, Type: {_item.itemType}");

            AddToStash(_item);
        }
        UpdateSlotUI();
    }

    private void AddToStash(ItemData _item)
    {
        if (stashDictionary.TryGetValue(_item, out InventoryItem existingItem))
        {
            existingItem.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            stashItems.Add(newItem);
            stashDictionary.Add(_item, newItem);
        }
    }

    private void AddToInventory(ItemData item)
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
        if (stashDictionary.TryGetValue(item, out InventoryItem stashItem))
        {
            if (stashItem.stackSize <= 1)
            {
                stashItems.Remove(stashItem);
                stashDictionary.Remove(item);
            }
            else
            {
                stashItem.RemoveStack();
            }
        }
        UpdateSlotUI();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ItemData item = inventoryItems[inventoryItems.Count - 1].itemData;
            RemoveItem(item);
        }
    }
    public bool CanCraft(ItemData_Equiment itemToCraft, List<CraftMaterial> requiredMaterials)
    {
        foreach (CraftMaterial mat in requiredMaterials)
        {
            if (!stashDictionary.TryGetValue(mat.material, out InventoryItem stashItem))
            {
                Debug.LogWarning($"Missing: {mat.material.itemName}");
                return false;
            }

            if (stashItem.stackSize < mat.requiredAmount)
            {
                Debug.LogWarning($"Not enough {mat.material.itemName}. Need {mat.requiredAmount}, have {stashItem.stackSize}");
                return false;
            }
        }

        // If all materials available, remove them
        foreach (CraftMaterial mat in requiredMaterials)
        {
            for (int i = 0; i < mat.requiredAmount; i++)
            {
                RemoveItem(mat.material);
            }
        }

        // Add the crafted equipment
        AddItem(itemToCraft);
        Debug.Log($"Crafted {itemToCraft.itemName}!");
        return true;
    }




}
