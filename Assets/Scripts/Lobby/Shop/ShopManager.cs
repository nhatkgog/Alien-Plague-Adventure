using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ShopManager : MonoBehaviour
{
    public List<ShopItemData> shopItems;
    public GameObject shopSlotPrefab;
    public Transform shopPanel;

    [SerializeField] private float playerGold;

    private void Start()
    {
        playerGold = PlayerSelector.Instance.GetSelectedPlayer().money;
        GenerateShop();
    }

    void GenerateShop()
    {
        foreach (ShopItemData item in shopItems)
        {
            GameObject slot = Instantiate(shopSlotPrefab, shopPanel);
            ShopItemUI shopSlot = slot.GetComponent<ShopItemUI>();
            shopSlot.SetUp(item, () => TryBuyItem(item));
        }
    }

    void TryBuyItem(ShopItemData item)
    {
        if (playerGold >= item.price)
        {
            playerGold -= item.price;
            PlayerSelector.Instance.SetMoney(playerGold);
            Inventory.instance.AddItem(item.itemData);
            Debug.Log("Đã mua: " + item.itemData.itemName);
        }
        else
        {
            Debug.Log("Không đủ tiền!");
        }
    }

    public float GetGold() => playerGold;

}
