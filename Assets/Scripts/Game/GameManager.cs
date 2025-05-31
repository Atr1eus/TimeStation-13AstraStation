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
    //todo£ºUIManager

    public PlayerController Player
    {
        get
        {
            if (m_player == null)
                m_player = PlayerController.Instance;
            return m_player;
        }
    }
    public NpcManager Npc
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

    protected override void Awake()
    {
        base.Awake();

    }

}
