using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TravelTo
{
    Past,
    Future
}
public class Npc
{
    public NpcData data;
    public GameObject hand;
    public Ticket ticket;
    public int selectedTimes;
    public List<bool> isSelected = new List<bool>();
    public List<bool> canAppera = new List<bool> { true };
    public int favorability;
    public int bit_0;
    public int bit_1;
    public int bit_2;
    public int branchNumber;
    public Npc(NpcData data)
    {
        this.data = data;
        favorability = data.initialFavorability;
        selectedTimes = 0;
        if (data.type == NpcType.Story)
        {
            hand = data.hand;
            ticket = new Ticket(this);
            ticket.data = data.ticket[selectedTimes];
        }
        else hand = GetRandomHand();
        isSelected.Add(false);
    }
    public void Select(bool result)
    {
        isSelected[selectedTimes++] = true;
        canAppera.Add(result == ticket.isTrueTicket ? true : false);
        isSelected.Add(false);
    }
    public GameObject GetRandomHand()
    {
        return GameManager.Instance.wholeNormalHandList[Random.Range(0, GameManager.Instance.wholeNormalHandList.Count)];
    }
    public void ClearNpc()
    {
        selectedTimes = 0;
        isSelected.Clear();
        favorability = 0;
    }

}