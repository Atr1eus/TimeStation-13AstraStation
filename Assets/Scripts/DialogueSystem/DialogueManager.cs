using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueSystem;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; set; }

    Dialogue dialogueManager;
    List<DialogueGameState> gameStateVariables = new List<DialogueGameState>();
    public bool isTalking;

    [SerializeField] private NpcManager npcmanager;

    [Header("对话设置")]
    public DialogueGraph m_graph;
    //public GameObject Pane;
    //public GameObject Text;

    [Header("输入控制")]
    public KeyCode advanceKey = KeyCode.Space;
    public KeyCode speedUpKey = KeyCode.T;
    public KeyCode themeTestKey = KeyCode.S;
    public KeyCode pauseKey = KeyCode.P;

    [Header("UI")]
    public DialogueUIHandler UiHandler;
    public DialogueTheme alternativeTheme;

    [Header("事件处理")]
    public DialogueEventList eventHandler;

    protected void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        isTalking = false;
    }



    private void Start()
    {
        npcmanager = NpcManager.Instance;
        dialogueManager = Dialogue.instance;

        dialogueManager.dialogueCallbackActions.OnNodeLeave += OnNodeLeave;
        dialogueManager.dialogueCallbackActions.OnNodeEnter += OnNodeEnter;

        dialogueManager.SetDialogGameState(gameStateVariables);

        dialogueManager.OnChoiceDraw += OnChoiceDraw;

        dialogueManager.RegisterEventHandler(EventHandler);
    }

    private void Update()
    {
        HandleInput();
    }

    public void StartDialogue()
    {
        if (dialogueManager == null)
        {
            Debug.Log("Nodialogue!");
        }
        if (dialogueManager.dialoguePane == null && dialogueManager.dialogueTextGameObject == null)
        {
            Debug.Log("Re-register!");
            dialogueManager.dialoguePane = UiHandler.Pane;
            dialogueManager.dialogueTextGameObject = UiHandler.Text;
        }
        isTalking = true;
        GameStateHandler();// 获取现在npc的数据，决定走那个对话
    }

    //输入处理
    private void HandleInput()
    {
        // 对话推进控制
        if (Input.GetKeyDown(advanceKey))
        {

            if (!dialogueManager.IsRunning && isTalking)
            {
                isTalking = false;
                if (!m_graph)
                {
                    Debug.LogWarning("NO GRAPH!!!");
                }
                Debug.Log("开始对话");
                dialogueManager.StartDialogue(m_graph); // 开始对话
            }
            else if (dialogueManager.IsRunning)
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

    public void HandleSpaceKeyPress()
    {
        if (!dialogueManager.IsRunning && isTalking)
        {
            isTalking = false;
            if (!m_graph)
            {
                Debug.LogWarning("NO GRAPH!!!");
            }
            Debug.Log("开始对话");
            dialogueManager.StartDialogue(m_graph); // 开始对话
        }
        else if (dialogueManager.IsRunning)
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
        }
    }
    public void SetGraph(DialogueGraph graph)
    {
        Debug.Log("对画图设置成功！");
        m_graph = graph;
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
        Debug.Log("获取角色数据用于对话分支");

        if (npcmanager == null)
        {
            npcmanager = NpcManager.Instance;
            //Debug.LogWarning("no npcmanager!");
        }
        if (npcmanager.currentNpc == null)
        {
            Debug.LogWarning("no npccontroller!");
        }
        if (npcmanager.currentNpc.npc == null)
        {
            Debug.LogWarning("no npc!");
        }
        Npc curNpc = npcmanager.currentNpc.npc;
        gameStateVariables.Add(new DialogueGameState(curNpc.favorability, "Favorability"));
        gameStateVariables.Add(new DialogueGameState(curNpc.bit_0, "bit_0"));
        gameStateVariables.Add(new DialogueGameState(curNpc.bit_1, "bit_1"));
        gameStateVariables.Add(new DialogueGameState(curNpc.bit_2, "bit_2"));
        Debug.Log("已获取");
    }
}
