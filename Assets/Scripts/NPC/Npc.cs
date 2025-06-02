using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum NpcAttribute
{
    Favorability
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