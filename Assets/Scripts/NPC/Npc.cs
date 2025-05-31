using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// npc数据配置类 只关心npc是什么
/// </summary>
public enum NpcType
{
    Normal,
    Story
}
public enum NpcSex
{
    Man,
    Woman
}
[CreateAssetMenu(fileName = "New NPC", menuName = "Game/NPC")]
public class Npc : ScriptableObject
{
    public int age;
    public string npcName;
    public NpcSex sex;
    public GameObject hand;
    public NpcType type;
    [TextArea] public string npcDescription;

    [Header("对话配置")]
    public List<string> greetingDialogues; //初始对话列表
    public List<string> successDialogues; //同意给予对话列表
    public List<string> refuseDialogues; //拒绝给予对话列表


    [Header("交易配置")]
    public List<ItemData> requestItems; //索取物品列表
    public List<ItemData> offerItems; //带来物品列表

    [Header("出现时间配置")]
    public int minAppearRound;
    public int maxAppearRound;

    [Header("NPC出现条件")]
    public bool isMoneyMore10K;
    public bool isRoundMore5;

    public string GetRandomGreeting() => greetingDialogues[Random.Range(0, greetingDialogues.Count)]; //获取随机招呼语

    public ItemData GetRandomRequestItem() => requestItems[Random.Range(0, requestItems.Count)]; //获取随机物品要求
    public ItemData GetRandomOfferItem() => offerItems[Random.Range(0, offerItems.Count)];
    public string GetDescription() => npcDescription;




}