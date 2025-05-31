using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : SingletonMonoBehaviour<PlayerController>
{
    public int gold { get; private set;}

    public bool CanAfford(ItemData item,int amount) => item.itemPrice * amount <= gold;
    public void Afford(ItemData item,int amount = 1) => gold -= item.itemPrice * amount;
    void Update()
    {
        TryInteract();
    }
    private void TryInteract()
    {
        if(Input.GetMouseButtonDown(0))
        {
            //todo:UI½»»¥
        }
    }

}