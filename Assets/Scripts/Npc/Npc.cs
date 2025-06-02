using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TravelTo
{
    Past,
    Future
}
public enum NpcAttribute
{
    Favorability,
    bit_1,
    bit_2,
    bit_3
}

public class Npc
{
    public NpcData data;
    public GameObject hand;
    public Ticket ticket;
    public int selectedTimes;
    public bool isSelected = false;
    public Dictionary<NpcAttribute, int> attribute = new Dictionary<NpcAttribute, int>();
    public Npc(NpcData data)
    {
        this.data = data;
        attribute.Add(NpcAttribute.Favorability, data.initialFavorability);
        selectedTimes = 0;
        if (data.type == NpcType.Story) hand = data.hand;
        else hand = GetRandomHand();
    }
    public void Select()
    {
        selectedTimes++;
        isSelected = true;
    }
    public GameObject GetRandomHand()
    {
        return GameManager.Instance.wholeNormalHandList[Random.Range(0, GameManager.Instance.wholeNormalHandList.Count)];
    }

}