using Newtonsoft.Json;
using System.IO;
using UnityEngine;
using System;
using System.Collections.Generic;


[Serializable]
public class PlayerData
{
    public int gold;
}
[Serializable]
public class InventorySaveData
{
    public List<ItemSaveEntry> items = new List<ItemSaveEntry>();
}
[System.Serializable]
public class ItemSaveEntry
{
    public string itemId;  // 对应 ItemData 的唯一标识
    public int amount;
}
public class SaveSystem : SingletonMonoBehaviour<SaveSystem>
{
    [Header("Systems")]
    [SerializeField] private PlayerController m_player;
    [SerializeField] private NpcManager m_npc;
    [SerializeField] private InventorySystem m_inventory;
    [SerializeField] private InventoryBrowser m_inventoryUI;
    [SerializeField] private RoundManager m_round;
    [SerializeField] private SceneLoader m_sceneLoader;
    [SerializeField] private TicketManager m_ticket;
    [SerializeField] private GameManager m_gameManager;

    public void GameSave()
    {
        SavePlayerData();
        SaveInventory();
    }
    public void GameLoad()
    {
        LoadPlayerData();
        LoadInventory();
    }
    public void SaveInventory()
    {
        var saveData = new InventorySaveData();

        foreach (Item item in m_inventory.items)
        {
            saveData.items.Add(new ItemSaveEntry
            {
                itemId = item.data.itemID, // 假设 ItemData 有唯一 itemID
                amount = item.amount
            });
        }

        string json = JsonConvert.SerializeObject(saveData, Formatting.Indented);
        string savePath = Path.Combine(Application.persistentDataPath, "inventory_save.json");
        File.WriteAllText(savePath, json);
        Debug.Log("库存已保存");
    }
    public bool LoadInventory()
    {
        string savePath = Path.Combine(Application.persistentDataPath, "inventory_save.json");
        if (!File.Exists(savePath)) return false;

        try
        {
            string json = File.ReadAllText(savePath);
            InventorySaveData saveData = JsonConvert.DeserializeObject<InventorySaveData>(json);

            // 清空当前库存
            m_inventory.items.Clear();
            m_inventory.datas.Clear();

            // 根据ID还原ItemData引用
            foreach (ItemSaveEntry entry in saveData.items)
            {
                ItemData itemData = FindItemDataById(entry.itemId);
                m_inventory.AddItem(itemData, entry.amount);
            }
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"读取库存失败: {e.Message}");
            return false;
        }
    }
    private ItemData FindItemDataById(string itemId)
    {
        foreach (var item in m_inventory.items)
        {
            if (item.data.itemID == itemId)
            {
                return item.data;
            }
        }
        return null;
    }
    // 保存金币数据
    public void SavePlayerData()
    {
        string savePath = Path.Combine(Application.persistentDataPath, "player_save.json");
        PlayerData data = new PlayerData { gold = m_player.gold };
        string json = JsonConvert.SerializeObject(data, Formatting.Indented);

        File.WriteAllText(savePath, json);
        Debug.Log($"金币已保存: {m_player.gold}");
    }
    public bool LoadPlayerData()
    {
        m_player.gold = 0;
        string savePath = Path.Combine(Application.persistentDataPath, "player_save.json");
        //无存档
        if (!File.Exists(savePath))
        {
            Debug.Log("无存档文件，返回默认值");
            return false;
        }
        try
        {
            //读取json文件
            string json = File.ReadAllText(savePath);
            //反序列化json文件中的data
            PlayerData data = JsonConvert.DeserializeObject<PlayerData>(json);
            m_player.gold = data.gold;
            Debug.Log($"金币已读取: {m_player.gold}");
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"读取失败: {e.Message}");
            return false;
        }
    }


}