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
    [Header("回合配置")]
    [SerializeField] public int selectionsPerRound = 5; //单回合npc出现数
    [SerializeField] public int maxSelectionsPerRound = 2;//单回合最多可选择npc数
    [SerializeField] public float roundDuration = 180f;//单回合持续时间

    [Header("当前回合状态")]
    public int currentRound = 0; //回合数
    public int remainingSelections; //剩余选择数
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
    public List<Npc> currentRoundNpcs = new List<Npc>();

    public UnityEvent OnRoundStart;
    public UnityEvent OnRoundEnd;
    public UnityEvent OnRountStep;

    public void StartNewRound() //新回合开始时调用
    {
        ClearCurrentNpc();
        currentRound++;
        remainingSelections = maxSelectionsPerRound;
        canSelect = true;
        DecriseSelectionTimes();//回合开始选择一次 次数要减去
        roundTimer = roundDuration;
        isRoundActive = true;
        InitializeAvaliableNormalNpcDatas();
        InitializeAvaliableStoryNpcDatas();
        InitializeRoundNpcDatas();
        InitializeRoundNpcs();
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
        foreach (var npc in wholeStoryNpcDatas)
        {
            if (NpcAppearConditions.CanNpcAppear(npc) && npc.minAppearRound <= currentRound
                && npc.maxAppearRound >= currentRound && NpcManager.Instance.npcDictionary[npc.name].isSelected == false)
            {
                availableStoryNpcDatas.Add(npc);
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
            currentRoundNpcs.Add(NpcManager.Instance.npcDictionary[npcdata.name]);
        }
        return currentRoundNpcs;
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
    public void DecriseSelectionTimes() //减一次选择机会
    {
        --remainingSelections;
        canSelect = remainingSelections > 0;
    }

    public void ClearCurrentNpc()
    {
        availableNormalNpcDatas.Clear();
        availableStoryNpcDatas.Clear();
        currentRoundNpcDatas.Clear();
        currentRoundNpcs.Clear();
    }
    protected override void Awake()
    {
        base.Awake();
        InitializeWholeNpcDatas();
    }
}