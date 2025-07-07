using Assets.Scripts.Save_and_Load;
using System.Collections.Generic;
using UnityEngine;


public class Inventory : MonoBehaviour, ISaveManager
{
    public static Inventory instance;

    public List<ItemData> startingEquipment;

    public List<InventoryItem> equiqment;
    public Dictionary<ItemData_Equiment, InventoryItem> equiqmentDictionary;

    public List<InventoryItem> inventoryItems;
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;

    public List<InventoryItem> stashItems;
    public Dictionary<ItemData, InventoryItem> stashDictionary;

    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;
    [SerializeField] private Transform equiqmentSlotParent;

    [Header("Items cooldown")]
    private float itemCooldown;
    private float lastTimeUsedItem;

    private UI_ItemSlot[] itemSlots;
    private UI_ItemSlot[] stashSlots;
    private UI_EquipmentSlot[] equiqmentSlots;

    [Header("Database")]
    public List<InventoryItem> loadedItems;


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
        equiqmentSlots = equiqmentSlotParent.GetComponentsInChildren<UI_EquipmentSlot>();

        AddStartingItems();
    }

    private void AddStartingItems()
    {
        //if (loadedItems.Count > 0)
        //{
        //    foreach (InventoryItem item in loadedItems)
        //    {
        //        for (int i = 0; i < item.stackSize; i++)
        //        {
        //            AddItem(item.itemData);
        //        }
        //    }
        //    return;
        //}
        for (int i = 0; i < startingEquipment.Count; i++)
        {


            AddItem(startingEquipment[i]);

        }
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
            AddItem(oldEquipment);
        }
        equiqment.Add(newItem);
        equiqmentDictionary.Add(newEquipment, newItem);
        RemoveItem(_item);
    }

    public void UnEquipItem(ItemData_Equiment itemDelete)
    {
        if (equiqmentDictionary.TryGetValue(itemDelete, out InventoryItem existingItem))
        {
            equiqment.Remove(existingItem);
            equiqmentDictionary.Remove(itemDelete);
        }
    }
    public void UseEquippedItem(ItemData_Equiment equipment)
    {
        // Remove from equipment slot
        if (equiqmentDictionary.TryGetValue(equipment, out InventoryItem existingItem))
        {
            equiqment.Remove(existingItem);
            equiqmentDictionary.Remove(equipment);
        }

        // Update UI to reflect removal
        UpdateSlotUI();
    }

    public void UpdateSlotUI()
    {
        for (int i = 0; i < equiqmentSlots.Length; i++)
        {
            equiqmentSlots[i].CleanUpSlot();
        }

        // Track which items have already been assigned
        HashSet<ItemData_Equiment> assignedItems = new HashSet<ItemData_Equiment>();

        // Assign each equipped item to only one matching slot
        for (int i = 0; i < equiqmentSlots.Length; i++)
        {
            foreach (KeyValuePair<ItemData_Equiment, InventoryItem> item in equiqmentDictionary)
            {
                if (assignedItems.Contains(item.Key)) continue; // Already used this item

                Debug.Log($"Checking slot: {equiqmentSlots[i].equimentType}, Item: {item.Key.equimentType}");

                if (item.Key.equimentType == equiqmentSlots[i].equimentType &&
                  equiqmentSlots[i].item == null)
                {
                    equiqmentSlots[i].UpdateSlot(item.Value);
                    assignedItems.Add(item.Key);
                    break;
                }
            }
        }

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
        if (_item.itemType == ItemType.Equipment && CanAddItem())
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
    public bool CanCraft(ItemData_Equiment itemToCraft, List<InventoryItem> requiredMaterials)
    {
        List<InventoryItem> materials = new List<InventoryItem>();
        for (int i = 0; i < requiredMaterials.Count; i++)
        {
            if (stashDictionary.TryGetValue(requiredMaterials[i].itemData, out InventoryItem existingItem))
            {
                if (existingItem.stackSize < requiredMaterials[i].stackSize)
                {
                    return false; // Not enough materials
                }
                else
                {
                    materials.Add(existingItem);

                }
            }
            else
            {
                return false; // Material not found in inventory
            }
        }
        for (int i = 0; i < materials.Count; i++)
        {
            RemoveItem(materials[i].itemData);
        }
        AddItem(itemToCraft);
        Debug.Log($"Crafted {itemToCraft.itemName}!");
        return true;
    }

    public List<InventoryItem> GetEquiqmentList() => equiqment;
    public List<InventoryItem> GetStashList() => stashItems;
    public ItemData_Equiment GetEquippedItem(EquimentType _type)
    {
        ItemData_Equiment equipmentItem = null;
        foreach (KeyValuePair<ItemData_Equiment, InventoryItem> item in equiqmentDictionary)
        {
            if (item.Key.equimentType == _type)
            {
                equipmentItem = item.Key;
            }
        }
        return equipmentItem;
    }
    public void UseHeal()
    {
        ItemData_Equiment currentHeal = GetEquippedItem(EquimentType.Heal);
        if (currentHeal == null)
        {
            Debug.LogWarning(" No healing item equipped!");
            return;
        }

        bool canUse = Time.time >= lastTimeUsedItem + currentHeal.itemCooldown;
        if (!canUse)
        {
            Debug.Log(" Healing item is on cooldown.");
            return;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError(" No GameObject with tag 'Player' found!");
            return;
        }

        Debug.Log(" Using healing item...");
        currentHeal.ItemEffect(player.transform);
        lastTimeUsedItem = Time.time;


        UseEquippedItem(currentHeal);
    }
    public void UseExpItem()
    {
        ItemData_Equiment currentExpItem = GetEquippedItem(EquimentType.Exp);
        if (currentExpItem == null)
        {
            Debug.LogWarning("No EXP item equipped!");
            return;
        }

        bool canUse = Time.time >= lastTimeUsedItem + currentExpItem.itemCooldown;
        if (!canUse)
        {
            Debug.Log("EXP item is on cooldown.");
            return;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("No GameObject with tag 'Player' found!");
            return;
        }

        Debug.Log("Using EXP item...");
        currentExpItem.ItemEffect(player.transform);
        lastTimeUsedItem = Time.time;

        UseEquippedItem(currentExpItem); // Remove after use
    }
    public bool CanAddItem()
    {
        if (inventoryItems.Count >= itemSlots.Length)
        {
            Debug.Log("Inventory is full!");
            return false;
        }
        return true;
    }

    public void LoadData(GameData _data)
    {
        loadedItems.Clear();
        inventoryItems.Clear();
        inventoryDictionary.Clear();
        stashItems.Clear();
        stashDictionary.Clear();

        foreach (KeyValuePair<string, int> pair in _data.inventory)
        {
            foreach (var item in GetitemDataBase())
            {
                if (item != null && item.itemId == pair.Key)
                {
                    InventoryItem itemToLoad = new InventoryItem(item);
                    itemToLoad.stackSize = pair.Value;

                    loadedItems.Add(itemToLoad);

                    if (item.itemType == ItemType.Material)
                    {
                        stashItems.Add(itemToLoad);
                        stashDictionary.Add(item, itemToLoad);
                    }
                    else if (item.itemType == ItemType.Equipment)
                    {
                        inventoryItems.Add(itemToLoad);
                        inventoryDictionary.Add(item, itemToLoad);
                    }
                }
            }
        }

        UpdateSlotUI();
    }


    public void SaveData(GameData _data)
    {
        _data.inventory.Clear();
        foreach (KeyValuePair<ItemData, InventoryItem> pair in inventoryDictionary)
        {
            _data.inventory.Add(pair.Key.itemId, pair.Value.stackSize);
        }
        foreach (KeyValuePair<ItemData, InventoryItem> pair in stashDictionary)
        {
            _data.inventory.Add(pair.Key.itemId, pair.Value.stackSize);
        }
    }
    private List<ItemData> GetitemDataBase()
    {
        var equipmentItems = Resources.LoadAll<ItemData>("ItemEquipment");
        var baseItems = Resources.LoadAll<ItemData>("Materials");

        List<ItemData> allItems = new List<ItemData>();
        allItems.AddRange(equipmentItems);
        allItems.AddRange(baseItems);

        return allItems;
    }


}