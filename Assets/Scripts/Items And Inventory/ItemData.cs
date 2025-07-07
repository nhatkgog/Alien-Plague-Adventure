using System.Text;
using UnityEditor;
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
    public string itemId;
    [Range(0, 100)]
    public int dropChance;

    protected StringBuilder sb = new StringBuilder();
    private void OnValidate()
    {
#if UNITY_EDITOR
        string path = AssetDatabase.GetAssetPath(this);
        itemId = AssetDatabase.AssetPathToGUID(path);
#endif
    }
    public virtual string GetDescription()
    {

        return "";
    }
}
