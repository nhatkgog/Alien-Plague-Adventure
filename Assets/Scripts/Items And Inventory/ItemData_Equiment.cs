using System.Collections.Generic;
using UnityEngine;

public enum EquimentType
{
    Weapon,
    Armor,
}

[System.Serializable]
public class CraftMaterial
{
    public ItemData material;
    public int requiredAmount;
}

[CreateAssetMenu(fileName = "NewItem", menuName = "Data/Equipment")]
public class ItemData_Equiment : ItemData
{
    public EquimentType equimentType;
    public float bonusDamage = 1000;
    public float bonusHP = 30;

    [Header("Crafting Requirements")]
    public List<CraftMaterial> crafting;
}
