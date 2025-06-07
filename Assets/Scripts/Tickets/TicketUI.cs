using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TicketUI : MonoBehaviour
{
    public Text nameText;
    public Text ageText;
    public Text sexText;
    public Text targetDateText;
    public Text travelDateText;
    public void SetUp(Ticket ticket)
    {
        nameText.text = "姓名：" + ticket.npc.npcName;
        ageText.text = "年龄：" + ticket.npc.age;
        targetDateText.text = "目的时间：" + ticket.targetDate;
        travelDateText.text = "出发时间：" + ticket.leaveDate;

    }
}