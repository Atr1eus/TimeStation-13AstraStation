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
    public void MergeWith(Item other) //堆叠操作
    {
        if(CanMergeWith(other))
        {
            int total = amount + other.amount;
            if(total <= data.maxStack)
            {
                amount = total;
                other.amount = 0;
            }
            else 
            {
                amount = data.maxStack;
                other.amount = total - data.maxStack;
            }
        }
    }
    public bool CanMergeWith(Item other)
    {
        return data == other.data && data.isStackable && amount < data.maxStack;
    }

}
