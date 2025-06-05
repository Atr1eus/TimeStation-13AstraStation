using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TradingSceneManager : SceneManager
{
    private Ticket currentTicket;
    [SerializeField] private GameManager manager;
    [SerializeField] private NpcCardManager cardManager;
    [SerializeField] private RoundManager roundManager;
    [Header("UI??")]
    [SerializeField] private Button loadRestSceneButton;
    [SerializeField] private Button nextNpcButton;
    [SerializeField] private Button agreeButton;
    [SerializeField] private Button disagreeButton;
    [SerializeField] private Button openTicketButton;
    [SerializeField] private Button closeTicketButton;
    [SerializeField] private Button openApplicationButton;
    [SerializeField] private Button closeApplicationButton;

    [SerializeField] private Transform npcSpawnPos;
    [SerializeField] private RectTransform ticketSpawnPos;
    [SerializeField] private RectTransform applicationSpawnPos;
    [SerializeField] private Text applicationBackpackText;
    [SerializeField] private Text applicationReasonText;
    [SerializeField] private Text applicationNameText;

    [SerializeField] private Image ticketImage;
    [SerializeField] private Text ticketNameText;
    [SerializeField] private Text ticketFromDateText;
    [SerializeField] private Text ticketToDateText;
    [SerializeField] private Text ticketIssuerText;

    [SerializeField] private Text rightTicketNameText;
    [SerializeField] private Text rightTicketFromDateText;
    [SerializeField] private Text rightTicketToDateText;
    [SerializeField] private Text rightTicketIssuerText;
    [SerializeField] private Text currentRoundText;
    [SerializeField] private Image applicationImage;
    [SerializeField] private Image decideImage;
    [SerializeField] private Transform handParent;
    private Transform npcHand;
    protected override void Awake()
    {
        base.Awake();

    }
    void Start()
    {
        GameManager.Instance.m_npcCard = FindObjectOfType<NpcCardManager>();
        manager = GameManager.Instance;
        NpcManager.Instance.handSpawnPoint = npcSpawnPos;
        roundManager = RoundManager.Instance;
        ticketImage.transform.position = ticketSpawnPos.position;
        applicationSpawnPos.transform.position = applicationSpawnPos.position;
        currentRoundText.text = (roundManager.currentRound + 1).ToString();
        InitializeButtonsEvent();
        InitializeUI();
        GameManager.Instance.OnNextRoundClick();
    }
    private void InitializeButtonsEvent()
    {
        loadRestSceneButton?.onClick.RemoveAllListeners();
        nextNpcButton?.onClick.RemoveAllListeners();
        agreeButton?.onClick.RemoveAllListeners();
        disagreeButton?.onClick.RemoveAllListeners();
        openTicketButton?.onClick.RemoveAllListeners();
        closeTicketButton?.onClick.RemoveAllListeners();
        openApplicationButton?.onClick.RemoveAllListeners();
        closeApplicationButton?.onClick?.RemoveAllListeners();

        loadRestSceneButton?.onClick.AddListener(() =>
        {
            SaveSystem.GameSave();
            StartCoroutine(TransitionToScene(SceneLoader.GameScene.RestArea));
        });

        disagreeButton?.onClick.AddListener(OnDisagreeButtonClick);
        nextNpcButton?.onClick.AddListener(manager.OnNextNpcButtonClick);
        agreeButton?.onClick.AddListener(manager.OnAgreeNpcButtonClick);
        nextNpcButton?.onClick.AddListener(InitializeUIStates);
        agreeButton?.onClick.AddListener(AgreeButtonState);
        openTicketButton?.onClick.AddListener(OnOpenTicketButtonClick);
        closeTicketButton?.onClick.AddListener(OnCloseTicketButtonClick);
        openApplicationButton?.onClick.AddListener(OnOpenApplicationButtonClick);
        closeApplicationButton?.onClick?.AddListener(OnCloseApplicationButtonClick);

    }
    public void OnDisagreeButtonClick()
    {
        manager.OnDisagreeNpcButtonClick();
        if (RoundManager.Instance.currentCanSelectNpcNum <= 0)
        {
            StartCoroutine(TransitionToScene(SceneLoader.GameScene.RestArea));
        }
        InitializeUIStates();
    }
    public void OnOpenTicketButtonClick()
    {

        currentTicket = NpcManager.Instance.currentNpc.npc.ticket;
        InitializeNpcTicket();
        UseImage(ticketImage);
        ticketImage.transform.position = ticketSpawnPos.position;
    }
    public void OnOpenApplicationButtonClick()
    {
        currentTicket = NpcManager.Instance.currentNpc.npc.ticket;
        if (NpcManager.Instance.currentNpc.npc.data.type == NpcType.Normal)
            InitializeNormalNpcApplication();
        else InitializeStoryNpcApplication();
        UseImage(applicationImage);
        applicationImage.transform.position = applicationSpawnPos.position;
    }
    public void OnCloseTicketButtonClick()
    {

        UnuseImage(ticketImage);
    }
    public void OnCloseApplicationButtonClick()
    {
        UnuseImage(applicationImage);
    }
    public void InitializeNpcTicket()
    {
        ticketNameText.text = currentTicket.npc.npcName;
        ticketFromDateText.text = currentTicket.leaveDate;
        ticketToDateText.text = currentTicket.targetDate;
        ticketIssuerText.text = currentTicket.ticketIssuer;
        rightTicketNameText.text = ticketNameText.text;
        rightTicketFromDateText.text = ticketFromDateText.text;
        rightTicketIssuerText.text = ticketIssuerText.text;
        rightTicketToDateText.text = ticketToDateText.text;
    }
    public void InitializeNormalNpcApplication()
    {
        applicationBackpackText.text = NpcManager.Instance.currentNpc.npc.data.backPack;
        applicationReasonText.text = NpcManager.Instance.currentNpc.npc.data.reason;
        applicationNameText.text = NpcManager.Instance.currentNpc.npc.data.npcName;
    }
    public void InitializeStoryNpcApplication()
    {
        applicationBackpackText.text = NpcManager.Instance.currentNpc.npc.data.storyNpcBackPack[NpcManager.Instance.currentNpc.npc.selectedTimes];
        applicationReasonText.text = NpcManager.Instance.currentNpc.npc.data.storyNpcReasons[NpcManager.Instance.currentNpc.npc.selectedTimes]; ;
        applicationNameText.text = NpcManager.Instance.currentNpc.npc.data.npcName;
    }
    public void InitializeUI()
    {
        InitCloseImage(decideImage);
        InitCloseButton(nextNpcButton);
        InitCloseButton(openApplicationButton);
        InitCloseButton(openTicketButton);
        InitCloseImage(ticketImage);
        InitCloseImage(applicationImage);
        InitCloseButton(loadRestSceneButton);
    }
    public void InitializeUIStates()
    {
        UnuseImage(decideImage);
        UnuseButton(nextNpcButton);
        UnuseButton(loadRestSceneButton);
        UnuseButton(openTicketButton);
        UnuseButton(openApplicationButton);
        OnCloseApplicationButtonClick();
        OnCloseTicketButtonClick();
    }
    public void DecidedNpcButtonState()
    {
        UnuseImage(decideImage);
        UnuseButton(nextNpcButton);
        UseButton(openTicketButton);
        UseButton(openApplicationButton);
        npcHand = FindObjectOfType<NpcController>().transform;
        npcHand.SetParent(handParent);
    }
    public void OnDialogueEnd()
    {
        UseImage(decideImage); UseImage(decideImage);
        UseButton(disagreeButton);
        if (RoundManager.Instance.canSelect)
        {
            UseButton(agreeButton);
            UnuseButton(loadRestSceneButton);
        }
        else
        {
            UseButBanButton(agreeButton);
            UseButton(loadRestSceneButton);
        }
    }
    public void GiveButtonClickButtonState()
    {
        UnuseImage(decideImage);
        if (RoundManager.Instance.canSelect && RoundManager.Instance.currentCanSelectNpcNum > 0)
        {
            UseButBanButton(nextNpcButton);
            UnuseButton(loadRestSceneButton);
        }
        else
        {
            UseButBanButton(loadRestSceneButton);
            UseButBanButton(nextNpcButton);
        }
        UnuseButton(openTicketButton);
        UnuseButton(openApplicationButton);
        GameManager.Instance.OnInventoryBrowseButtonClick();
    }
    public void DialogueAgree()
    {
        agreeButton.onClick?.Invoke();
    }
    public void DialigueDisagree()
    {
        disagreeButton.onClick?.Invoke();
    }
    public void AgreeButtonState()
    {
        UnuseImage(decideImage);
        if (RoundManager.Instance.canSelect && RoundManager.Instance.currentCanSelectNpcNum > 0)
        {
            UseButton(nextNpcButton);
            UnuseButton(loadRestSceneButton);
        }
        else
        {
            UseButton(loadRestSceneButton);
            UseButton(nextNpcButton);
        }
        UnuseButton(openTicketButton);
        UnuseButton(openApplicationButton);
        UnuseImage(ticketImage);
        UnuseImage(applicationImage);
    }
}