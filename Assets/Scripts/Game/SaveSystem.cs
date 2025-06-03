using Newtonsoft.Json;
using System.IO;
using UnityEngine;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;


[Serializable]
public class PlayerData
{
    public int gold;
}


public class InventorySaveData
{
    public List<ItemSaveEntry> items = new List<ItemSaveEntry>();
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
        Save();
    }
    public void GameLoad()
    {
        Load();
    }
    public void Save()
    {
        SaveGlobalData();
    }
    public void Load()
    {
        LoadGlobalData();
    }
    public void SaveGlobalData()
    {
        GameData gameData = new GameData { currentRoundData = new RoundData(), lastRoundData = new RoundData() };
        SaveCurrentRoundData(gameData.currentRoundData);
        SaveLastRoundData(gameData.lastRoundData);


        string json = JsonConvert.SerializeObject(gameData, Formatting.Indented);
        string savePath = Path.Combine(Application.persistentDataPath, "global_save.json");
        string encryptedJson = EncryptionUtility.Encrypt(json);

        File.WriteAllText(savePath, encryptedJson);
        Debug.Log("全局存储成功!");
    }
    public bool LoadGlobalData()
    {
        string savePath = Path.Combine(Application.persistentDataPath, "global_save.json");
        if (!File.Exists(savePath)) return false;
        try
        {
            string encryptedJson = File.ReadAllText(savePath);
            string json = EncryptionUtility.Decrypt(encryptedJson);
            GameData saveData = JsonConvert.DeserializeObject<GameData>(json);
            m_gameManager.ClearDatasBeforeLoading();

            LoadCurrentRoundData(saveData.currentRoundData);
            LoadLastRoundData(saveData.lastRoundData);
            Debug.Log("读取存档成功!");

            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"读取存档失败: {e.Message}");
            return false;
        }
    }
    #region Save具体操作
    public void SaveCurrentRoundData(RoundData roundData)
    {
        roundData.currentRound = m_round.currentRound;
        roundData.gold = m_player.gold;
        roundData.maxSelectionsPerRound = m_round.maxSelectionsPerRound;
        roundData.selectionsPerRound = m_round.selectionsPerRound;


        List<NpcDataEntry> normalNpcDatas = new List<NpcDataEntry>();
        List<NpcDataEntry> storyNpcDatas = new List<NpcDataEntry>();
        List<ItemSaveEntry> itemSaveEntries = new List<ItemSaveEntry>();
        SaveCurrentNpcData(normalNpcDatas, storyNpcDatas);
        SaveCurrentInventoryData(itemSaveEntries);


        roundData.storyNpcData = storyNpcDatas;
        roundData.normalNpcData = normalNpcDatas;
        roundData.items = itemSaveEntries;


    }
    public void SaveLastRoundData(RoundData roundData)
    {
        roundData.currentRound = m_round.lastRound;
        roundData.maxSelectionsPerRound = m_round.lastRoundMaxSelectionsPerRound;
        roundData.selectionsPerRound = m_round.lastRoundSelectionsPerRound;
        roundData.gold = m_player.lastRoundGold;


        List<NpcDataEntry> normalNpcDatas = new List<NpcDataEntry>();
        List<NpcDataEntry> storyNpcDatas = new List<NpcDataEntry>();
        List<ItemSaveEntry> itemSaveEntries = new List<ItemSaveEntry>();
        SaveLastNpcData(normalNpcDatas, storyNpcDatas);
        SaveLastInventoryData(itemSaveEntries);


        roundData.storyNpcData = storyNpcDatas;
        roundData.normalNpcData = normalNpcDatas;
        roundData.items = itemSaveEntries;
    }

    public void SaveCurrentInventoryData(List<ItemSaveEntry> itemSaveEntry)
    {

        foreach (Item item in m_inventory.items)
        {
            itemSaveEntry.Add(new ItemSaveEntry
            {
                itemId = item.data.itemID, // 假设 ItemData 有唯一 itemID
                amount = item.amount
            });
        }
    }
    public void SaveLastInventoryData(List<ItemSaveEntry> itemSaveEntry)
    {
        foreach (Item item in m_inventory.lastRoundItems)
        {
            itemSaveEntry.Add(new ItemSaveEntry
            {
                itemId = item.data.itemID, // 假设 ItemData 有唯一 itemID
                amount = item.amount
            });
        }
    }



    public void SaveCurrentNpcData(List<NpcDataEntry> normalNpcData, List<NpcDataEntry> storyNpcData)
    {
        foreach (var data in m_gameManager.wholeNormalNpcDataList)
        {
            Npc npc = m_npc.npcDictionary[data.name];
            NpcDataEntry npcDataEntry = new NpcDataEntry
            {
                favorability = npc.favorability,
                npcName = npc.data.name,
                selectedTimes = npc.selectedTimes,
                isSelected = npc.isSelected,
                bit_0 = npc.bit_0,
                bit_1 = npc.bit_1,
                bit_2 = npc.bit_2,
                branchNumber = npc.branchNumber
            };
            normalNpcData.Add(npcDataEntry);
        }
        foreach (var data in m_gameManager.wholeStoryNpcDataList)
        {
            Npc npc = m_npc.npcDictionary[data.name];
            NpcDataEntry npcDataEntry = new NpcDataEntry
            {
                favorability = npc.favorability,
                npcName = npc.data.name,
                selectedTimes = npc.selectedTimes,
                isSelected = npc.isSelected,
                bit_0 = npc.bit_0,
                bit_1 = npc.bit_1,
                bit_2 = npc.bit_2,
                branchNumber = npc.branchNumber
            };
            storyNpcData.Add(npcDataEntry);
        }
    }
    public void SaveLastNpcData(List<NpcDataEntry> normalNpcData, List<NpcDataEntry> storyNpcData)
    {
        foreach (var data in m_gameManager.wholeNormalNpcDataList)
        {
            Npc npc = m_npc.lastRoundNpcDictionary[data.name];
            NpcDataEntry npcDataEntry = new NpcDataEntry
            {
                favorability = npc.favorability,
                npcName = npc.data.name,
                selectedTimes = npc.selectedTimes,
                isSelected = npc.isSelected,
                bit_0 = npc.bit_0,
                bit_1 = npc.bit_1,
                bit_2 = npc.bit_2,
                branchNumber = npc.branchNumber
            };
            normalNpcData.Add(npcDataEntry);
        }
        foreach (var data in m_gameManager.wholeStoryNpcDataList)
        {
            Npc npc = m_npc.lastRoundNpcDictionary[data.name];
            NpcDataEntry npcDataEntry = new NpcDataEntry
            {
                favorability = npc.favorability,
                npcName = npc.data.name,
                selectedTimes = npc.selectedTimes,
                isSelected = npc.isSelected,
                bit_0 = npc.bit_0,
                bit_1 = npc.bit_1,
                bit_2 = npc.bit_2,
                branchNumber = npc.branchNumber
            };
            storyNpcData.Add(npcDataEntry);
        }
    }
    #endregion
    public void LoadCurrentRoundData(RoundData data)
    {
        m_player.gold = data.gold;
        m_round.currentRound = data.currentRound;
        m_round.maxSelectionsPerRound = data.maxSelectionsPerRound;
        m_round.selectionsPerRound = data.selectionsPerRound;
        LoadCurrentNpcData(data.normalNpcData, data.storyNpcData);
        LoadCurrentInventory(data.items);

    }
    public void LoadLastRoundData(RoundData data)
    {
        m_player.lastRoundGold = data.gold;
        m_round.lastRound = data.currentRound;
        m_round.lastRoundMaxSelectionsPerRound = data.maxSelectionsPerRound;
        m_round.lastRoundSelectionsPerRound = data.selectionsPerRound;
        LoadLastNpcData(data.normalNpcData, data.storyNpcData);
        LoadLastInventory(data.items);
    }
    public void LoadCurrentNpcData(List<NpcDataEntry> normalNpc, List<NpcDataEntry> storyNpc)
    {
        for (int i = 0; i < normalNpc.Count; i++)
        {
            NpcDataEntry npc = normalNpc[i];
            LoadCurrentNpcDatas(npc);
        }
        for (int i = 0; i < storyNpc.Count; i++)
        {
            NpcDataEntry npc = normalNpc[i];
            LoadCurrentNpcDatas(npc);
        }
    }
    public void LoadLastNpcData(List<NpcDataEntry> normalNpc, List<NpcDataEntry> storyNpc)
    {
        for (int i = 0; i < normalNpc.Count; i++)
        {
            NpcDataEntry npc = normalNpc[i];
            LoadLastNpcDatas(npc);
        }
        for (int i = 0; i < storyNpc.Count; i++)
        {
            NpcDataEntry npc = normalNpc[i];
            LoadLastNpcDatas(npc);
        }
    }
    public void LoadCurrentNpcDatas(NpcDataEntry npc)
    {
        m_npc.npcDictionary[npc.npcName].favorability = npc.favorability;
        m_npc.npcDictionary[npc.npcName].isSelected = npc.isSelected;
        m_npc.npcDictionary[npc.npcName].selectedTimes = npc.selectedTimes;
        m_npc.npcDictionary[npc.npcName].bit_0 = npc.bit_0;
        m_npc.npcDictionary[npc.npcName].bit_1 = npc.bit_1;
        m_npc.npcDictionary[npc.npcName].bit_2 = npc.bit_2;
        m_npc.npcDictionary[npc.npcName].branchNumber = npc.branchNumber;
    }
    public void LoadLastNpcDatas(NpcDataEntry npc)
    {
        m_npc.lastRoundNpcDictionary[npc.npcName].favorability = npc.favorability;
        m_npc.lastRoundNpcDictionary[npc.npcName].isSelected = npc.isSelected;
        m_npc.lastRoundNpcDictionary[npc.npcName].selectedTimes = npc.selectedTimes;
        m_npc.lastRoundNpcDictionary[npc.npcName].bit_0 = npc.bit_0;
        m_npc.lastRoundNpcDictionary[npc.npcName].bit_1 = npc.bit_1;
        m_npc.lastRoundNpcDictionary[npc.npcName].bit_2 = npc.bit_2;
        m_npc.lastRoundNpcDictionary[npc.npcName].branchNumber = npc.branchNumber;
    }


    public void LoadCurrentInventory(List<ItemSaveEntry> items)
    {
        foreach (ItemSaveEntry entry in items)
        {
            ItemData itemData = FindItemDataById(entry.itemId);
            m_inventory.AddItem(itemData, entry.amount);
        }
    }
    public void LoadLastInventory(List<ItemSaveEntry> items)
    {
        foreach (ItemSaveEntry entry in items)
        {
            ItemData itemData = FindItemDataById(entry.itemId);
            m_inventory.AddItemToLast(itemData, entry.amount);
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



}