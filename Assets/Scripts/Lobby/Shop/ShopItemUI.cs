using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemUI : MonoBehaviour
{
    public Image icon;
    public TMP_Text nameText;
    public TMP_Text priceText;
    public Button buyButton;

    private ShopItemData itemData;
    private ShopManager shopManager;

    public void Setup(ShopItemData data, ShopManager manager)
    {
        itemData = data;
        shopManager = manager;

        icon.sprite = data.itemToSell.itemIcon;
        nameText.text = data.itemToSell.itemName;
        priceText.text = $"{data.price} Gold";

        buyButton.onClick.AddListener(BuyItem);
    }

    private void BuyItem()
    {
        shopManager.TryBuyItem(itemData);
    }
}
