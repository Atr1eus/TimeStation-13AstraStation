using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// npc控制类 只关心npc的行为逻辑
/// </summary>
public class NpcController : MonoBehaviour
{
    public Npc npc;
    public float npcSpawnTime { get; protected set; } //当前npc的创建时间
    private bool isTradeFinish = false; //交换是否结束
    private bool isRequestSuccess = false; //是否获取到了需要的物品
    public ItemData currentOfferItem { get; protected set; } //当前npc给予物品
    public int currentOfferAmount { get; protected set; }
    public ItemData currentRequestItem { get; protected set; } //当前npc索求物品
    public int currentRequestAmount { get; protected set; }

    public void InitializeController(Npc npc)
    {
        this.npc = npc;
        npcSpawnTime = Time.time;
        isRequestSuccess = false;
        isTradeFinish = false;
    }
    public void InitializeRandomOfferItem()
    {
        //初始化随机OfferItem
        currentOfferItem = npc.GetRandomOfferItem();
        currentOfferAmount = Random.Range(currentOfferItem.minOfferAmount, currentOfferItem.maxOfferAmount + 1);
        //todo：UI显示
    }
    public void InitializeRandomRequestItem()
    {
        //初始化随机RequestItem
        currentRequestItem = npc.GetRandomRequestItem();
        currentRequestAmount = Random.Range(currentRequestItem.minRequestAmount, currentRequestItem.maxOfferAmount + 1);
        //todo：UI显示
    }
    public void HandMoveIn() //手进入动画 
    {

    }
    public void HandMoveOut()
    {
        
    }

    public void OnTradeEnter() //交易开始行为
    {
        HandMoveIn();
    }
    public void OnTradeStep() //交易过程update 更新isTradeSuccess参数
    {

    }
    public void OnTradeExit()
    {
        isTradeFinish = true;
        if (isRequestSuccess)
        {
            OnTradeAccepted();
        }
        else
        {
            OnTradeFailed();
        }
        HandMoveOut();
    }
    public void OnTradeAccepted() //交易成功命令 UI监听
    {

    }
    public void OnTradeFailed()
    {

    }

}
