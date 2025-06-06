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


public static class SaveSystem
{
    private static PlayerController m_player => PlayerController.Instance;
    private static NpcManager m_npc => NpcManager.Instance;
    private static InventorySystem m_inventory => InventorySystem.Instance;
    private static RoundManager m_round => RoundManager.Instance;
    private static SceneLoader m_sceneLoader => SceneLoader.Instance;
    private static TicketManager m_ticket => TicketManager.Instance;
    private static GameManager m_gameManager => GameManager.Instance;

    public static void GameSave()
    {
        Save();
    }
    public static void GameLoad()
    {
        Load();
    }
    public static void InitSave()
    {
        ItSave();
    }
    public static bool InitLoad()
    {
        return ItLoad();
    }
    public static void Save()
    {
        SaveGlobalData();
    }
    public static void Load()
    {
        LoadGlobalData();
    }
    public static void ItSave()
    {
        GameData gameData = new GameData { currentRoundData = new RoundData(), lastRoundData = new RoundData() };
        SaveCurrentRoundData(gameData.currentRoundData);
        SaveLastRoundData(gameData.lastRoundData);


        string json = JsonConvert.SerializeObject(gameData, Formatting.Indented);
        string savePath = Path.Combine(Application.persistentDataPath, "Init_save.json");
        string encryptedJson = EncryptionUtility.Encrypt(json);

        File.WriteAllText(savePath, encryptedJson);
        Debug.Log("全局存储成功!");
    }
    public static bool ItLoad()
    {
        string savePath = Path.Combine(Application.persistentDataPath, "Init_save.json");
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
    public static void SaveGlobalData()
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
    public static bool LoadGlobalData()
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
    public static void SaveCurrentRoundData(RoundData roundData)
    {
        roundData.currentRound = m_round.currentRound;
        roundData.gold = m_player.gold;
        roundData.maxSelectionsPerRound = m_round.maxSelectionsPerRound;
        roundData.selectionsPerRound = m_round.selectionsPerRound;
        roundData.dateTime = DateExtensions.BaseDate;
        roundData.trainCountRank = m_player.lastTrainCountRank;
        roundData.cutomerCountRank = m_player.lastCustomerCountRank;
        foreach (var data in GameManager.Instance.currentLoseNpcList)
        {
            roundData.loseNpc.Add(data);
        }

        List<NpcDataEntry> normalNpcDatas = new List<NpcDataEntry>();
        List<NpcDataEntry> storyNpcDatas = new List<NpcDataEntry>();
        List<ItemSaveEntry> itemSaveEntries = new List<ItemSaveEntry>();
        SaveCurrentNpcData(normalNpcDatas, storyNpcDatas);
        SaveCurrentInventoryData(itemSaveEntries);


        roundData.storyNpcData = storyNpcDatas;
        roundData.normalNpcData = normalNpcDatas;
        roundData.items = itemSaveEntries;


    }
    public static void SaveLastRoundData(RoundData roundData)
    {
        roundData.currentRound = m_round.lastRound;
        roundData.maxSelectionsPerRound = m_round.lastRoundMaxSelectionsPerRound;
        roundData.selectionsPerRound = m_round.lastRoundSelectionsPerRound;
        roundData.gold = m_player.lastRoundGold;
        roundData.trainCountRank = m_player.trainCountRank;
        roundData.cutomerCountRank = m_player.customerCountRank;


        List<NpcDataEntry> normalNpcDatas = new List<NpcDataEntry>();
        List<NpcDataEntry> storyNpcDatas = new List<NpcDataEntry>();
        List<ItemSaveEntry> itemSaveEntries = new List<ItemSaveEntry>();
        SaveLastNpcData(normalNpcDatas, storyNpcDatas);
        SaveLastInventoryData(itemSaveEntries);


        roundData.storyNpcData = storyNpcDatas;
        roundData.normalNpcData = normalNpcDatas;
        roundData.items = itemSaveEntries;
    }

    public static void SaveCurrentInventoryData(List<ItemSaveEntry> itemSaveEntry)
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
    public static void SaveLastInventoryData(List<ItemSaveEntry> itemSaveEntry)
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



    public static void SaveCurrentNpcData(List<NpcDataEntry> normalNpcData, List<NpcDataEntry> storyNpcData)
    {
        foreach (var data in m_gameManager.wholeNormalNpcDataList)
        {
            Npc npc = m_npc.npcDictionary[data.npcName];
            NpcDataEntry npcDataEntry = new NpcDataEntry
            {
                favorability = npc.favorability,
                npcName = npc.data.npcName,
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
            Npc npc = m_npc.npcDictionary[data.npcName];
            NpcDataEntry npcDataEntry = new NpcDataEntry
            {
                favorability = npc.favorability,
                npcName = npc.data.npcName,
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
    public static void SaveLastNpcData(List<NpcDataEntry> normalNpcData, List<NpcDataEntry> storyNpcData)
    {
        foreach (var data in m_gameManager.wholeNormalNpcDataList)
        {
            Npc npc = m_npc.lastRoundNpcDictionary[data.npcName];
            NpcDataEntry npcDataEntry = new NpcDataEntry
            {
                favorability = npc.favorability,
                npcName = npc.data.npcName,
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
            Npc npc = m_npc.lastRoundNpcDictionary[data.npcName];
            NpcDataEntry npcDataEntry = new NpcDataEntry
            {
                favorability = npc.favorability,
                npcName = npc.data.npcName,
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
    public static void LoadCurrentRoundData(RoundData data)
    {
        m_player.gold = data.gold;
        DateExtensions.ToGameDate(data.currentRound - m_round.currentRound);
        m_round.currentRound = data.currentRound;
        m_round.maxSelectionsPerRound = data.maxSelectionsPerRound;
        m_round.selectionsPerRound = data.selectionsPerRound;
        DateExtensions.BaseDate = data.dateTime;
        m_player.customerCountRank = data.cutomerCountRank;
        m_player.trainCountRank = data.trainCountRank;
        foreach (var dt in data.loseNpc)
        {
            GameManager.Instance.currentLoseNpcList.Add(dt);
        }
        LoadCurrentNpcData(data.normalNpcData, data.storyNpcData);
        LoadCurrentInventory(data.items);

    }
    public static void LoadLastRoundData(RoundData data)
    {
        m_player.lastRoundGold = data.gold;
        DateExtensions.ToGameDate(data.currentRound - m_round.currentRound - 1);
        m_round.lastRound = data.currentRound;
        m_round.lastRoundMaxSelectionsPerRound = data.maxSelectionsPerRound;
        m_round.lastRoundSelectionsPerRound = data.selectionsPerRound;
        m_player.lastCustomerCountRank = data.cutomerCountRank;
        m_player.trainCountRank = data.trainCountRank;
        LoadLastNpcData(data.normalNpcData, data.storyNpcData);
        LoadLastInventory(data.items);
    }
    public static void LoadCurrentNpcData(List<NpcDataEntry> normalNpc, List<NpcDataEntry> storyNpc)
    {
        for (int i = 0; i < normalNpc.Count; i++)
        {
            NpcDataEntry npc = normalNpc[i];
            LoadCurrentNpcDatas(npc);
        }
        for (int i = 0; i < storyNpc.Count; i++)
        {
            NpcDataEntry npc = storyNpc[i];
            LoadCurrentNpcDatas(npc);
        }
    }
    public static void LoadLastNpcData(List<NpcDataEntry> normalNpc, List<NpcDataEntry> storyNpc)
    {
        for (int i = 0; i < normalNpc.Count; i++)
        {
            NpcDataEntry npc = normalNpc[i];
            LoadLastNpcDatas(npc);
        }
        for (int i = 0; i < storyNpc.Count; i++)
        {
            NpcDataEntry npc = storyNpc[i];
            LoadLastNpcDatas(npc);
        }
    }
    public static void LoadCurrentNpcDatas(NpcDataEntry npc)
    {
        m_npc.npcDictionary[npc.npcName].favorability = npc.favorability;
        m_npc.npcDictionary[npc.npcName].isSelected = npc.isSelected;
        m_npc.npcDictionary[npc.npcName].selectedTimes = npc.selectedTimes;
        m_npc.npcDictionary[npc.npcName].bit_0 = npc.bit_0;
        m_npc.npcDictionary[npc.npcName].bit_1 = npc.bit_1;
        m_npc.npcDictionary[npc.npcName].bit_2 = npc.bit_2;
        m_npc.npcDictionary[npc.npcName].branchNumber = npc.branchNumber;
    }
    public static void LoadLastNpcDatas(NpcDataEntry npc)
    {
        m_npc.lastRoundNpcDictionary[npc.npcName].favorability = npc.favorability;
        m_npc.lastRoundNpcDictionary[npc.npcName].isSelected = npc.isSelected;
        m_npc.lastRoundNpcDictionary[npc.npcName].selectedTimes = npc.selectedTimes;
        m_npc.lastRoundNpcDictionary[npc.npcName].bit_0 = npc.bit_0;
        m_npc.lastRoundNpcDictionary[npc.npcName].bit_1 = npc.bit_1;
        m_npc.lastRoundNpcDictionary[npc.npcName].bit_2 = npc.bit_2;
        m_npc.lastRoundNpcDictionary[npc.npcName].branchNumber = npc.branchNumber;
    }


    public static void LoadCurrentInventory(List<ItemSaveEntry> items)
    {
        foreach (ItemSaveEntry entry in items)
        {
            ItemData itemData = FindItemDataById(entry.itemId);
            m_inventory.AddItem(itemData, entry.amount);
        }
    }
    public static void LoadLastInventory(List<ItemSaveEntry> items)
    {
        foreach (ItemSaveEntry entry in items)
        {
            ItemData itemData = FindItemDataById(entry.itemId);
            m_inventory.AddItemToLast(itemData, entry.amount);
        }
    }
    private static ItemData FindItemDataById(string itemId)
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

    public static bool HadSaveFile()
    {
        string savePath = Path.Combine(Application.persistentDataPath, "global_save.json");
        if (!File.Exists(savePath)) return false;
        else return true;
    }
    public static bool HadInitSaveFile()
    {
        string savePath = Path.Combine(Application.persistentDataPath, "Init_save.json");
        if (!File.Exists(savePath)) return false;
        else return true;
    }

}