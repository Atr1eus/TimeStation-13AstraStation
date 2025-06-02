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
        nameText.text = "姓名：" + ticket.npc.name;
        ageText.text = "年龄：" + ticket.npc.age;
        sexText.text = "性别：" + ticket.npc.sex == "Man" ? "男" : "女";
        targetDateText.text = "目的时间：" + ticket.targetDate;
        travelDateText.text = "出发时间：" + ticket.leaveDate;

    }
}