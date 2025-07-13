using UnityEngine;
using TMPro;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private ShopItemData[] shopItems;
    [SerializeField] private Transform contentParent;
    [SerializeField] private GameObject shopItemPrefab;
    [SerializeField] private TMP_Text goldText;

    private float playerGold;

    private void Start()
    {
        playerGold = PlayerSelector.Instance.GetSelectedPlayer().money;
        UpdateGoldUI();

        foreach (var item in shopItems)
        {
            GameObject go = Instantiate(shopItemPrefab, contentParent);
            go.GetComponent<ShopItemUI>().Setup(item, this);
        }
    }

    public void TryBuyItem(ShopItemData itemData)
    {
        if (playerGold >= itemData.price)
        {
            playerGold -= itemData.price;
            Inventory.instance.AddItem(itemData.itemToSell);
            UpdateGoldUI();
            Debug.Log($"Đã mua {itemData.itemToSell.itemName}");
        }
        else
        {
            Debug.Log("Không đủ tiền!");
        }
    }

    private void UpdateGoldUI()
    {
        goldText.text = $"Gold: {playerGold}";
    }
}
