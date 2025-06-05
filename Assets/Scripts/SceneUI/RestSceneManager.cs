using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestSceneManager : SceneManager
{
    private PlayerController m_player;
    private RoundManager m_round;
    [Header("UI??")]
    [SerializeField] private Button loadTradingSceneButton;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button loadButton;
    [SerializeField] private Image rankUpImage;
    [SerializeField] private Button rankUpOpenButton;
    [SerializeField] private Button rankUpCloseButton;
    [SerializeField] private Button trainCountUpButton;
    [SerializeField] private Button customerCountUpButton;
    [SerializeField] private Image reportImage;
    [SerializeField] private Text decreaseItemHeaderText;
    [SerializeField] private Text totalItemHeaderText;
    [SerializeField] private Text newsReportText;
    [SerializeField] private Button awakeDailyReportButton;
    [SerializeField] private Button exitDailyReportButton;
    private InventorySystem m_inventory;

    protected override void Awake()
    {
        base.Awake();

    }
    protected void Start()
    {
        decreaseItemHeaderText.text = "";
        totalItemHeaderText.text = "";
        newsReportText.text = "";
        m_player = FindAnyObjectByType<PlayerController>();
        m_inventory = FindObjectOfType<InventorySystem>();
        m_round = FindAnyObjectByType<RoundManager>();
        InitializeUI();
        InitializeButtonsEvent();
        InitializeReport();
    }



    public void InitializeReport()
    {
        if (m_round.currentRound > 1)
        {
            for (int i = 0; i < m_round.decreaseItemList.Count; i++)
            {
                decreaseItemHeaderText.text += m_round.decreaseItemList[i];
                decreaseItemHeaderText.text += "     " + m_round.decreaseItemCountList[i] + "\n";
            }
        }
        for (int i = 0; i < m_inventory.items.Count; i++)
        {
            totalItemHeaderText.text += m_inventory.items[i].data.name;
            totalItemHeaderText.text += "      " + m_inventory.items[i].amount + "\n";
        }
        for (int i = 0; i < m_round.currentStoryNpcList.Count; i++)
        {
            Npc npc = m_round.currentStoryNpcList[i];
            newsReportText.text += npc.data.storyNpcFollowUpPlot[npc.selectedTimes - 1] + "\n";
        }
    }

    private void InitializeButtonsEvent()
    {
        loadTradingSceneButton?.onClick.RemoveAllListeners();
        saveButton?.onClick.RemoveAllListeners();
        loadButton?.onClick.RemoveAllListeners();
        rankUpCloseButton?.onClick.RemoveAllListeners();
        rankUpOpenButton?.onClick.RemoveAllListeners();
        customerCountUpButton?.onClick.RemoveAllListeners();
        trainCountUpButton?.onClick?.RemoveAllListeners();
        awakeDailyReportButton?.onClick?.RemoveAllListeners();
        exitDailyReportButton?.onClick.RemoveAllListeners();

        saveButton?.onClick.AddListener(() => SaveSystem.GameSave());
        loadButton?.onClick.AddListener(() => SaveSystem.GameLoad());
        loadButton?.onClick.AddListener(() => StartCoroutine(TransitionToScene(SceneLoader.GameScene.RestArea)));
        loadTradingSceneButton?.onClick.AddListener(() =>
        {
            StartCoroutine(TransitionToScene(SceneLoader.GameScene.TradingArea));
        });
        rankUpCloseButton?.onClick?.AddListener(OnRankUpCloseButtonClick);
        trainCountUpButton?.onClick?.AddListener(OnTrainCountUpButtonClick);
        customerCountUpButton?.onClick?.AddListener(OnCustomerCountRankUpButtonClick);
        rankUpOpenButton?.onClick?.AddListener(OnRankUpOpenButtonClick);
        awakeDailyReportButton?.onClick?.AddListener(OnAwakeDailyImageButtonClick);
        exitDailyReportButton?.onClick.AddListener(OnExitDailyImageButtonClick);
    }
    public void OnAwakeDailyImageButtonClick()
    {
        UseImage(reportImage);
        UseButBanButton(loadButton);
        UseButBanButton(saveButton);
        UseButBanButton(loadTradingSceneButton);
        UseButBanButton(rankUpOpenButton);
        UseButBanButton(exitDailyReportButton);
    }
    public void OnExitDailyImageButtonClick()
    {
        InitializeScene();
    }
    public void OnRankUpOpenButtonClick()
    {
        UseButBanButton(loadButton);
        UseButBanButton(saveButton);
        UseButBanButton(loadTradingSceneButton);
        UseButBanButton(rankUpOpenButton);
        UseButBanButton(awakeDailyReportButton);
        UseImage(rankUpImage);
        UseButton(rankUpCloseButton);
        UnuseButton(trainCountUpButton);
        UnuseButton(customerCountUpButton);


        if (m_player.CanTrainRankUp()) UseButton(trainCountUpButton);
        else UseButBanButton(trainCountUpButton);

        if (m_player.CanCustomerRankUp()) UseButton(customerCountUpButton);
        else UseButBanButton(customerCountUpButton);
    }
    public void OnRankUpCloseButtonClick()
    {
        InitializeScene();
    }
    public void OnTrainCountUpButtonClick()
    {
        m_player.TrainCountRankUp();
        m_round.AddMaxSelectionsPerRound();
        if (m_player.CanTrainRankUp()) UseButton(trainCountUpButton);
        else UseButBanButton(trainCountUpButton);
    }
    public void OnCustomerCountRankUpButtonClick()
    {
        m_player.CustomerCountRankUp();
        m_round.AddSelectionsPerRound();
        if (m_player.CanCustomerRankUp()) UseButton(customerCountUpButton);
        else UseButBanButton(customerCountUpButton);
    }
    public void InitializeUI()
    {
        InitCloseImage(reportImage);
        InitCloseImage(rankUpImage);
        UseButton(loadButton);
        UseButton(saveButton);
        UseButton(loadTradingSceneButton);
        UseButton(rankUpOpenButton);
        UseButton(awakeDailyReportButton);
    }
    public void InitializeScene()
    {
        UnuseImage(rankUpImage);
        UnuseImage(reportImage);
        UseButton(loadButton);
        UseButton(saveButton);
        UseButton(loadTradingSceneButton);
        UseButton(rankUpOpenButton);
        UseButton(awakeDailyReportButton);
    }


}