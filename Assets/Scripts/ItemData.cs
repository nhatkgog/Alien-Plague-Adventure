using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Data/ItemData")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
}
