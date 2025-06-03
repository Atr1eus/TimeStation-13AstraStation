using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
public class TicketManager : SingletonMonoBehaviour<TicketManager>
{
    public Ticket currentTicket;
    public Transform ticketContainer;
    public GameObject ticketPrefab;
    public GameObject currentTicketObj;
    public List<NpcData> wholeNormalNpcDataList = new List<NpcData>();
    private int ticketIndex;
    [Header("npc信息出错概率")]
    public float npcWrongProbability = 0.1f;

    [Header("发行商与外貌不一致概率")]
    public float issue_graphWrongProbility = 0.1f;
    [Header("Npc出发时间与当前回合不一致概率")]
    public float leaveRoundWrongProbility = 0.1f;
    [Header("票的发行商池及外貌池(不同发行商对应外貌请一一对应)")]
    public List<Issuer> ticketIssure;
    public List<GameObject> ticket;
    [Header("目的时间及对应回合数(若目标时间不在游戏时间范围内,前往过去填写-2,前往未来填写-1)")]
    public List<string> targetDate;
    public List<int> targetRound;

    [Header("出发时间及对应回合数")]
    public List<string> leaveDate;
    public List<int> leaveRound;

    public void InitializeWholeNormalNpcDatas()
    {
        wholeNormalNpcDataList = GameManager.Instance.wholeNormalNpcDataList;
    }
    public Ticket GetTrueTicket()
    {
        currentTicket = new Ticket();
        currentTicket.npc = NpcManager.Instance.currentNpc.npc.data;
        currentTicket.ticketGraph = GetRandomTrueTicketGraph();
        currentTicket.targetDate = GetRandomTrueTargetDate();
        currentTicket.leaveDate = GetTrueLeaveDate(currentTicket);
        return currentTicket;
    }
    public void InitializeTicket()
    {

        float screenWidth = Screen.width;
        float xPos = screenWidth / 10f;
        float yPos = Screen.height / 2f; // 默认居中
        currentTicketObj = Instantiate(ticketPrefab, ticketContainer);

        RectTransform ticketRect = currentTicketObj.GetComponent<RectTransform>();
        Vector2 screenPos = new Vector2(xPos, yPos);

        Vector2 anchoredPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)ticketContainer,
            screenPos,
            null,
            out anchoredPos
        );
        ticketRect.anchoredPosition = anchoredPos;


        TicketUI ticketUI = currentTicketObj.GetComponent<TicketUI>();

        ticketUI.SetUp(currentTicket);
    }
    public void InitializeTicket(Ticket ticket)
    {

        float screenWidth = Screen.width;
        float xPos = screenWidth / 10f;
        float yPos = Screen.height / 2f; // 默认居中
        currentTicketObj = Instantiate(ticketPrefab, ticketContainer);

        RectTransform ticketRect = currentTicketObj.GetComponent<RectTransform>();
        Vector2 screenPos = new Vector2(xPos, yPos);

        Vector2 anchoredPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)ticketContainer,
            screenPos,
            null,
            out anchoredPos
        );
        ticketRect.anchoredPosition = anchoredPos;


        TicketUI ticketUI = currentTicketObj.GetComponent<TicketUI>();

        ticketUI.SetUp(ticket);
    }
    public Ticket GetStoryNpcTicket(Npc npc)
    {

        npc.ticket.ticketGraph = npc.data.ticket[npc.selectedTimes].ticketGraph;
        npc.ticket.travelTo = npc.data.ticket[npc.selectedTimes].travelTo;
        npc.ticket.targetDate = npc.data.ticket[npc.selectedTimes].targetDate;
        npc.ticket.isTrueTicket = npc.data.ticket[npc.selectedTimes].isTrueTicket;
        npc.ticket.leaveDate = GetTrueLeaveDate(npc.ticket);
        return npc.ticket;
    }
    //随机生成票
    public Ticket GetRandomTicket()
    {
        currentTicket = new Ticket();
        currentTicket.npc = GetRandomNpcData();
        currentTicket.ticketGraph = GetRandomTicketGraph();
        currentTicket.targetDate = GetRandomTargetDate();
        currentTicket.leaveDate = GetRandomLeaveDate();
        return currentTicket;
    }
    public NpcData GetRandomNpcData()
    {
        float randomNumc = Random.Range(0, 100);
        if (randomNumc < npcWrongProbability * 100.0)
        {
            return wholeNormalNpcDataList[Random.Range(0, wholeNormalNpcDataList.Count)];
        }
        return NpcManager.Instance.currentNpc.npc.data;
    }
    /// <summary>
    /// 获取随机外貌，在此基础上手动控制概率获取随机发行商
    /// </summary>
    /// <returns></returns>
    public GameObject GetRandomTicketGraph()
    {
        int index = Random.Range(0, ticket.Count);
        ticketIndex = index;
        currentTicket.ticketIssuer = GetRandomIssure();
        return ticket[index];
    }
    public Issuer GetRandomIssure()
    {
        float randomNumc = Random.Range(0, 100);
        if (randomNumc < issue_graphWrongProbility * 100.0)
        {
            currentTicket.isTrueTicket = false;
            return ticketIssure[Random.Range(0, ticketIssure.Count)];
        }
        return ticketIssure[ticketIndex];
    }
    public GameObject GetRandomTrueTicketGraph()
    {
        int index = Random.Range(0, ticket.Count);
        ticketIndex = index;
        currentTicket.ticketIssuer = GetTrueIssure();
        return ticket[index];
    }
    public Issuer GetTrueIssure()
    {
        return ticketIssure[ticketIndex];
    }
    public string GetRandomTargetDate()
    {
        int randomNum = Random.Range(0, targetDate.Count);
        while (targetRound[randomNum] == RoundManager.Instance.currentRound)
        {
            currentTicket.isTrueTicket = false;
            randomNum = Random.Range(0, targetDate.Count);
        }
        currentTicket.travelTo = GetTravelTo(targetRound[randomNum]);
        return targetDate[randomNum];
    }
    public string GetRandomTrueTargetDate()
    {
        int randomNum = Random.Range(0, targetDate.Count);
        while (targetRound[randomNum] == RoundManager.Instance.currentRound && currentTicket.travelTo != NpcManager.Instance.currentNpc.npc.data.travelTo)
        {
            currentTicket.isTrueTicket = false;
            randomNum = Random.Range(0, targetDate.Count);
            currentTicket.travelTo = GetTravelTo(targetRound[randomNum]);
        }
        return targetDate[randomNum];
    }
    public TravelTo GetTravelTo(int count)
    {
        if (count == -2) return TravelTo.Past;
        else if (count == -1) return TravelTo.Future;
        else
        {
            if (count <= RoundManager.Instance.currentRound)
            {
                return TravelTo.Past;
            }
            else return TravelTo.Future;
        }
    }
    public string GetRandomLeaveDate()
    {
        int randomNum = Random.Range(0, 100);
        if (randomNum < leaveRoundWrongProbility * 100)
        {
            int randomLeaveRound = Random.Range(0, leaveDate.Count);
            currentTicket.isTrueTicket = false;
            currentTicket.leaveRound = leaveRound[randomLeaveRound];
            return leaveDate[randomLeaveRound];
        }
        for (int i = 0; i < leaveRound.Count; i++)
        {
            if (leaveRound[i] == RoundManager.Instance.currentRound)
            {
                currentTicket.leaveRound = leaveRound[i];
                return leaveDate[i];
            }
        }
        return null;
    }
    public string GetTrueLeaveDate(Ticket ticket)
    {
        for (int i = 0; i < leaveRound.Count; i++)
        {
            if (leaveRound[i] == RoundManager.Instance.currentRound)
            {
                ticket.leaveRound = leaveRound[i];
                return leaveDate[i];
            }
        }
        return null;
    }

    public void ClearCurrentTicket()
    {
        Destroy(currentTicketObj);
        currentTicket = null;
    }
    void Start()
    {
        InitializeWholeNormalNpcDatas();
    }

}