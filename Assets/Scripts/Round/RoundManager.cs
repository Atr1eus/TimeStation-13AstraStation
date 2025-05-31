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
    private int currentRound = 1; //回合数
    private int remainingSelections; //剩余选择数
    private float roundTimer; //回合剩余时间
    private bool isRoundActive; //是否激活回合时间

    [Header("NPC池子")]
    public List<Npc> wholeNpcs = new List<Npc>(); //总NPC池子
    public List<Npc> availableNpcs = new List<Npc>(); //可出现NPC池子

    public List<Npc> currentRoundNpcs = new List<Npc>(); //本回合出现NPC
    public UnityEvent OnRoundStart;
    public UnityEvent OnRoundEnd;
    public UnityEvent OnRountStep;

    public void StartNewRound() //新回合开始时调用
    {
        ClearCurrentNpc();
        currentRound++;
        remainingSelections = maxSelectionsPerRound;
        roundTimer = roundDuration;
        isRoundActive = true;
        InitializeAvaliableNpcs();
        InitializeRoundNpcs();
        OnRoundStart?.Invoke();
    }

    public List<Npc> InitializeAvaliableNpcs()//初始化本回合角色池
    {
        foreach (var npc in wholeNpcs)
        {
            if (NpcAppearConditions.CanNpcAppear(npc) && npc.minAppearRound <= currentRound
                 && npc.maxAppearRound >= currentRound)
            {
                availableNpcs.Add(npc);
            }
        }
        return availableNpcs;
    }
    public List<Npc> InitializeRoundNpcs()//初始化本回合角色
    {
        OutOfOrder(availableNpcs);
        for (int i = 0; i < selectionsPerRound && i < availableNpcs.Count; i++)
        {
            currentRoundNpcs.Add(availableNpcs[i]);
        }
        return currentRoundNpcs;
    }

    public List<Npc> OutOfOrder(List<Npc> Npcs) //随机打乱Npc池
    {
        System.Random randomNum = new System.Random();
        int index = 0;
        Npc temp;
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
    public void ClearCurrentNpc() 
    {
        availableNpcs.Clear();
        currentRoundNpcs.Clear();
    }
}