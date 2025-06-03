using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[System.Serializable]
public class PlayerController : SingletonMonoBehaviour<PlayerController>
{
    public int gold = 200;
    public int lastRoundGold;
    public bool CanAfford(ItemData item, int amount) => item.itemPrice * amount <= gold;
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
    void Update()
    {

    }
}