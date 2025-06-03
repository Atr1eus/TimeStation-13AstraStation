using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TradingSceneManager : SceneManager
{
    [SerializeField] private GameManager manager;
    [SerializeField] private NpcCardManager cardManager;
    [Header("UI??")]
    [SerializeField] private Button loadRestSceneButton;
    [SerializeField] private Button nextNpcButton;
    [SerializeField] private Button agreeButton;
    [SerializeField] private Button disagreeButton;
    [SerializeField] private Button giveButton;
    [SerializeField] private Button item0Button;
    [SerializeField] private Button item1Button;
    [SerializeField] private Button item2Button;
    [SerializeField] private Button exitBagButton;
    [SerializeField] private Transform npcSpawnPos;
    [SerializeField] private Transform ticketSpawnPos;
    protected override void Awake()
    {
        base.Awake();
        InitializeButtonsAwake();

    }
    void Start()
    {
        GameManager.Instance.m_npcCard = FindObjectOfType<NpcCardManager>();
        manager = GameManager.Instance;
        NpcManager.Instance.handSpawnPoint = npcSpawnPos;
        TicketManager.Instance.ticketContainer = ticketSpawnPos;
        InitializeButtonsStart();
        InitializeButtonStates();
        //todo:下面这行暂时放这 白盒测试用
        GameManager.Instance.OnNextRoundClick();
    }
    private void InitializeButtonsStart()
    {
        nextNpcButton?.onClick.AddListener(manager.OnNextNpcButtonClick);
        agreeButton?.onClick.AddListener(manager.OnAgreeNpcButtonClick);
        item0Button?.onClick.AddListener(manager.TESTOfferItem);
        item1Button?.onClick.AddListener(manager.TESTOfferIte1);
        item2Button?.onClick.AddListener(manager.TESTOfferItem2);
        nextNpcButton?.onClick.AddListener(InitializeButtonStates);
        agreeButton?.onClick.AddListener(AgreeButtonState);
        giveButton?.onClick.AddListener(GiveButtonClickButtonState);
        exitBagButton?.onClick.AddListener(AgreeButtonState);

    }
    private void InitializeButtonsAwake()
    {
        loadRestSceneButton?.onClick.AddListener(() =>
        {
            SaveSystem.GameSave();
            StartCoroutine(TransitionToScene(SceneLoader.GameScene.RestArea));
        });

        disagreeButton?.onClick.AddListener(OnDisagreeButtonClick);
    }
    public void OnDisagreeButtonClick()
    {
        manager.OnDisagreeNpcButtonClick();
        if (RoundManager.Instance.currentCanSelectNpcNum <= 0)
        {
            StartCoroutine(TransitionToScene(SceneLoader.GameScene.RestArea));
        }
        InitializeButtonStates();
    }
    public void InitializeButtonStates()
    {
        UnuseButton(agreeButton);
        UnuseButton(disagreeButton);
        UnuseButton(nextNpcButton);
        UnuseButton(loadRestSceneButton);
        UnuseButton(giveButton);
        UnuseButton(item0Button);
        UnuseButton(item1Button);
        UnuseButton(item2Button);
        UnuseButton(exitBagButton);
    }
    public void DecidedNpcButtonState()
    {
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
        UnuseButton(nextNpcButton);
        UnuseButton(giveButton);
        UnuseButton(item0Button);
        UnuseButton(item1Button);
        UnuseButton(item2Button);
        UnuseButton(exitBagButton);
    }
    public void GiveButtonClickButtonState()
    {
        UseButBanButton(agreeButton);
        UseButBanButton(disagreeButton);
        UseButBanButton(giveButton);
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
        UseButton(item0Button);
        UseButton(item1Button);
        UseButton(item2Button);
        UseButton(exitBagButton);
    }
    public void AgreeButtonState()
    {
        UseButBanButton(agreeButton);
        UseButBanButton(disagreeButton);
        UseButton(giveButton);
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
        UnuseButton(item0Button);
        UnuseButton(item1Button);
        UnuseButton(item2Button);
        UnuseButton(exitBagButton);
    }
    public void UseButBanButton(Button button)
    {
        UseButton(button);
        BanButton(button);
    }
}