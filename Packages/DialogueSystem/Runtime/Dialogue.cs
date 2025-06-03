using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;
using XNode;

namespace DialogueSystem
{
    public class Dialogue : MonoBehaviour, INotificationReceiver
    {
        public static Dialogue instance { get; set; }

        /* Private members */
        private DialogueHandler _handler;

        /* Public members */
        #region Public members

        [Header("Graph")]
        public DialogueGraph dialogueGraph;

        [Header("UI")]
        public GameObject dialoguePane;
        public GameObject dialogueTextGameObject;

        [Header("Theme")]
        [SerializeField]
        public DialogueTheme theme;

        [Header("Settings")]
        [SerializeField]
        public DialogueSettings settings;
        public DialogueDictionary dictionary;
        #endregion

        #region Custom functions
        public Func<DialogueConditional, bool> CustomTestCondition
        {

            set
            {
                _handler.CustomTestCondition = value;
            }
        }

        public Func<DialogueChoices, bool> OnChoiceDraw { get { return _handler.OnChoiceDraw; } set { _handler.OnChoiceDraw += value; } }

        public Func<DialogueSignalEmitter, bool> StartDialogueFromSignal = null;
        #endregion

        #region Callback functions
        public DialogueCallbackActions dialogueCallbackActions;
        #endregion

        public List<GameEventFlag> flags
        {
            get
            {
                return _handler.gameEventFlags;
            }

            set
            {
                _handler.gameEventFlags = value;
            }
        }

        public List<DialogueGameState> gameStateVariables
        {
            get
            {
                return _handler.gameStateVariables;
            }

            private set
            {
                _handler.gameStateVariables = value;
            }
        }

        #region Readonly members
        public bool IsRunning { get { return _handler.IsRunning; } }
        public bool isAnimating { get { return Ui.isAnimating; } }
        public bool speedUp { get { return Ui.textEffects.speedUp; } }
        public Playable currentPlayable { get; private set; }
        public BaseNode CurrentNode
        {
            get
            {
                return _handler.CurrentNode;
            }

        }
        public BranchNode CurrentBranch
        {
            get
            {
                return _handler.CurrentBranch;
            }

        }
        public DialogueGraph CurrentGraph
        {
            get
            {
                return _handler.CurrentGraph;
            }

        }
        public DialogueUI Ui { get; private set; }
        public DialogueState CurrentState { get { return _handler.CurrentState; } }
        #endregion

        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                instance = this;
            }
            
            if (theme == null)
            {
                theme = ScriptableObject.CreateInstance<DialogueTheme>();
            }

            if (settings == null)
            {
                settings = ScriptableObject.CreateInstance<DialogueSettings>();
            }

            Ui = new DialogueUI(dialogueTextGameObject, dialoguePane, theme, settings, dialogueCallbackActions);

            if (settings != null)
            {
                if (settings.HideDialoguePaneOnStart) Ui.ShowDialoguePane(false);
            }

