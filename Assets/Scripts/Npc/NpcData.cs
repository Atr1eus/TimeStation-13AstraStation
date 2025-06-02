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
    public GameObject hand; //��(����npc��)
    public NpcType type; //��ͨ/����
    public NpcCareer career; //ְҵ
    public NpcSpecies species; //����
    public TravelTo travelTo; //ȥ��ȥ����δ��
    [TextArea] public string npcDescription; //��ò�Ƚ���


    [Header("对话配置")]
    public List<string> greetingDialogues; //初始对话列表
    public List<string> successDialogues; //同意给予对话列表
    public List<string> refuseDialogues; //拒绝给予对话列表


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
    public int trueChoiceAwardGolds; //ѡ����ȷ������Ǯ
    public int falseChoicePunishGolds; //ѡ�����ͷ���Ǯ
    [Header("Npc好感度配置")]
    public int initialFavorability;

    [Header("Npcȥ����ȥ/δ����������")]
    public List<string> reasonToFurture; //ȥ��δ�������б�
    public List<string> reasonToPast; //ȥ����ȥ�����б�

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