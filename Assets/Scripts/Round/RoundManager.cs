using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class RoundManager : SingletonMonoBehaviour<RoundManager>
{
    [Header("回合配置")]
    [SerializeField] private int selectionsPerRound = 5; //单回合npc出现数
    [SerializeField] private int maxSelectionsPerRound = 2;//单回合最多可选择npc数
    [SerializeField] private float roundDuration = 180f;//单回合持续时间

    [Header("当前回合状态")]
    private int currentRound = 0; //回合数
    private int remainingSelections; //剩余选择数
    private float roundTimer; //回合剩余时间
    private bool isRoundActive; //是否激活回合时间
    public bool canSelect = true;

    [Header("NPC池子")]
    public List<NpcData> wholeNpcDatas = new List<NpcData>(); //总NPC池子
    public List<NpcData> availableNpcDatas = new List<NpcData>(); //可出现NPC池子

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
        InitializeAvaliableNpcDatas();
        InitializeRoundNpcDatas();
        InitializeRoundNpcs();
        OnRoundStart?.Invoke();
    }

    public List<NpcData> InitializeAvaliableNpcDatas()//初始化本回合角色池
    {
        availableNpcDatas.Clear();
        Debug.Log($"InitializeAvailableNpcs: currentRound={currentRound}, wholeNpcs.Count={wholeNpcDatas.Count}");
        foreach (var npc in wholeNpcDatas)
        {
            if (NpcAppearConditions.CanNpcAppear(npc) && npc.minAppearRound <= currentRound
                 && npc.maxAppearRound >= currentRound)
            {
                availableNpcDatas.Add(npc);
            }
        }
        return availableNpcDatas;
    }
    public List<NpcData> InitializeRoundNpcDatas()//初始化本回合角色
    {
        OutOfOrder(availableNpcDatas);
        for (int i = 0; i < selectionsPerRound && i < availableNpcDatas.Count; i++)
        {
            currentRoundNpcDatas.Add(availableNpcDatas[i]);
        }
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
        availableNpcDatas.Clear();
        currentRoundNpcDatas.Clear();
        currentRoundNpcs.Clear();
    }
}