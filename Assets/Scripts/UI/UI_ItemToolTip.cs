using TMPro;
using UnityEngine;

public class UI_ItemToolTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;
    [SerializeField] private TextMeshProUGUI itemTypeText;

    public void ShowToolTip(ItemData_Equiment item)
    {
        string desc = item.GetDescription();
        Debug.Log($"Item: {item.itemName}, Type: {item.equimentType}, Desc: {desc}");

        itemNameText.text = item.itemName;
        itemTypeText.text = item.equimentType.ToString();
        itemDescriptionText.text = desc;
        gameObject.SetActive(true);
    }

    public void HideToolTip() => gameObject.SetActive(false);
}
