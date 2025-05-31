using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class NpcCard 
{
    public Npc npc; 
    public string description;
    public NpcCard(Npc npc)
    {
        this.npc = npc;
        description = npc.GetDescription();
    }
}