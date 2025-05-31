using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// npc管理类 只关心npc在什么时候做
/// </summary>
public class NpcManager : SingletonMonoBehaviour<NpcManager>
{
    public List<Npc> Npcs = new List<Npc>(); 
    public Transform npcSpawnPoint;
    private int npcIndex = -1;
    public NpcController currentNpc { get; set; }
    private Npc GetNpcInRandom() => Npcs[Random.Range(0, Npcs.Count)]; //在列表中随机选取npc
    private Npc GetNpcInOrder() => Npcs[++npcIndex % Npcs.Count]; // 在列表中顺序选取npc

    protected override void Awake()
    {
        base.Awake();
        
    }
    void Start()
    {

    }
    void Update()
    {
        if (currentNpc == null)
        {
            SpawnNpc(0);
            return;
        }
        if(currentNpc != null) currentNpc.OnTradeStep();
    }

    private void SpawnNpc(int rule) //0为随机生成 1为顺序生成
    {
        //每次生成重新实例化NPC，初始化控制器
        Npc npc = rule == 0 ? GetNpcInRandom() : GetNpcInOrder();
        GameObject npcObj = Instantiate(npc.hand, npcSpawnPoint.position, Quaternion.identity);
        currentNpc = npcObj.GetComponent<NpcController>();
        currentNpc.Initialize(npc);
        currentNpc.OnTradeEnter();
    }
    private void ClearCurrentNpc() 
    {
        //NPC离开时 销毁NPC实例并置空控制器
        Destroy(currentNpc.gameObject);
        currentNpc = null;
    }

}