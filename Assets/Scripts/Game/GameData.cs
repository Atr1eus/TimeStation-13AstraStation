using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
[Serializable]
public class GameData
{
    public RoundData currentRoundData;
    public RoundData lastRoundData; //上一回合的数据

}
[Serializable]
public class RoundData
{
    public int gold;
    public List<NpcDataEntry> normalNpcData;
    public List<NpcDataEntry> storyNpcData;
    public List<ItemSaveEntry> items = new List<ItemSaveEntry>();
    public int currentRound; //目前回合数
    public string currentDate;
    public int selectionsPerRound; //每回合可选择次数
    public int maxSelectionsPerRound; //每回合可提供选择数
}

[Serializable]
public class NpcDataEntry
{
    public int favorability;
    public int selectedTimes;
    public List<bool> isSelected;
    public string npcName;
    public int bit_0;
    public int bit_1;
    public int bit_2;
    public int branchNumber;
}
[Serializable]

public class ItemSaveEntry
{
    public string itemId;  // 对应 ItemData 的唯一标识
    public int amount;
}