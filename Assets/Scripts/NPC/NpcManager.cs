using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// npc管理类 只关心npc在什么时候做
/// </summary>
public class NpcManager : SingletonMonoBehaviour<NpcManager>
{
    public List<NpcData> npcDatas = new List<NpcData>();
    public Dictionary<string, Npc> npcDictionary = new Dictionary<string, Npc>();
    public Transform npcSpawnPoint;
    private int npcIndex = -1;
    public NpcController currentNpc;
    public GameObject npcObj;
    private NpcData GetNpcInRandom() => npcDatas[Random.Range(0, npcDatas.Count)]; //在列表中随机选取npc
    private NpcData GetNpcInOrder() => npcDatas[++npcIndex % npcDatas.Count]; // 在列表中顺序选取npc

    protected override void Awake()
    {
        base.Awake();
        currentNpc = FindObjectOfType<NpcController>();
        InitializeNpcs();
    }
    public void InitializeNpcs()
    {
        foreach (var npcData in npcDatas)
        {
            npcDictionary.Add(npcData.name, new Npc(npcData));
        }
    }
    public void SpawnNpc(int rule) //0为随机生成 1为顺序生成 废弃功能 暂时留着
    {
        //每次生成重新实例化NPC，初始化控制器
        NpcData npcdata = rule == 0 ? GetNpcInRandom() : GetNpcInOrder();
        Npc npc = npcDictionary[npcdata.name];
        GameObject npcObj = Instantiate(npc.data.hand, npcSpawnPoint.position, Quaternion.identity);
        currentNpc = npcObj.AddComponent<NpcController>();
        currentNpc.InitializeController(npc);
        currentNpc.OnTradeEnter();
    }
    public void SpawnNpc(NpcData npcdata) //直接根据NPC生成 不与NpcCard产生关联 半废弃
    {
        GameObject npcObj = Instantiate(npcdata.hand, npcSpawnPoint.position, Quaternion.identity);
        Npc npc = npcDictionary[npcdata.name];
        currentNpc = npcObj.AddComponent<NpcController>();
        currentNpc.InitializeController(npc);
        currentNpc.OnTradeEnter();
    }
    public void InitializeCurrentNpc(NpcCard npcCard)//npcCard选择事件 根据玩家的选择定义currentNpc
    {
        NpcData npcdata = npcCard.npc;
        Npc npc = npcDictionary[npcdata.name];
        npcObj = Instantiate(npc.data.hand, npcSpawnPoint.position, Quaternion.identity);
        currentNpc = npcObj.AddComponent<NpcController>();
        currentNpc.InitializeController(npc);
        currentNpc.OnTradeEnter();
        Debug.Log($"currentNpc:{currentNpc.npc.data.name}");
    }
    public void ClearCurrentNpc()
    {
        Destroy(npcObj);
        //NPC离开时 销毁NPC实例并置空控制器
        if (currentNpc.gameObject != null)
        {
            Destroy(currentNpc.gameObject);
        }
        currentNpc = null;
    }

}