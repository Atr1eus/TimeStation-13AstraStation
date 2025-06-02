using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Issuer
{
    Tencent,
    CyGames
}
public class Ticket
{
    public NpcData npc;
    public string targetDate;
    public string leaveDate;
    public TravelTo travelTo;
    public int leaveRound;
    public GameObject ticketGraph;
    public Issuer ticketIssuer;
    public bool isTrueTicket;
    public Ticket()
    {
        isTrueTicket = true;
    }
}