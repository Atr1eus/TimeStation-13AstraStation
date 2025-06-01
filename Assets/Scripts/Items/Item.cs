using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// [System.Serializable]
public class Item
{
    public ItemData data;
    public int amount = 1; //该物品数量

    public Item(ItemData itemData, int quantity = 1)
    {
        this.data = itemData;
        this.amount = quantity;
    }

}
