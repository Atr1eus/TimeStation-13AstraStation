using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class NpcCard
{
    public NpcData data;
    public string description;
    public NpcCard(NpcData npc)
    {
        this.data = npc;
        if (npc.type == NpcType.Normal)
            description = npc.GetDescription();
        else
        {
            description = npc.storyNpcDescriptions[NpcManager.Instance.npcDictionary[data.npcName].selectedTimes];
        }

    }
}