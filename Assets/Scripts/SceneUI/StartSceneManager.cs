using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.IO;
public class StartSceneManager : SceneManager
{
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button quitGameButton;
    [SerializeField] private Button creatorListButton;
    [SerializeField] private Button creatorListCloseButton;
    [SerializeField] private Image creatorList;
    [SerializeField] private RectTransform creatorListPos;
    protected override void Awake()
    {
        isStartScene = true;
        base.Awake();
        isStartScene = false;
        creatorList.gameObject.SetActive(false);
        InitializeButtonsAwake();
    }

    void Start()
    {
        InitializeButtonsStart();
    }
    private void InitializeButtonsAwake()
    {
        startGameButton?.onClick.AddListener(() =>
        {
            StartCoroutine(TransitionToScene(SceneLoader.GameScene.TradingArea));
        });
        quitGameButton?.onClick.AddListener(() => Application.Quit());
        creatorListButton?.onClick.AddListener(OnCreatorListButtonClick);
        creatorListCloseButton?.onClick.AddListener(OnCreatorListCloseButtonClick);
    }
    private void InitializeButtonsStart()
    {
        InitializeLoadButton();
        creatorList.gameObject.SetActive(false);
    }
    private void OnCreatorListCloseButtonClick()
    {
        creatorList.gameObject.SetActive(false);
    }

    private void OnCreatorListButtonClick()
    {
        creatorList.rectTransform.position = creatorListPos.position;
        creatorList.gameObject.SetActive(true);
    }
    private void InitializeLoadButton()
    {
        string savePath = Path.Combine(Application.persistentDataPath, "global_save.json");

    }


}