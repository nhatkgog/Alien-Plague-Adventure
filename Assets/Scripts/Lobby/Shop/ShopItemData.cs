using UnityEngine;

[CreateAssetMenu(fileName = "ShopItem", menuName = "Data/ShopItem")]
public class ShopItemData : ScriptableObject
{
    public ItemData itemToSell;
    public int price;
}
