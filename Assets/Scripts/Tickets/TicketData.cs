using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Ticket", menuName = "Game/Ticket")]
public class TicketData : ScriptableObject
{
    public NpcData npc;
    public string targetDate;
    public TravelTo travelTo;
    public Sprite ticketGraph;
    public string ticketIssuer;
    public bool isTrueTicket;

}