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

    public PlayerController Player
    {
        get
        {
            if (m_player == null)
                m_player = PlayerController.Instance;
            return m_player;
        }
    }
    public NpcManager NpcM
    {
        get
        {
            if (m_npcManager == null)
                m_npcManager = NpcManager.Instance;
            return m_npcManager;
        }
    }
    public InventorySystem Inventory
    {
        get
        {
            if (m_inventory == null)
                m_inventory = InventorySystem.Instance;
            return m_inventory;
        }
    }
    public RoundManager RoundManager
    {
        get
        {
            if (m_roundManager == null)
                m_roundManager = RoundManager.Instance;
            return m_roundManager;
        }
    }
    public NpcCardManager CardManger
    {
        get
        {
            if (m_cardManager == null)
                m_cardManager = NpcCardManager.Instance;
            return m_cardManager;
        }
    }

    protected override void Awake()
    {
        base.Awake();

    }
    protected virtual void Start()
    {
        NewRoundEnter();
    }
    public void NewRoundEnter()
    {
        RoundManager.StartNewRound();
        NpcCardManager.Instance.remainingNpcs = RoundManager.Instance.currentRoundNpcs;
        NpcCardManager.Instance.InitializeCards(RoundManager.Instance.currentRoundNpcs);
    }
    protected virtual void Update()
    {

    }
    public void InitializeCardUI()
    {

    }
}
