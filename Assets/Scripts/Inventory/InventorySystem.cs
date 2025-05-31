using System.Collections.Generic;

/// <summary>
/// 仓库系统
/// </summary>
public class InventorySystem : SingletonMonoBehaviour<InventorySystem>
{
    public List<Item> items;
    public bool AddItem(Item item) //增加物品操作
    {
        //堆叠成功返回true 反之增加item
        if (item.data.isStackable)
        {
            foreach (var it in items)
            {
                it.MergeWith(item);
                if (item.amount <= 0) return true;
            }
        }
        items.Add(item);
        return false;
    }
    public bool RemoveItem(Item item, int removeAmount = 1)//移除一定数量的物品
    {
        for (int i = items.Count - 1; i >= 0; i--)
        {
            if (items[i].data == item.data)
            {
                if (items[i].amount > removeAmount)
                {
                    items[i].amount -= removeAmount;
                    return true;
                }
                else
                {
                    removeAmount -= items[i].amount;
                    items.RemoveAt(i);
                    if (removeAmount <= 0) return true;
                }
            }
        }
        return false;
    }
    public bool HasEnoughItem(Item item, int needAmount = 1) //判断是否有足量的物品/是否存在物品
    {
        int total = 0;
        foreach (var it in items)
        {
            if (item.data == it.data)
            {
                total += it.amount;
            }
        }
        return total >= needAmount;
    }

    protected override void Awake()
    {
        base.Awake();
    }
}