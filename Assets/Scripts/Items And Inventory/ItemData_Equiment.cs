using Assets.Scripts.Efect;
using System.Collections.Generic;
using UnityEngine;

public enum EquimentType
{

    Heal,
    Exp,
    Boom
}


[CreateAssetMenu(fileName = "NewItem", menuName = "Data/Equipment")]
public class ItemData_Equiment : ItemData
{
    public EquimentType equimentType;
    public ItemEffect[] itemEffect;
    public float itemCooldown;

    private int minDescriptionLength;
    [Header("Heal Description")]
    public string heal;



    [Header("Exp Description")]
    public string exp;

    [Header("Crafting Requirements")]
    public List<InventoryItem> crafting;

    public void ItemEffect(Transform _enemyPosition)
    {
        foreach (var item in itemEffect)
        {
            item.ExcecuteEffect(_enemyPosition);
        }
    }
    public virtual string GetDescription()
    {
        sb.Length = 0;

        AddItemDescription(heal, "Heal");
        AddItemDescription(exp, "Exp");
        if (minDescriptionLength < 5)
        {
            for (int i = 0; i < 5 - minDescriptionLength; i++)
            {
                sb.AppendLine();
                sb.Append(" ");
            }
        }

        return sb.ToString();
    }
    private void AddItemDescription(string _value, string _name)
    {
        if (_value != null)
        {
            if (sb.Length > 0)
                sb.AppendLine();
            if (_value.Length > 0)
                sb.Append(_name + _value);
            minDescriptionLength++;

        }
    }
}
