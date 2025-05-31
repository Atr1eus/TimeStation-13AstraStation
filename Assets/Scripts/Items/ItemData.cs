using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ItemType
{
    Normal,
    Story
}
/// <summary>
/// 物品数据类
/// </summary>
[CreateAssetMenu(fileName = "New Item", menuName = "Items/Item Data")]
public class ItemData : ScriptableObject
{
    public string itemName; 
    public string itemID; //物品唯一标识
    public int itemPrice;
    public GameObject prefab;
    [TextArea]public string itemDescription; //物品描述

    [Header("索要/交易配置")]
    public int minRequestAmount = 1;
    public int maxRequestAmount = 5;
    public int minOfferAmount = 0;
    public int maxOfferAmount = 5;

    //todo：根据需求增加item属性，如item种类或item动效
}