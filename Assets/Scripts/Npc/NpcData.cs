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
public enum NpcCareer
{
    Teacher,
    Doctor,
    None
}
public enum NpcSpecies
{
    Human,
    Cat
}
[CreateAssetMenu(fileName = "New NPC", menuName = "Game/NPC")]
public class NpcData : ScriptableObject
{
    public int age;
    public string npcName;
    public NpcSex sex;
    public GameObject hand; //手(剧情npc绑定)
    public NpcType type; //普通/剧情
    public NpcCareer career; //职业
    public NpcSpecies species; //物种
    public TravelTo travelTo; //去过去还是未来
    [TextArea] public string npcDescription; //外貌等介绍


    [Header("对话配置")]
    public List<string> greetingDialogues; //初始对话列表
    public List<string> successDialogues; //放行后对话列表
    public List<string> refuseDialogues; //拒绝后对话列表


    [Header("交易配置")]
    public List<ItemData> requestItems; //索取物品列表
    public List<ItemData> offerItems; //带来物品列表

    [Header("出现时间配置")]
    public int minAppearRound; //最早出现回合
    public int maxAppearRound; //最晚出现回合
    [Header("Npc金钱奖励")]
    public int minPerRequestAwardGolds; //每交易一个需求物品可能获得的最低金钱
    public int maxPerRequestAwardGolds; //每交易一个需求物品可能获得的最高金钱
    public int minPerNonrequestAwardGolds; //每交易一个非需求物品可能获得的最低金钱
    public int maxPerNonrequestAwardGolds; //每交易一个非需求物品可能获得的最高金钱
    public int trueChoiceAwardGolds; //选择正确奖励金钱
    public int falseChoicePunishGolds; //选择错误惩罚金钱
    [Header("Npc好感度配置")]
    public int initialFavorability;

    [Header("Npc去往过去/未来理由配置")]
    public List<string> reasonToFurture; //去往未来理由列表
    public List<string> reasonToPast; //去往过去理由列表



    [Header("NPC出现条件")]
    public bool isMoneyMore10K;
    public bool isRoundMore5;

    public string GetRandomGreeting() => greetingDialogues[Random.Range(0, greetingDialogues.Count)]; //获取随机招呼语

    public ItemData GetRandomRequestItem() => requestItems[Random.Range(0, requestItems.Count)]; //获取随机物品要求
    public ItemData GetRandomOfferItem() => offerItems[Random.Range(0, offerItems.Count)];
    public string GetDescription() => npcDescription;
    public int GetRandomRequestAwardGolds() => Random.Range(minPerRequestAwardGolds, maxPerRequestAwardGolds + 1);
    public int GetRandomNonrequestAwardGolds() => Random.Range(minPerNonrequestAwardGolds, maxPerNonrequestAwardGolds + 1);




}