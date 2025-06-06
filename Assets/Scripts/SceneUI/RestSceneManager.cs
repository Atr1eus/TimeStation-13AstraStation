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
    [SerializeField] private Image rankUpImage;
    [SerializeField] private Button rankUpOpenButton;
    [SerializeField] private Button rankUpCloseButton;
    [SerializeField] private Button trainCountUpButton;
    [SerializeField] private Button customerCountUpButton;
    [SerializeField] private Button returnToMainButton;
    [SerializeField] private Image reportImage;
    [SerializeField] private Text newsReportText1;
    [SerializeField] private Text newsReportText2;
    [SerializeField] private Text newsReportText3;
    [SerializeField] private Text customerNeedMoney;
    [SerializeField] private Text TrainNeedMoney;
    [SerializeField] private Button awakeDailyReportButton;
    [SerializeField] private Button exitDailyReportButton;
    [SerializeField] private RectTransform dailyReportPos;
    [SerializeField] private RectTransform RankUpPos;
    [SerializeField] private CanvasGroup canvas;
    private List<string> newsReport;
    private int newsReportidx = -1;

    private InventorySystem m_inventory;

    protected override void Awake()
    {
        base.Awake();

    }
    protected override void Start()
    {
        canvas.interactable = true;
        newsReportText1.text = "---";
        newsReportText2.text = "---";
        newsReportText3.text = "---";
        newsReportidx = -1;
        newsReport = new List<string>();

        m_player = FindAnyObjectByType<PlayerController>();
        m_inventory = FindObjectOfType<InventorySystem>();
        m_round = FindAnyObjectByType<RoundManager>();
        currentRoundText.text = RoundManager.Instance.currentRound.ToString();
        InitializeUI();
        InitializeButtonsEvent();
        InitializeReport();
    }

    protected override void Update()
    {
        base.Update();
        positionText.text = RoundManager.Instance.maxSelectionsPerRound.ToString();
        customerText.text = RoundManager.Instance.selectionsPerRound.ToString();
    }


    public void InitializeReport()
    {

        for (int i = 0; i < m_round.currentSelectedNpcs.Count; i++)
        {
            Npc npc = m_round.currentSelectedNpcs[i];
            if (npc.data.type == NpcType.Story)
            {
                newsReportidx++;
                newsReport.Add(npc.data.storyNpcFollowUpPlot[npc.selectedTimes - 1]);
            }
        }
        newsReportText1.text = newsReportidx >= 0 ? newsReport[0] : "---";
        newsReportText2.text = newsReportidx >= 1 ? newsReport[1] : "---";
        newsReportText3.text = newsReportidx >= 2 ? newsReport[2] : "---";
    }

    private void InitializeButtonsEvent()
    {
        loadTradingSceneButton?.onClick.RemoveAllListeners();
        saveButton?.onClick.RemoveAllListeners();
        rankUpCloseButton?.onClick.RemoveAllListeners();
        rankUpOpenButton?.onClick.RemoveAllListeners();
        customerCountUpButton?.onClick.RemoveAllListeners();
        trainCountUpButton?.onClick?.RemoveAllListeners();
        awakeDailyReportButton?.onClick?.RemoveAllListeners();
        exitDailyReportButton?.onClick.RemoveAllListeners();
        returnToMainButton?.onClick.RemoveAllListeners();

        saveButton?.onClick.AddListener(() => SaveSystem.GameSave());
        loadTradingSceneButton?.onClick.AddListener(() =>
        {
            canvas.interactable = false;
            StartCoroutine(TransitionToScene(SceneLoader.GameScene.TradingArea));
        });
        rankUpCloseButton?.onClick?.AddListener(OnRankUpCloseButtonClick);
        trainCountUpButton?.onClick?.AddListener(OnTrainCountUpButtonClick);
        customerCountUpButton?.onClick?.AddListener(OnCustomerCountRankUpButtonClick);
        rankUpOpenButton?.onClick?.AddListener(OnRankUpOpenButtonClick);
        awakeDailyReportButton?.onClick?.AddListener(OnAwakeDailyImageButtonClick);
        exitDailyReportButton?.onClick.AddListener(OnExitDailyImageButtonClick);
        returnToMainButton?.onClick?.AddListener(() =>
        {
            if(SaveSystem.HadInitSaveFile())
            {
                SaveSystem.InitLoad();
            }
            canvas.interactable = false;
            StartCoroutine(TransitionToScene(SceneLoader.GameScene.MainArea));
        });
    }
    public void OnAwakeDailyImageButtonClick()
    {
        reportImage.rectTransform.position = dailyReportPos.position;
        UseImage(reportImage);
        UseButton(saveButton);
        UseButton(loadTradingSceneButton);
        UseButton(rankUpOpenButton);
        UseButton(exitDailyReportButton);
    }
    public void OnExitDailyImageButtonClick()
    {
        InitializeScene();
    }
    public void OnRankUpOpenButtonClick()
    {
        rankUpImage.rectTransform.position = RankUpPos.position;
        UseButton(saveButton);
        UseButton(loadTradingSceneButton);
        UseButton(rankUpOpenButton);
        UseButton(awakeDailyReportButton);
        UseImage(rankUpImage);
        UseButton(rankUpCloseButton);
        TrainNeedMoney.text = m_player.trainCountRank >= m_player.MaxTrainRank ?
                         "---" :
                         m_player.trainRankUpNeedMoney[m_player.trainCountRank].ToString();

        customerNeedMoney.text = m_player.customerCountRank >= m_player.MaxCustomerCountRank ?
                                 "---" :
                                m_player.customerCountRankUpNeedMoney[m_player.customerCountRank].ToString();

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
        TrainNeedMoney.text = m_player.trainCountRank >= m_player.MaxTrainRank ?
                         "---" :
                         m_player.trainRankUpNeedMoney[m_player.trainCountRank].ToString();
        if (m_player.CanTrainRankUp()) UseButton(trainCountUpButton);
        else UseButBanButton(trainCountUpButton);
        if (m_player.CanCustomerRankUp()) UseButton(customerCountUpButton);
        else UseButBanButton(customerCountUpButton);
    }
    public void OnCustomerCountRankUpButtonClick()
    {
        m_player.CustomerCountRankUp();
        m_round.AddSelectionsPerRound();
        customerNeedMoney.text = m_player.customerCountRank >= m_player.MaxCustomerCountRank ?
                                 "---" :
                                m_player.customerCountRankUpNeedMoney[m_player.customerCountRank].ToString();
        if (m_player.CanCustomerRankUp()) UseButton(customerCountUpButton);
        else UseButBanButton(customerCountUpButton);
        if (m_player.CanTrainRankUp()) UseButton(trainCountUpButton);
        else UseButBanButton(trainCountUpButton);
    }
    public void InitializeUI()
    {
        InitCloseImage(reportImage);
        InitCloseImage(rankUpImage);
        UseButton(saveButton);
        UseButton(loadTradingSceneButton);
        UseButton(rankUpOpenButton);
        UseButton(awakeDailyReportButton);
    }
    public void InitializeScene()
    {
        UnuseImage(rankUpImage);
        UnuseImage(reportImage);
        UseButton(saveButton);
        UseButton(loadTradingSceneButton);
        UseButton(rankUpOpenButton);
        UseButton(awakeDailyReportButton);
    }


}