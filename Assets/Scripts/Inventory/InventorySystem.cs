using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 仓库系统
/// </summary>
public class InventorySystem : SingletonMonoBehaviour<InventorySystem>
{
    public List<Item> items;
    public List<ItemData> datas;
    public void AddItem(Item item) //增加物品操作
    {
        foreach (var it in items)
        {
            if (it.data == item.data)
            {
                it.amount += item.amount;
                return;
            }
        }
        items.Add(item);
        datas.Add(item.data);
    }
    public bool RemoveItem(ItemData item, int removeAmount = 1)//移除一定数量的物品
    {
        for (int i = datas.Count - 1; i >= 0; i--)
        {
            if (datas[i] == item)
            {
                if (items[i].amount > removeAmount)
                {
                    items[i].amount -= removeAmount;
                    return true;
                }
                else if (items[i].amount == removeAmount)
                {
                    items.RemoveAt(i);
                    datas.RemoveAt(i);
                    return true;
                }
                return false;
            }
        }
        return false;
    }
    public bool HasEnoughItem(ItemData item, int needAmount = 1) //判断是否有足量的物品/是否存在物品
    {
        for (int i = 0; i < datas.Count; i++)
        {
            if (item.Equals(datas[i]))
            {
                Debug.Log($"有{items[i].amount}个{item.name}");
                return items[i].amount > needAmount;
            }
        }
        return false;
    }

    protected override void Awake()
    {
        base.Awake();
        items = new List<Item>();
        datas = new List<ItemData>();
    }
}