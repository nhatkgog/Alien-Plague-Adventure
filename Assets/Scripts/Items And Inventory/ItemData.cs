using UnityEngine;
public enum ItemType
{
    Material,
    Equipment
}

[CreateAssetMenu(fileName = "NewItem", menuName = "Data/ItemData")]
public class ItemData : ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    public Sprite itemIcon;
}
