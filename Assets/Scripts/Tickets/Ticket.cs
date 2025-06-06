using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Ticket
{
    public TicketData data;
    public NpcData npc;
    public string targetDate;
    public string leaveDate;
    public TravelTo travelTo;
    public int leaveRound;
    public Sprite ticketGraph;
    public string ticketIssuer;
    public bool isTrueTicket;
    public Ticket()
    {
        isTrueTicket = true;
    }
    public Ticket(Npc npc)
    {
        isTrueTicket = true;
        if (npc.data.type == NpcType.Story)
        {
            this.npc = npc.data;
        }
    }
}