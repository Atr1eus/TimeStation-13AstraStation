using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// npc管理类 只关心npc在什么时候做
/// </summary>

[System.Serializable]
public class NpcManager : SingletonMonoBehaviour<NpcManager>
{
    public List<NpcData> npcDatas = new List<NpcData>();
    public Dictionary<string, Npc> npcDictionary = new Dictionary<string, Npc>();
    public Dictionary<string, Npc> lastRoundNpcDictionary = new Dictionary<string, Npc>();
    public Transform handSpawnPoint;
    private int npcIndex = -1;
    public NpcController currentNpc;
    public GameObject npcObj;
    private NpcData GetNpcInRandom() => npcDatas[Random.Range(0, npcDatas.Count)]; //在列表中随机选取npc
    private NpcData GetNpcInOrder() => npcDatas[++npcIndex % npcDatas.Count]; // 在列表中顺序选取npc
    public int nowTicketIdx = 0;

    protected override void Awake()
    {
        base.Awake();
        InitializeNpcDatas();
        InitializeNpcs();
    }
    void Start()
    {
        currentNpc = FindObjectOfType<NpcController>();
    }
    public void InitializeNpcDatas()
    {
        npcDatas.Clear();
        foreach (var npc in GameManager.Instance.wholeNormalNpcDataList)
        {
            npcDatas.Add(npc);
        }
        foreach (var npc in GameManager.Instance.wholeStoryNpcDataList)
        {
            npcDatas.Add(npc);
        }

    }
    public void InitializeNpcs()
    {
        foreach (NpcData npcData in npcDatas)
        {
            npcDictionary.Add(npcData.npcName, new Npc(npcData));
            lastRoundNpcDictionary.Add(npcData.npcName, new Npc(npcData));
        }
    }
    public void SpawnNpc(int rule) //0为随机生成 1为顺序生成 废弃功能 暂时留着
    {
        //每次生成重新实例化NPC，初始化控制器
        NpcData npcdata = rule == 0 ? GetNpcInRandom() : GetNpcInOrder();
        Npc npc = npcDictionary[npcdata.npcName];
        GameObject npcObj = Instantiate(npc.data.hand, handSpawnPoint.position, Quaternion.identity);
        currentNpc = npcObj.AddComponent<NpcController>();
        currentNpc.InitializeController(npc);
        currentNpc.OnTradeEnter();
    }
    public void SpawnNpc(NpcData npcdata) //直接根据NPC生成 不与NpcCard产生关联 半废弃
    {
        GameObject npcObj = Instantiate(npcdata.hand, handSpawnPoint.position, Quaternion.identity);
        Npc npc = npcDictionary[npcdata.npcName];
        currentNpc = npcObj.AddComponent<NpcController>();
        currentNpc.InitializeController(npc);
        currentNpc.OnTradeEnter();
    }
    public void InitializeCurrentNpc(NpcCard npcCard)//npcCard选择事件 根据玩家的选择定义currentNpc
    {
        Debug.Log("开始初始化角色！");
        NpcData npcdata = npcCard.npc;
        Npc npc = npcDictionary[npcdata.npcName];
        npcObj = Instantiate(npc.hand, handSpawnPoint.position, Quaternion.identity);
        currentNpc = npcObj.AddComponent<NpcController>();
        currentNpc.InitializeController(npc);
        if (RoundManager.Instance.isRandomTicket[nowTicketIdx++])
            npc.ticket = TicketManager.Instance.GetRandomTicket();
        else npc.ticket = TicketManager.Instance.GetTrueTicket();
        TicketManager.Instance.InitializeTicket();
        if (npc.ticket.travelTo != npc.data.travelTo) npc.ticket.isTrueTicket = false;
        RoundManager.Instance.isSelected = true;
        currentNpc.OnTradeEnter();
        DialogueManager.Instance.SetGraph(npcdata.npcDialogue);
        Debug.Log($"currentNpc:{currentNpc.npc.data.npcName}");
    }
    public void InitializeCurrentStoryNpc(NpcCard npcCard)
    {
        NpcData npcdata = npcCard.npc;
        Npc npc = npcDictionary[npcdata.npcName];
        npcObj = Instantiate(npc.hand, handSpawnPoint.position, Quaternion.identity);
        currentNpc = npcObj.AddComponent<NpcController>();
        currentNpc.InitializeController(npc);
        currentNpc.npc.ticket = TicketManager.Instance.GetStoryNpcTicket(npc);
        TicketManager.Instance.InitializeTicket(npc.ticket);
        nowTicketIdx++;
        currentNpc.OnTradeEnter();
        DialogueManager.Instance.SetGraph(npcdata.npcDialogue);
        Debug.Log($"currentNpc:{currentNpc.npc.data.npcName}");
    }
    public void AgreeNpcAward()
    {
        if (currentNpc.npc.ticket.isTrueTicket)
        {
            PlayerController.Instance.AddGold(currentNpc.npc.data.trueChoiceAwardGolds);
            Debug.Log("你放过了一个正确的人");
            return;
        }
        PlayerController.Instance.MinusGold(currentNpc.npc.data.falseChoicePunishGolds);
        Debug.Log("判断错误！！！");
    }
    public void DisagreeNpcAward()
    {
        if (currentNpc.npc.ticket.isTrueTicket)
        {
            PlayerController.Instance.MinusGold(currentNpc.npc.data.falseChoicePunishGolds);
            Debug.Log("判断错误！！！");
            return;
        }
        PlayerController.Instance.AddGold(currentNpc.npc.data.trueChoiceAwardGolds);
        Debug.Log("你放过了一个正确的人");
    }
    public void ChangeDialogueNumber(int num)
    {
        currentNpc.npc.branchNumber = num;
    }
    public void SetFavorability(int num)
    {
        currentNpc.npc.favorability = num;
    }

    public void ClearCurrentNpc()
    {
        if (npcObj != null) Destroy(npcObj);
        //NPC离开时 销毁NPC实例并置空控制器
        if (currentNpc != null && currentNpc.gameObject != null)
        {
            Destroy(currentNpc.gameObject);
        }
        currentNpc = null;
    }
    public void CurrentNpcDataToLastRound()
    {
        lastRoundNpcDictionary = npcDictionary;
    }

}