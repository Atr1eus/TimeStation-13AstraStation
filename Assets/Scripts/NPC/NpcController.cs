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
    public bool isRequestSuccess = false; //是否获取到了需要的物品
    // public ItemData currentOfferItem { get; protected set; } //当前npc给予物品
    public int currentOfferAmount { get; protected set; }
    public ItemData currentRequestItem { get; protected set; } //当前npc索求物品
    public int currentRequestAmount { get; protected set; } 
    public int perRequestAwardGolds { get; protected set; } //获得索要物品时 每个索要物品给予的金钱
    public int perNonrequestAwardGolds { get; protected set; } //获得非索要物品时 每个非索要物品给予的金钱

    public void InitializeController(Npc npc)
    {
        this.npc = npc;
        npcSpawnTime = Time.time;
        isRequestSuccess = false;
        isTradeFinish = false;
        // InitializeRandomOfferItem();
        // InitializeRandomRequestItem();
        perRequestAwardGolds = npc.data.GetRandomRequestAwardGolds();
        perNonrequestAwardGolds = npc.data.GetRandomNonrequestAwardGolds();
        // Debug.Log($"{npc.data.name}向你索要{currentRequestAmount}个{currentRequestItem.name},他给你带来了{currentOfferAmount}个{currentOfferItem.name}");
    }
    public int AffordForItems(ItemData itemdata,int amount) //计算Npc获得物品后给予玩家的金钱
    {
        int total = 0;
        if(itemdata == currentRequestItem)
        {
            if(amount > currentRequestAmount)
            {
                total += currentRequestAmount * perRequestAwardGolds + (amount - currentRequestAmount) * perNonrequestAwardGolds;
                currentRequestAmount = 0;
            }
            else 
            {
                total += amount * perRequestAwardGolds;
                currentRequestAmount -= amount;
            }
        }
        else
        {
            total = amount * perRequestAwardGolds;
        }
        return total;
    }
    // public void InitializeRandomOfferItem()
    // {
    //     //初始化随机OfferItem
    //     currentOfferItem = npc.data.GetRandomOfferItem();
    //     currentOfferAmount = Random.Range(currentOfferItem.minOfferAmount, currentOfferItem.maxOfferAmount + 1);
    // }
    // public void InitializeRandomRequestItem()
    // {
    //     //初始化随机RequestItem
    //     currentRequestItem = npc.data.GetRandomRequestItem();
    //     currentRequestAmount = Random.Range(currentRequestItem.minRequestAmount, currentRequestItem.maxOfferAmount + 1);
    // }

    public void OnTradeEnter() //交易开始行为
    {
        HandMoveIn();
    }
    public void OnTradeStep() //交易过程update
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

    public void HandMoveIn() //手进入动画 
    {
        //todo
        Debug.Log("手进入动画播放");
    }
    public void HandMoveOut()
    {
        //todo
        Debug.Log("手移出动画播放");
    }
}
