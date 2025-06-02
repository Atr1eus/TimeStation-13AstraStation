using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum NpcAttribute
{
    Favorability,
    bit1,
    bit2,
    bit3
}

public class Npc
{
    public NpcData data;
    public Dictionary<NpcAttribute, int> attribute = new Dictionary<NpcAttribute, int>();
    public Npc(NpcData data)
    {
        this.data = data;
        attribute.Add(NpcAttribute.Favorability, data.initialFavorability);
    }
}