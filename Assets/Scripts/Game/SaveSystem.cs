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
[Serializable]
public class RoundData
{
    public int currendRound;
    public int selectionsPerRound;
    public int maxSelectionsPerRound;
}

[Serializable]
public class NormalNpcDatas
{
    public List<NpcDataEntry> data;
}

[Serializable]
public class StoryNpcDatas
{
    public List<NpcDataEntry> data;
}

[Serializable]
public class NpcDataEntry
{
    public int favorability;
    public int selectedTimes;
    public bool isSelected;
    public string npcName;
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
        SaveRound();
        SaveStoryNpc();
        SaveNormalNpc();
    }
    public void GameLoad()
    {
        LoadPlayerData();
        LoadInventory();
        LoadRound();
        LoadStoryNpc();
        LoadNormalNpc();
    }
    public void SaveStoryNpc()
    {
        StoryNpcDatas npcData = new StoryNpcDatas { data = new List<NpcDataEntry>() };
        foreach (var data in m_gameManager.wholeStoryNpcDataList)
        {
            Npc npc = m_npc.npcDictionary[data.name];
            NpcDataEntry npcDataEntry = new NpcDataEntry
            {
                favorability = npc.favorability,
                npcName = npc.data.name,
                selectedTimes = npc.selectedTimes,
                isSelected = npc.isSelected
            };
            npcData.data.Add(npcDataEntry);
        }
        string json = JsonConvert.SerializeObject(npcData, Formatting.Indented);
        string savePath = Path.Combine(Application.persistentDataPath, "storyNpc_save.json");

        string encryptedJson = EncryptionUtility.Encrypt(json);
        File.WriteAllText(savePath, encryptedJson);
    }
    public bool LoadStoryNpc()
    {
        foreach (var data in m_gameManager.wholeStoryNpcDataList)
        {
            m_npc.npcDictionary[data.name].ClearNpc();
        }
        string savePath = Path.Combine(Application.persistentDataPath, "storyNpc_save.json");
        if (!File.Exists(savePath))
        {
            Debug.Log("无存档文件，返回默认值");
            return false;
        }
        try
        {
            //读取json文件
            string encryptedJson = File.ReadAllText(savePath);
            string json = EncryptionUtility.Decrypt(encryptedJson);
            //反序列化json文件中的data
            NormalNpcDatas data = JsonConvert.DeserializeObject<NormalNpcDatas>(json);
            for (int i = 0; i < data.data.Count; i++)
            {
                NpcDataEntry npc = data.data[i];
                m_npc.npcDictionary[npc.npcName].favorability = npc.favorability;
                m_npc.npcDictionary[npc.npcName].isSelected = npc.isSelected;
                m_npc.npcDictionary[npc.npcName].selectedTimes = npc.selectedTimes;
            }
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"读取失败: {e.Message}");
            return false;
        }
    }
    public void SaveNormalNpc()
    {
        StoryNpcDatas npcData = new StoryNpcDatas { data = new List<NpcDataEntry>() };
        foreach (var data in m_gameManager.wholeNormalNpcDataList)
        {
            Npc npc = m_npc.npcDictionary[data.name];
            NpcDataEntry npcDataEntry = new NpcDataEntry
            {
                favorability = npc.favorability,
                npcName = npc.data.name,
                selectedTimes = npc.selectedTimes,
                isSelected = npc.isSelected
            };
            npcData.data.Add(npcDataEntry);
        }
        string json = JsonConvert.SerializeObject(npcData, Formatting.Indented);
        string savePath = Path.Combine(Application.persistentDataPath, "normalNpc_save.json");

        string encryptedJson = EncryptionUtility.Encrypt(json);
        File.WriteAllText(savePath, encryptedJson);
    }
    public bool LoadNormalNpc()
    {
        foreach (var data in m_gameManager.wholeNormalNpcDataList)
        {
            m_npc.npcDictionary[data.name].ClearNpc();
        }
        string savePath = Path.Combine(Application.persistentDataPath, "normalNpc_save.json");
        if (!File.Exists(savePath))
        {
            Debug.Log("无存档文件，返回默认值");
            return false;
        }
        try
        {
            //读取json文件
            string encryptedJson = File.ReadAllText(savePath);
            string json = EncryptionUtility.Decrypt(encryptedJson);
            //反序列化json文件中的data
            NormalNpcDatas data = JsonConvert.DeserializeObject<NormalNpcDatas>(json);
            for (int i = 0; i < data.data.Count; i++)
            {
                NpcDataEntry npc = data.data[i];
                m_npc.npcDictionary[npc.npcName].favorability = npc.favorability;
                m_npc.npcDictionary[npc.npcName].isSelected = npc.isSelected;
                m_npc.npcDictionary[npc.npcName].selectedTimes = npc.selectedTimes;
            }
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"读取失败: {e.Message}");
            return false;
        }
    }
    public void SaveRound()
    {
        var saveData = new RoundData
        {
            currendRound = m_round.currentRound,
            maxSelectionsPerRound = m_round.maxSelectionsPerRound,
            selectionsPerRound = m_round.selectionsPerRound
        };
        string json = JsonConvert.SerializeObject(saveData, Formatting.Indented);
        string savePath = Path.Combine(Application.persistentDataPath, "round_save.json");
        string encryptedJson = EncryptionUtility.Encrypt(json);
        File.WriteAllText(savePath, encryptedJson);
        Debug.Log("回合信息已保存");
    }
    public bool LoadRound()
    {
        m_round.currentRound = 0;
        m_round.maxSelectionsPerRound = 0;
        m_round.selectionsPerRound = 0;
        string savePath = Path.Combine(Application.persistentDataPath, "round_save.json");
        //无存档
        if (!File.Exists(savePath))
        {
            Debug.Log("无存档文件，返回默认值");
            return false;
        }
        try
        {
            //读取json文件
            string encryptedJson = File.ReadAllText(savePath);
            string json = EncryptionUtility.Decrypt(encryptedJson);
            //反序列化json文件中的data
            RoundData data = JsonConvert.DeserializeObject<RoundData>(json);
            m_round.currentRound = data.currendRound;
            m_round.selectionsPerRound = data.selectionsPerRound;
            m_round.maxSelectionsPerRound = data.maxSelectionsPerRound;
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"读取失败: {e.Message}");
            return false;
        }
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
        string encryptedJson = EncryptionUtility.Encrypt(json);
        File.WriteAllText(savePath, encryptedJson);
        Debug.Log("库存已保存");
    }
    public bool LoadInventory()
    {
        string savePath = Path.Combine(Application.persistentDataPath, "inventory_save.json");
        if (!File.Exists(savePath)) return false;

        try
        {
            string encryptedJson = File.ReadAllText(savePath);
            string json = EncryptionUtility.Decrypt(encryptedJson);
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
        foreach (var item in m_gameManager.wholeItemDataList)
        {
            if (item.itemID == itemId)
            {
                return item;
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

        string encryptedJson = EncryptionUtility.Encrypt(json);
        File.WriteAllText(savePath, encryptedJson);
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
            string encryptedJson = File.ReadAllText(savePath);
            string json = EncryptionUtility.Decrypt(encryptedJson);
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