using DialogueSystem;
using System.Collections.Generic;
using UnityEngine;
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
public class NpcData : ScriptableObject
{


    [Header("npc通用配置")]
    public int age;
    public string npcName;
    public NpcSex sex;
    public NpcType type;
    public TravelTo travelTo;
    public DialogueGraph npcDialogue;
    public List<ItemData> requestItems; //索取物品列表
    public List<ItemData> offerItems; //带来物品列表
    public int minPerRequestAwardGolds; //每交易一个需求物品可能获得的最低金钱
    public int maxPerRequestAwardGolds; //每交易一个需求物品可能获得的最高金钱
    public int minPerNonrequestAwardGolds; //每交易一个非需求物品可能获得的最低金钱
    public int maxPerNonrequestAwardGolds; //每交易一个非需求物品可能获得的最高金钱
    public int trueChoiceAwardGolds;
    public int falseChoicePunishGolds;
    public int initialFavorability;

    [Header("仅剧情npc配置")]
    public GameObject hand;
    public List<TicketData> ticket;
    public List<string> storyNpcDescriptions;
    public List<int> storyNpcMinAppearRounds;
    public List<int> storyNpcMaxAppearRounds;
    public List<string> storyNpcReasons;
    public List<string> storyNpcBackPack;
    public List<string> storyNpcFollowUpPlot;
    public List<bool> shouldAgreeList;

    [Header("仅路边npc配置")]
    public string npcDescription;
    public int minAppearRound; //最早出现回合
    public int maxAppearRound; //最晚出现回合
    public string reason;
    public string backPack;

    [Header("该npc出现条件")]
    public bool isMoneyMore10K;
    public bool isRoundMore5;


    public ItemData GetRandomRequestItem() => requestItems[Random.Range(0, requestItems.Count)]; //获取随机物品要求
    public ItemData GetRandomOfferItem() => offerItems[Random.Range(0, offerItems.Count)];
    public string GetDescription() => npcDescription;
    public int GetRandomRequestAwardGolds() => Random.Range(minPerRequestAwardGolds, maxPerRequestAwardGolds + 1);
    public int GetRandomNonrequestAwardGolds() => Random.Range(minPerNonrequestAwardGolds, maxPerNonrequestAwardGolds + 1);




}