using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
[System.Serializable]
public class RoundManager : SingletonMonoBehaviour<RoundManager>
{
    public int lastRoundSelectionsPerRound;
    public int lastRoundMaxSelectionsPerRound;
    public int lastRound;
    [Header("回合配置")]
    [SerializeField] public int selectionsPerRound = 2; //单回合npc出现数
    [SerializeField] public int maxSelectionsPerRound = 1;//单回合最多可选择npc数
    [SerializeField] public float roundDuration = 180f;//单回合持续时间

    [Header("当前回合状态")]
    public int currentRound = 0; //回合数
    public int remainingSelections; //剩余选择数
    public int currentCanSelectNpcNum;
    private float roundTimer; //回合剩余时间
    private bool isRoundActive; //是否激活回合时间
    public bool canSelect = true;
    public bool isSelected = false;

    [Header("NPC池子(无需填写)")]
    public List<NpcData> wholeNormalNpcDatas = new List<NpcData>(); //总普通NPC池子
    public List<NpcData> wholeStoryNpcDatas = new List<NpcData>();

    public List<NpcData> availableNormalNpcDatas = new List<NpcData>(); //可出现NPC池子
    public List<NpcData> availableStoryNpcDatas = new List<NpcData>();

    public List<NpcData> currentRoundNpcDatas = new List<NpcData>(); //本回合出现NPC


    public List<string> decreaseItemList = new List<string>();
    public List<int> decreaseItemCountList = new List<int>();
    public List<Npc> currentStoryNpcList = new List<Npc>();

    public List<Npc> currentRoundNpcs = new List<Npc>();


    public List<Npc> currentSelectedNpcs = new List<Npc>();

    public List<bool> isRandomTicket = new List<bool>();

    public UnityEvent OnRoundStart;
    public UnityEvent OnRoundEnd;
    public UnityEvent OnRountStep;

    public void StartNewRound() //新回合开始时调用
    {
        ClearCurrentNpc();
        currentRound++;
        remainingSelections = maxSelectionsPerRound;
        canSelect = true;
        roundTimer = roundDuration;
        isRoundActive = true;
        currentCanSelectNpcNum = selectionsPerRound;
        InitializeRandomTicketList();
        InitializeAvaliableNormalNpcDatas();
        InitializeAvaliableStoryNpcDatas();
        InitializeRoundNpcDatas();
        InitializeRoundNpcs();
        DateExtensions.ToGameDate(1);
        OnRoundStart?.Invoke();

    }
    public void InitializeWholeNpcDatas()
    {
        wholeNormalNpcDatas = GameManager.Instance.wholeNormalNpcDataList;
        wholeStoryNpcDatas = GameManager.Instance.wholeStoryNpcDataList;
    }
    public List<NpcData> InitializeAvaliableNormalNpcDatas()//初始化本回合角色池
    {
        availableNormalNpcDatas.Clear();
        foreach (var npc in wholeNormalNpcDatas)
        {
            if (NpcAppearConditions.CanNpcAppear(npc) && npc.minAppearRound <= currentRound
                 && npc.maxAppearRound >= currentRound)
            {
                availableNormalNpcDatas.Add(npc);
            }
        }
        return availableNormalNpcDatas;
    }
    public List<NpcData> InitializeAvaliableStoryNpcDatas()
    {
        availableStoryNpcDatas.Clear();
        foreach (var npcData in wholeStoryNpcDatas)
        {
            Npc npc = NpcManager.Instance.npcDictionary[npcData.npcName];
            if (NpcAppearConditions.CanNpcAppear(npcData) && npc.selectedTimes < npcData.storyNpcMinAppearRounds.Count
                && npcData.storyNpcMinAppearRounds[npc.selectedTimes] <= currentRound
                && npcData.storyNpcMaxAppearRounds[npc.selectedTimes] >= currentRound
                && NpcManager.Instance.npcDictionary[npcData.npcName].isSelected[npc.selectedTimes] == false
                && NpcManager.Instance.npcDictionary[npcData.npcName].canAppera[npc.selectedTimes]
                && npc.selectedTimes < npc.data.ticket.Count)
            {
                availableStoryNpcDatas.Add(npcData);
            }
        }
        return availableStoryNpcDatas;
    }
    public List<NpcData> InitializeRoundNpcDatas()//初始化本回合角色
    {
        OutOfOrder(availableNormalNpcDatas);
        int count = 0;
        for (; count < selectionsPerRound && count < availableStoryNpcDatas.Count; count++) //优先选取剧情npc出现
        {
            currentRoundNpcDatas.Add(availableStoryNpcDatas[count]);
        }
        for (; count < selectionsPerRound && count < availableNormalNpcDatas.Count; count++)
        {
            currentRoundNpcDatas.Add(availableNormalNpcDatas[count]);
        }
        OutOfOrder(currentRoundNpcDatas);
        return currentRoundNpcDatas;
    }
    public List<Npc> InitializeRoundNpcs()
    {
        foreach (var npcdata in currentRoundNpcDatas)
        {
            currentRoundNpcs.Add(NpcManager.Instance.npcDictionary[npcdata.npcName]);
        }
        return currentRoundNpcs;
    }

