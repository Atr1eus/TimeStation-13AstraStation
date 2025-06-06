using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[System.Serializable]
public class PlayerController : SingletonMonoBehaviour<PlayerController>
{
    public int gold = 10;
    public int lastRoundGold;
    public int trainCountRank = 0;
    public int customerCountRank = 1;
    public readonly int MaxTrainRank = 3;
    public readonly int MaxCustomerCountRank = 3;
    public List<int> roundLimitMoney = new List<int>();
    public List<int> trainRankUpNeedMoney = new List<int>();
    public List<int> customerCountRankUpNeedMoney = new List<int>();
    public bool CanAfford(ItemData item, int amount) => item.itemPrice * amount <= gold;
    public bool CanAfford(int amount) => gold >= amount;
    public bool CanTrainRankUp()
    {
        if (trainCountRank >= MaxTrainRank) return false;
        return CanAfford(trainRankUpNeedMoney[trainCountRank]);
    }
    public bool CanCustomerRankUp()
    {
        if (customerCountRank >= MaxCustomerCountRank) return false;
        return CanAfford(customerCountRankUpNeedMoney[customerCountRank]);
    }
    public void Afford(ItemData item, int amount = 1) => gold -= item.itemPrice * amount;
    public void AddGold(int gold) => this.gold += gold;
    public void MinusGold(int gold) => this.gold -= gold;
    public void CurrentPlayerDataToLastRound()
    {
        lastRoundGold = gold;
    }
    public void ClearBeforeLoading()
    {
        gold = 0;
        lastRoundGold = 0;
    }
    public void TrainCountRankUp()
    {
        MinusGold(trainRankUpNeedMoney[trainCountRank++]);
    }
    public void CustomerCountRankUp()
    {
        MinusGold(customerCountRankUpNeedMoney[customerCountRank++]);
    }
    void Update()
    {

    }
}