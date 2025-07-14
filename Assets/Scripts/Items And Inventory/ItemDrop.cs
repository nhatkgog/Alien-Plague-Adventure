using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private int possibleItemDrop;
    [SerializeField] private ItemData[] possibleDrops;
    private List<ItemData> dropList = new List<ItemData>();

    [SerializeField] private GameObject dropPrefab;

    public virtual void GenerateDrop()
    {
        for (int i = 0; i < possibleDrops.Length; i++)
        {
            if (Random.Range(0, 100) < possibleDrops[i].dropChance)
            {
                dropList.Add(possibleDrops[i]);
            }
        }
        for (int i = 0; i < possibleItemDrop; i++)
        {
            if (dropList.Count == 0)
                break; 

            int randomIndex = Random.Range(0, dropList.Count);
            ItemData randomItem = dropList[randomIndex];

            dropList.RemoveAt(randomIndex);
            DropItem(randomItem);
        }

    }
    protected void DropItem(ItemData _itemData)
    {
        GameObject newDrop = Instantiate(dropPrefab, transform.position, Quaternion.identity);
        Vector2 randomVelocity = new Vector2(Random.Range(-5, 5), Random.Range(15, 20));
        newDrop.GetComponent<ItemObject>().SetUpItem(_itemData, randomVelocity);
    }
}