    public List<bool> InitializeRandomTicketList()
    {
        for (int i = 0; i < maxSelectionsPerRound; i++)
        {
            isRandomTicket.Add(false);
        }
        for (int i = 0; i < selectionsPerRound - maxSelectionsPerRound; i++)
        {
            isRandomTicket.Add(true);
        }
        OutOfOrder(isRandomTicket);
        return isRandomTicket;
    }

    public List<NpcData> OutOfOrder(List<NpcData> Npcs) //随机打乱Npc池
    {
        System.Random randomNum = new System.Random();
        int index = 0;
        NpcData temp;
        for (int i = 0; i < Npcs.Count; i++)
        {
            index = randomNum.Next(0, Npcs.Count);
            if (index != i)
            {
                temp = Npcs[i];
                Npcs[i] = Npcs[index];
                Npcs[index] = temp;
            }
        }
        return Npcs;
    }

    public List<bool> OutOfOrder(List<bool> list) //随机打乱ticket池
    {
        System.Random randomNum = new System.Random();
        int index = 0;
        bool temp;
        for (int i = 0; i < list.Count; i++)
        {
            index = randomNum.Next(0, list.Count);
            if (index != i)
            {
                temp = list[i];
                list[i] = list[index];
                list[index] = temp;
            }
        }
        return list;
    }

    public void DecriseSelectionTimes() //减一次选择机会
    {
        --remainingSelections;
        canSelect = remainingSelections > 0;
    }
    public void AddCurrentRoundSelectedNpc(NpcData data)
    {
        Npc npc = NpcManager.Instance.npcDictionary[data.npcName];
        currentSelectedNpcs.Add(npc);
    }
    public void ClearCurrentNpc()
    {
        availableNormalNpcDatas.Clear();
        availableStoryNpcDatas.Clear();
        currentRoundNpcDatas.Clear();
        currentRoundNpcs.Clear();
        currentSelectedNpcs.Clear();
        currentStoryNpcList.Clear();
        decreaseItemList.Clear();
        decreaseItemCountList.Clear();
    }
    public void CurrentRoundDataToLastRound()
    {
        lastRound = currentRound;
        lastRoundMaxSelectionsPerRound = maxSelectionsPerRound;
        lastRoundSelectionsPerRound = selectionsPerRound;
    }
    public void ClearBeforeLoading()
    {
        currentRound = 0;
        maxSelectionsPerRound = 0;
        selectionsPerRound = 0;
        lastRoundSelectionsPerRound = 0;
        lastRoundMaxSelectionsPerRound = 0;
        lastRound = 0;
        currentStoryNpcList.Clear();
        decreaseItemCountList.Clear();
        decreaseItemList.Clear();

    }
    public void AddMaxSelectionsPerRound()
    {
        maxSelectionsPerRound++;
    }
    public void AddSelectionsPerRound()
    {
        selectionsPerRound++;
    }

    protected override void Awake()
    {
        base.Awake();
        InitializeWholeNpcDatas();
    }
    public void UpdateDailyDecreaseListData()
    {
        foreach (var item in InventorySystem.Instance.items)
        {
            if (InventorySystem.Instance.lastRoundItems.Contains(item) &&
                InventorySystem.Instance.lastRoundItems[InventorySystem.Instance.lastRoundItems.IndexOf(item)].amount >
                InventorySystem.Instance.items[InventorySystem.Instance.items.IndexOf(item)].amount)
            {
                decreaseItemList.Add(item.data.name);
                decreaseItemCountList.Add(item.amount);
            }
        }
    }
}