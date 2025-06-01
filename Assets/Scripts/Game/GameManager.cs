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
    [Header("Systems")]
    [SerializeField] private PlayerController m_player;
    [SerializeField] private NpcManager m_npc;
    [SerializeField] private InventorySystem m_inventory;
    [SerializeField] private RoundManager m_round;
    [SerializeField] private NpcCardManager m_npcCard;

    //todo:UIManager

    protected override void Awake()
    {
        base.Awake();
    }
    protected virtual void Start()
    {
    }
    public void OnNextRoundClick()
    {
        m_npc.currentNpc.OnTradeExit();
        m_npc.ClearCurrentNpc();
        m_round.StartNewRound();
        m_npcCard.InitializeRemainingNpcs(m_round.currentRoundNpcs);
        m_npcCard.InitializeCards(m_round.currentRoundNpcs);
    }
    public void OnNextNpcButtonClick()
    {
        if (!m_round.canSelect)
        {
            Debug.Log("无法再进行选择了");
            return;
        }
        m_npc.currentNpc.OnTradeExit();
        m_round.DecriseSelectionTimes();
        m_npcCard.LoadRemainingCards();
        m_npc.ClearCurrentNpc();
    }
    public void OnAcceptNpcOfferButtonClick()
    {
        m_inventory.GetNpcOfferItems(m_npc.currentNpc);

        Debug.Log($"成功获取了{m_npc.currentNpc.currentOfferAmount}个{m_npc.currentNpc.currentOfferItem}");
    }

    public void OnInventoryBrowseButtonClick()
    {
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


    /// <summary>
    /// 测试给予功能
    /// </summary>
    public ItemData item0;
    public ItemData item1;
    public ItemData item2;
    public void TESTOfferItem()
    {
        if (InventorySystem.Instance.HasEnoughItem(item0, 2))
        {
            int money = NpcManager.Instance.currentNpc.AffordForItems(item0, 2);
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
            PlayerController.Instance.gold += money;
            Debug.Log($"交易了2个Item2,获得了{money}元");
        }
        else Debug.Log("没有足够的Item2");
    }
    protected virtual void Update()
    {

    }


}
