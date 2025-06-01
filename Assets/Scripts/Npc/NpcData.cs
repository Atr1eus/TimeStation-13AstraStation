using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// npc���������� ֻ����npc��ʲô
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
    public int age;
    public string npcName;
    public NpcSex sex;
    public GameObject hand;
    public NpcType type;
    [TextArea] public string npcDescription;

    [Header("�Ի�����")]
    public List<string> greetingDialogues; //��ʼ�Ի��б�
    public List<string> successDialogues; //ͬ�����Ի��б�
    public List<string> refuseDialogues; //�ܾ�����Ի��б�


    [Header("��������")]
    public List<ItemData> requestItems; //��ȡ��Ʒ�б�
    public List<ItemData> offerItems; //������Ʒ�б�

    [Header("����ʱ������")]
    public int minAppearRound; //������ֻغ�
    public int maxAppearRound; //�������ֻغ�
    [Header("Npc��Ǯ����")]
    public int minPerRequestAwardGolds; //ÿ����һ��������Ʒ���ܻ�õ���ͽ�Ǯ
    public int maxPerRequestAwardGolds; //ÿ����һ��������Ʒ���ܻ�õ���߽�Ǯ
    public int minPerNonrequestAwardGolds; //ÿ����һ����������Ʒ���ܻ�õ���ͽ�Ǯ
    public int maxPerNonrequestAwardGolds; //ÿ����һ����������Ʒ���ܻ�õ���߽�Ǯ
    [Header("Npc�øж�����")]
    public int initialFavorability;

    [Header("NPC��������")]
    public bool isMoneyMore10K;
    public bool isRoundMore5;

    public string GetRandomGreeting() => greetingDialogues[Random.Range(0, greetingDialogues.Count)]; //��ȡ����к���

    public ItemData GetRandomRequestItem() => requestItems[Random.Range(0, requestItems.Count)]; //��ȡ�����ƷҪ��
    public ItemData GetRandomOfferItem() => offerItems[Random.Range(0, offerItems.Count)];
    public string GetDescription() => npcDescription;
    public int GetRandomRequestAwardGolds() => Random.Range(minPerRequestAwardGolds, maxPerRequestAwardGolds + 1);
    public int GetRandomNonrequestAwardGolds() => Random.Range(minPerNonrequestAwardGolds, maxPerNonrequestAwardGolds + 1);




}