            if (_handler == null)
            {
                _handler = new DialogueHandler(Ui, settings, dialogueCallbackActions, dictionary, this);
            }

        }

        public void SetTheme(DialogueTheme theme)
        {
            Ui.SetTheme(theme);
        }

        public void SetCallbackActions(DialogueCallbackActions callbackActions)
        {
            this.dialogueCallbackActions = callbackActions;
            Ui.SetCallbackActions(callbackActions);

        }
        public void RegisterEventHandler(Action<DialogueEvent> eventHandler)
        {
            dialogueCallbackActions.EventHandler += eventHandler;
        }

        public void DeregisterEventHandler(Action<DialogueEvent> eventHandler)
        {
            dialogueCallbackActions.EventHandler -= eventHandler;
        }

        public void SetFlags(List<GameEventFlag> flags)
        {
            _handler.gameEventFlags = flags;
        }

        public void SetDialogGameState(List<DialogueGameState> gameState)
        {
            this.gameStateVariables = gameState;
        }

        public void StartDialogue()
        {
            if (IsRunning)
            {
                if (settings.showWarnings) Debug.LogWarning("The dialogue system is already running.");
                return;
            }

            if (dialogueGraph == null)
            {
                Debug.LogError("No dialogue graph is set.");
            }
            _handler.ExecuteGraph(dialogueGraph);

        }

        public void StartDialogue(DialogueGraph graph)
        {
            if (IsRunning)
            {
                if (settings.showWarnings) Debug.LogWarning("The dialogue system is already running.");
                return;
            }

            this.dialogueGraph = graph;
            _handler.ExecuteGraph(dialogueGraph);

        }

        #region Typewriter Interactions
        public void ToggleSpeedUp(bool toggle)
        {
            if (CurrentState == DialogueState.Paused)
            {
                Debug.LogWarning("The dialogue system is currently paused.");
                return;
            }
            Ui.textEffects.SpeedUp(toggle);
        }

        public void EndLine()
        {
            if (CurrentState == DialogueState.Paused)
            {
                Debug.LogWarning("The dialogue system is currently paused.");
                return;
            }
            if (CurrentState == DialogueState.Animating)
            {
                Ui.textEffects.DisplayWholeText();
            }

        }
        #endregion

        public void AdvanceDialogue()
        {
            if (CurrentState == DialogueState.Paused)
            {
                if (settings.showWarnings) Debug.LogWarning("The dialogue system is currently paused.");
                return;
            }

            if (CurrentState == DialogueState.AwaitingChoice)
            {
                if (settings.showWarnings) Debug.LogWarning("Dialogue is currently awaiting a dialogue choice. Use SelectDialogueChoice to advance the dialogue.");
                return;
            }

            if (CurrentState == DialogueState.Waiting)
            {
                if (settings.showWarnings) Debug.LogWarning("Dialogue system is currently waiting.");
                return;
            }

            if (CurrentState == DialogueState.NotRunning)
            {
                if (settings.showWarnings) Debug.LogWarning("The dialogue system is not running. Use StartDialogue() to start.");
                return;
            }

            if(CurrentState == DialogueState.PausedByTimeline)
            {
                if(settings.showWarnings) Debug.LogWarning("The dialogue system is paused by the playing timeline.");
                return;
            }

            _handler.TraverseGraph();


        }

        public void SelectDialogueChoice(int choiceNum)
        {
            if (CurrentState != DialogueState.AwaitingChoice)
            {
                Debug.LogWarning("The dialogue system is not awaiting a player choice. Current node is " + _handler.CurrentNode.NodeType);
                return;
            }
            _handler.HandleChoiceNode(_handler.CurrentNode as ChoiceNode, choiceNum);
        }

        public void SelectDialogueChoice(string choiceName)
        {
            if (CurrentState != DialogueState.AwaitingChoice)
            {
                if (settings.showWarnings) Debug.LogWarning("The dialogue system is not awaiting a player choice. Current node is " + _handler.CurrentNode.NodeType);
                return;
            }

            ChoiceNode choiceNode = _handler.CurrentNode as ChoiceNode;
            DialogueChoice selectedChoice = choiceNode.GetChoices().Choices.Find(x => x.choiceName == choiceName);
            if (selectedChoice == null)
            {
                Debug.LogError($"No choice with name {choiceName} found.");
                return;
            }

            _handler.HandleChoiceNode(_handler.CurrentNode as ChoiceNode, selectedChoice.choiceNumber);
        }

        public void Pause(bool toggle)
        {
            _handler.Pause(toggle);
        }

        public void OnApplicationPause(bool pause)
        {
            //Debug.Log("Pause");
            //Pause(pause);   
        }

        #region
        private void CheckHealth()
        {
            if (flags == null && _handler.CustomTestCondition == null)
            {
                Debug.LogWarning("Game Event Flags are not set, and no custom flag function is provided.");
            }
        }
        #endregion

        public void OnNotify(Playable origin, INotification notification, object context)
        {
            if (notification is DialogueSignalEmitter signal)
            {
                _handler.SetCurrentPlayable(origin);
                if(signal.asset.name.ToLower() == "triggerdialoguesignal")
                {
                    if (StartDialogueFromSignal != null)
                    {
                        StartDialogueFromSignal(signal);
                    }
                    else
                    {
                        if (signal.PauseTimeline) _handler.PauseCurrentPlayable(true);
                        dialogueGraph = signal.Graph;
                        StartDialogue();
                    }
                }

                if (signal.asset.name.ToLower() == "resumedialoguesignal")
                {
                    if (signal.PauseTimeline) _handler.PauseCurrentPlayable(true);
                    Ui.ShowDialoguePane(true);
                    _handler.ResumeDialoguePausedByTimeline();
                    AdvanceDialogue();
                }

                if (signal.asset.name.ToLower() == "advancedialoguesignal")
                {
                    if (signal.PauseTimeline) _handler.PauseCurrentPlayable(true);
                    Ui.ShowDialoguePane(true);
                    _handler.ResumeDialoguePausedByTimeline();
                    AdvanceDialogue();
                }
            }
        }


        //private void OnEnable()
        //{
        //    SceneManager.sceneLoaded += OnSceneLoaded;
        //}

        //private void OnDisable()
        //{
        //    SceneManager.sceneLoaded -= OnSceneLoaded;
        //}

        //private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        //{
        //    RebindUIReferences();
        //    Debug.Log($"场景 '{scene.name}' 加载完成，已重绑定UI引用");
        //}

        //public void RebindUIReferences()
        //{
        //    // 1. 查找关键的UI对象
        //    GameObject FindInScene(string name) => GameObject.Find(name);
        //    GameObject FindByTag(string tag) => GameObject.FindGameObjectWithTag(tag);

        //    // 2. 优先使用标签查找（更可靠）
        //    dialoguePane = FindByTag("DialoguePane") ?? FindInScene("DialoguePanel");
        //    dialogueTextGameObject = FindByTag("DialogueText") ?? FindInScene("DialogueText");

        //    // 3. 重构UI对象
        //    if (dialoguePane != null && dialogueTextGameObject != null)
        //    {
        //        Ui = new DialogueUI(dialogueTextGameObject, dialoguePane, theme, settings, dialogueCallbackActions);

        //        // 4. 重新应用主题
        //        if (theme != null)
        //        {
        //            Ui.SetTheme(theme);
        //        }
        //    }
        //    else
        //    {
        //        Debug.LogWarning($"UI引用重绑定失败: " +
        //            $"DialoguePane: {dialoguePane != null} | " +
        //            $"DialogueText: {dialogueTextGameObject != null}");
        //    }
        //}
    }
}
