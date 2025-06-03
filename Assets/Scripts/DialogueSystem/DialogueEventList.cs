using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DialogueSystem;

public class DialogueEventList : MonoBehaviour
{
    [SerializeField] private TradingSceneManager scene;
    [SerializeField] private NpcManager npcManager;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Npc FindNpcByName(string npcName)
    {
        // 检查字典是否初始化且包含该键
        if (npcManager.npcDictionary != null &&
            npcManager.npcDictionary.ContainsKey(npcName))
        {
            return npcManager.npcDictionary[npcName];
        }

        Debug.LogWarning($"NPC '{npcName}' not found in dictionary!");
        return null;
    }

    public void EventHandler(DialogueEvent myEvent)
    {
        if (myEvent == null) return;

        // 在此处添加您的各种事件处理逻辑
        switch (myEvent.eventName)
        {
            case "SSVGG": //测试事件
                SSVGGEvent();
                break;

            case "EndDialogue": // 对话结束，开始选择
                scene.DecidedNpcButtonState();
                break;

            case "ChangeDialogueNumber": // 更改对话组
                ChangeDialogueNumber(myEvent.intParameter, myEvent.stringParameter);
                break;

            case "SetFavoribility": // 设置好感度
                SetFavorability(myEvent.intParameter);
                break;

            case "AddGold":
                AddGold(myEvent.intParameter);
                break;

            default:
                Debug.LogWarning($"Unknown event: {myEvent.eventName}");
                break;
        }
    }

    public void SSVGGEvent()
    {
        Debug.Log("SSVGG");
    }


    public void ChangeDialogueNumber(int num, string npcName)
    {
        Npc npc = FindNpcByName(npcName);
        switch (num)
        {
            case 0:
                npc.bit_0 = 0;
                npc.bit_1 = 0;
                npc.bit_2 = 0;
                break;
            case 1:
                npc.bit_0 = 1;
                npc.bit_1 = 0;
                npc.bit_2 = 0;
                break;
            case 2:
                npc.bit_0 = 0;
                npc.bit_1 = 1;
                npc.bit_2 = 0;
                break;
            case 3:
                npc.bit_0 = 1;
                npc.bit_1 = 1;
                npc.bit_2 = 0;
                break;
            case 4:
                npc.bit_0 = 0;
                npc.bit_1 = 0;
                npc.bit_2 = 1;
                break;
            case 5:
                npc.bit_0 = 1;
                npc.bit_1 = 0;
                npc.bit_2 = 1;
                break;
            case 6:
                npc.bit_0 = 0;
                npc.bit_1 = 1;
                npc.bit_2 = 1;
                break;
            case 7:
                npc.bit_0 = 1;
                npc.bit_1 = 1;
                npc.bit_2 = 1;
                break;
            default:
                Debug.LogWarning($"Unknown dialogueStream: {npc.branchNumber}");
                break;

        }
    }

    public void SetFavorability(int num)
    {
        NpcManager.Instance.SetFavorability(num);
    }

    public void AddGold(int num)
    {
        PlayerController.Instance.AddGold(num);
    }
}
