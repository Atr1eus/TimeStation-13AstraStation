using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 物品实例类
/// </summary>
public class Item : MonoBehaviour 
{
    public ItemData data;
    public int amount = 1; //该物品数量

    public Item(ItemData itemData,int quantity = 1)
    {
        this.data = itemData;
        this.amount = quantity;
    }

}
