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

    protected override void Awake()
    {
        base.Awake();

    }
    protected void Start()
    {
        m_player = FindAnyObjectByType<PlayerController>();
        m_round = FindAnyObjectByType<RoundManager>();
        InitializeScene();
        InitializeButtonsEvent();
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
    }
    public void OnRankUpOpenButtonClick()
    {
        UseButBanButton(loadButton);
        UseButBanButton(saveButton);
        UseButBanButton(loadTradingSceneButton);
        UseButBanButton(rankUpOpenButton);
        UseImage(rankUpImage);
        UseButton(rankUpCloseButton);
        UnuseButton(trainCountUpButton);
        UnuseButton(customerCountUpButton);
        if (m_player.CanTrainRankUp()) UseButton(trainCountUpButton);
        if (m_player.CanCustomerRankUp()) UseButton(customerCountUpButton);
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
    }
    public void OnCustomerCountRankUpButtonClick()
    {
        m_player.CustomerCountRankUp();
        m_round.AddSelectionsPerRound();
        if (m_player.CanCustomerRankUp()) UseButton(customerCountUpButton);
    }
    public void InitializeScene()
    {
        UnuseImage(rankUpImage);
        UseButton(loadButton);
        UseButton(saveButton);
        UseButton(loadTradingSceneButton);
        UseButton(rankUpOpenButton);
    }
}