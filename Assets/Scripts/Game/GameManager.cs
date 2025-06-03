using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public enum GameState
{
    Menu,
    Trading,
    Paused
}
public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public List<NpcData> wholeNormalNpcDataList;
    public List<NpcData> wholeStoryNpcDataList;
    public List<ItemData> wholeItemDataList;
    public List<GameObject> wholeNormalHandList;
    public NpcCardManager m_npcCard;
    [Header("Systems")]
    [SerializeField] private PlayerController m_player;
    [SerializeField] private NpcManager m_npc;
    [SerializeField] private InventorySystem m_inventory;
    [SerializeField] private InventoryBrowser m_inventoryUI;
    [SerializeField] private RoundManager m_round;
    [SerializeField] private SceneLoader m_sceneLoader;
    [SerializeField] private TicketManager m_ticket;

    //todo:UIManager

    protected override void Awake()
    {
        base.Awake();
    }
    protected virtual void Start()
    {
    }
    public void OnNextRoundClick() //下一回合按钮点击事件
    {
        if (m_npc.currentNpc != null) m_npc.currentNpc.OnTradeExit();
        CurrentDataToLastRound();
        m_npc.ClearCurrentNpc(); //清除当前npc
        m_round.StartNewRound(); //开始新的回合
        m_npcCard.InitializeRemainingNpcs(m_round.currentRoundNpcDatas);
        m_npcCard.InitializeCards(m_round.currentRoundNpcDatas);
    }
    public void CurrentDataToLastRound()
    {
        m_npc.CurrentNpcDataToLastRound(); //下一回合中暂存本回合npc数据
        m_inventory.CurrentInventoryDataToLastRound();
        m_round.CurrentRoundDataToLastRound();
        m_player.CurrentPlayerDataToLastRound();
    }
    public void OnNextNpcButtonClick() //下一个npc按钮点击事件
    {
        if (m_npc.currentNpc != null) m_npc.currentNpc.OnTradeExit();
        Debug.Log("下一位");
        m_npcCard.LoadRemainingCards();
        m_ticket.ClearCurrentTicket();
        m_npc.ClearCurrentNpc();
    }
    public void OnAgreeNpcButtonClick()
    {
        m_inventory.GetNpcOfferItems(m_npc.currentNpc); //不论是否正确，都会给予物品
        m_npc.AgreeNpcAward(); //根据实际情况增加或减少金钱
        m_round.DecriseSelectionTimes();
        m_npc.currentNpc.npc.Select(true);

        Debug.Log($"成功获取了{m_npc.currentNpc.currentOfferAmount}个{m_npc.currentNpc.currentOfferItem}");
    }
    public void OnDisagreeNpcButtonClick()
    {
        m_npc.DisagreeNpcAward();
        if (m_npc.currentNpc != null) m_npc.currentNpc.OnTradeExit();
        m_ticket.ClearCurrentTicket();
        m_npcCard.LoadRemainingCards();
        m_npc.currentNpc.npc.Select(false);
        m_npc.ClearCurrentNpc();
    }

    public void OnInventoryBrowseButtonClick()
    {
        m_inventoryUI.OnInventoryBrowseButtonClick();
        foreach (Item item in m_inventory.items)
        {
            Debug.Log($"拥有{item.data.name}{item.amount}个");
        }
    }

    public void OnRefuseNpcButtonClick()
    {
        m_npc.currentNpc.isRequestSuccess = false;
        m_npc.currentNpc.OnTradeExit();
    }

    public void OnOfferNpcItemButtonClick()
    {

    }
    #region  测试一些功能
    public ItemData item0;
    public ItemData item1;
    public ItemData item2;
    public void TESTOfferItem()
    {
        if (InventorySystem.Instance.HasEnoughItem(item0, 2))
        {
            int money = NpcManager.Instance.currentNpc.AffordForItems(item0, 2);
            m_inventory.RemoveItem(item0, 2);
            PlayerController.Instance.gold += money;
            Debug.Log($"交易了2个Item,获得了{money}元");
        }
        else Debug.Log("没有足够的Item");
    }

    public void TESTOfferIte1()
    {
        if (InventorySystem.Instance.HasEnoughItem(item1, 2))
        {
            int money = NpcManager.Instance.currentNpc.AffordForItems(item1, 2);
            m_inventory.RemoveItem(item1, 2);
            PlayerController.Instance.gold += money;
            Debug.Log($"交易了2个Item1,获得了{money}元");
        }
        else Debug.Log("没有足够的Item1");
    }

    public void TESTOfferItem2()
    {
        if (InventorySystem.Instance.HasEnoughItem(item2, 2))
        {
            int money = NpcManager.Instance.currentNpc.AffordForItems(item2, 2);
            m_inventory.RemoveItem(item2, 2);
            PlayerController.Instance.gold += money;
            Debug.Log($"交易了2个Item2,获得了{money}元");
        }
        else Debug.Log("没有足够的Item2");
    }
    #endregion

    public void ClearDatasBeforeLoading()
    {
        ClearNpcBeforeLoading();
        m_round.ClearBeforeLoading();
        m_inventory.ClearBeforeLoading();
        m_player.ClearBeforeLoading();
    }
    public void ClearNpcBeforeLoading()
    {
        foreach (var data in wholeStoryNpcDataList)
        {
            m_npc.npcDictionary[data.name].ClearNpc();
            m_npc.lastRoundNpcDictionary[data.name].ClearNpc();
        }
        foreach (var data in wholeNormalNpcDataList)
        {
            m_npc.npcDictionary[data.name].ClearNpc();
            m_npc.lastRoundNpcDictionary[data.name].ClearNpc();
        }
    }
    protected virtual void Update()
    {

    }


}
