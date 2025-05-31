using System.ComponentModel.Design.Serialization;
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
    [SerializeField] private NpcManager m_npcManager;
    [SerializeField] private InventorySystem m_inventory;
    [SerializeField] private RoundManager m_roundManager;
    [SerializeField] private NpcCardManager m_cardManager;

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
        NpcManager.Instance.currentNpc.OnTradeExit();
        NpcManager.Instance.ClearCurrentNpc();
        RoundManager.Instance.StartNewRound();
        NpcCardManager.Instance.remainingNpcs = RoundManager.Instance.currentRoundNpcs;
        NpcCardManager.Instance.InitializeCards(RoundManager.Instance.currentRoundNpcs);
    }
    public void OnNextNpcButtonClick()
    {
        if (!RoundManager.Instance.canSelect)
        {
            Debug.Log("无法再进行选择了");
            return;
        }
        NpcManager.Instance.currentNpc.OnTradeExit();
        RoundManager.Instance.DecriseSelectionTimes();
        NpcCardManager.Instance.LoadRemainingCards();
        NpcManager.Instance.ClearCurrentNpc();
    }
    public void OnAcceptNpcOfferButtonClick()
    {
        InventorySystem.Instance.AddItem(new Item(NpcManager.Instance.currentNpc.currentOfferItem, NpcManager.Instance.currentNpc.currentOfferAmount));

        Debug.Log($"成功获取了{NpcManager.Instance.currentNpc.currentOfferAmount}个{NpcManager.Instance.currentNpc.currentOfferItem}");
    }

    public void OnInventoryBrowseButtonClick()
    {
        NpcManager.Instance.currentNpc.isRequestSuccess = true;
        foreach (Item item in InventorySystem.Instance.items)
        {
            Debug.Log($"拥有{item.data.name}{item.amount}个");
        }
    }

    public void OnRefuseNpcButtonClick()
    {
        NpcManager.Instance.currentNpc.isRequestSuccess = false;
        NpcManager.Instance.currentNpc.OnTradeExit();
    }

    public void OnOfferNpcItemButtonClick()
    {

    }
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
