using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueSystem;

public class DialogueManager : MonoBehaviour
{
    Dialogue dialogueManager;
    List<DialogueGameState> gameStateVariables = new List<DialogueGameState>();
    private Npc curNpc;

    [Header("对话设置")]
    public DialogueGraph graph;
    public DialogueTheme alternativeTheme;

    [Header("输入控制")]
    public KeyCode advanceKey = KeyCode.Space;
    public KeyCode speedUpKey = KeyCode.T;
    public KeyCode themeTestKey = KeyCode.S;
    public KeyCode pauseKey = KeyCode.P;

    [Header("UIHandler")]
    public DialogueUIHandler UiHandler;

    [Header("事件处理器")]
    public DialogueEventList eventHandler;

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        //curNpc = GetComponent<Npc>();

        if (curNpc == null)
        {
            Debug.LogWarning($"DialogueManager所在的GameObject缺少NPC组件", gameObject);
        }
    }

    private void Start()
    {
        dialogueManager = Dialogue.instance;

        dialogueManager.dialogueCallbackActions.OnNodeLeave += OnNodeLeave;
        dialogueManager.dialogueCallbackActions.OnNodeEnter += OnNodeEnter;

        //GameStateHandler();
        dialogueManager.SetDialogGameState(gameStateVariables);

        dialogueManager.OnChoiceDraw += OnChoiceDraw;

        dialogueManager.RegisterEventHandler(EventHandler);
    }

    private void Update()
    {
        HandleInput();
    }

    //输入处理
    private void HandleInput()
    {
        // 对话推进控制
        if (Input.GetKeyDown(advanceKey))
        {
            if (dialogueManager.IsRunning)
            {
                if (dialogueManager.isAnimating)
                {
                    dialogueManager.EndLine(); // 跳过文本动画
                }
                else if (dialogueManager.CurrentState != DialogueState.AwaitingEventResponse)
                {
                    dialogueManager.AdvanceDialogue(); // 推进对话
                }
            }
            else
            {
                dialogueManager.StartDialogue(); // 开始对话
            }
        }

        // 文本加速显示
        if (Input.GetKeyDown(speedUpKey))
        {
            dialogueManager.ToggleSpeedUp(!dialogueManager.speedUp);
        }

        // 主题测试
        if (Input.GetKeyDown(themeTestKey) && alternativeTheme != null)
        {
            //alternativeTheme.ChangeColor("red", Color.green);
        }

        // 暂停/恢复对话
        if (Input.GetKeyDown(pauseKey))
        {
            if (dialogueManager.CurrentState == DialogueState.Paused)
            {
                dialogueManager.Pause(false);
            }
            else
            {
                dialogueManager.Pause(true);
            }
        }
    }

    // 文本节点开始时的回调
    public void OnLineStartCallback(TextNode node)
    {
        // TODO

    }

    // 文本节点结束时的回调
    public void OnLineEndCallback(TextNode node)
    {
        UiHandler.DisableNamePlate();
    }

    // 结点进入时的回调
    public void OnNodeEnter(BaseNode node)
    {
        //Debug.Log("Entered " + node.name);
    }

    // 结点离开时的回调
    public void OnNodeLeave(BaseNode node)
    {
        //Debug.Log("left " + node.name);
        //GameStateHandler();//实时更新角色数据（例如好感度）
    }

    //绘制选项
    public bool OnChoiceDraw(DialogueChoices node)
    {
        UiHandler.RenderDialogueChoices(node, dialogueManager);
        return true;
    }

    //事件处理
    public void EventHandler(DialogueEvent myEvent)
    {
        if (eventHandler != null && myEvent != null)
        {
            eventHandler.EventHandler(myEvent);
        }
    }

    //游戏状态处理（获取角色属性判断是哪组对话）
    public void GameStateHandler()
    {
        gameStateVariables.Add(new DialogueGameState(curNpc.attribute[NpcAttribute.Favorability], "Favorability")); //希望改写成通用接口
    }
}
