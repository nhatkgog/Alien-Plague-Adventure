using UnityEngine;

[CreateAssetMenu(fileName = "ShopItem", menuName = "Data/ShopItem")]
public class ShopItemData : ScriptableObject
{
    public ItemData itemData;
    public float price;
    public Sprite icon => itemData.itemIcon;
    public string itemName => itemData.itemName;

}
