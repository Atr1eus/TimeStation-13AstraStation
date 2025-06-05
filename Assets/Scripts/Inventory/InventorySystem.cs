using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// 仓库系统
/// </summary>
[System.Serializable]
public class InventorySystem : SingletonMonoBehaviour<InventorySystem>
{
    public List<Item> items = new List<Item>();
    public List<ItemData> datas = new List<ItemData>();
    public List<Item> lastRoundItems = new List<Item>();
    public List<ItemData> lastRoundDatas = new List<ItemData>();
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
    public void AddItem(ItemData itemdata, int amount) //增加物品操作
    {
        foreach (var it in items)
        {
            if (it.data == itemdata)
            {
                it.amount += amount;
                return;
            }
        }
        Item item = new Item(itemdata, amount);
        items.Add(item);
        datas.Add(itemdata);
    }
    public void AddItemToLast(ItemData itemdata, int amount) //增加物品操作
    {
        foreach (var it in items)
        {
            if (it.data == itemdata)
            {
                it.amount += amount;
                return;
            }
        }
        Item item = new Item(itemdata, amount);
        lastRoundItems.Add(item);
        lastRoundDatas.Add(itemdata);
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
                return items[i].amount >= needAmount;
            }
        }
        return false;
    }
    // public bool GetNpcOfferItems(NpcController npc)
    // {
    //     return GetNpcOfferItems(npc.currentOfferItem, npc.currentOfferAmount);
    // }
    // public bool GetNpcOfferItems(ItemData offerItem, int offerAmount)
    // {
    //     AddItem(offerItem, offerAmount);
    //     return true;
    // }
    public bool GiveNpcRequestItems(ItemData requestItem, int requestAmount)
    {
        if (!HasEnoughItem(requestItem, requestAmount)) return false;
        RemoveItem(requestItem, requestAmount);
        return true;
    }
    protected override void Awake()
    {
        base.Awake();
        items = new List<Item>();
        datas = new List<ItemData>();
    }
    public void CurrentInventoryDataToLastRound()
    {
        lastRoundItems = items;
        lastRoundDatas = datas;
    }
    public void ClearBeforeLoading()
    {
        items.Clear();
        datas.Clear();
        lastRoundDatas.Clear();
        lastRoundItems.Clear();
    }
}