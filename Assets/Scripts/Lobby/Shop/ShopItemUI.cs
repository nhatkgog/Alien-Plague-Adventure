using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemUI : MonoBehaviour
{
    public Image icon;
    public Button buyButton;
    public float price;
    private ItemData currentItem;

    public void SetUp(ShopItemData item, System.Action onBuy)
    {
        currentItem = item.itemData;
        icon.sprite = item.icon;
        price = item.price;
        buyButton.onClick.AddListener(() => onBuy?.Invoke());
    }
}
