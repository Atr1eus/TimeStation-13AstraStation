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
    [SerializeField] private Image loadImage;
    [SerializeField] private Button loadImageConfirmButton;
    [SerializeField] private Button loadImageCancelButton;
    [SerializeField] private Button loadImageQuitButton;
    protected override void Awake()
    {
        isStartScene = true;
        base.Awake();
        isStartScene = false;
        creatorList.gameObject.SetActive(false);
    }

    protected override void Start()
    {
        startGameButton?.onClick.RemoveAllListeners();
        quitGameButton?.onClick.RemoveAllListeners();
        creatorListButton?.onClick.RemoveAllListeners();
        creatorListCloseButton?.onClick.RemoveAllListeners();
        loadImageCancelButton?.onClick.RemoveAllListeners();
        loadImageConfirmButton?.onClick.RemoveAllListeners();
        loadImageQuitButton?.onClick.RemoveAllListeners();


        InitializeButtonsStart();
        InitializeLoadImage();
        if (SaveSystem.InitLoad() == false)
        {
            SaveSystem.InitSave();
        }
    }
    private void InitializeButtonsStart()
    {
        creatorList.gameObject.SetActive(false);
        loadImage.gameObject.SetActive(false);


        startGameButton?.onClick.AddListener(() =>
        {
            if (!HadSaveFile()) StartCoroutine(TransitionToScene(SceneLoader.GameScene.TradingArea));
            else
            {
                WhiteBanButton(startGameButton);
                WhiteBanButton(creatorListButton);
                loadImage.gameObject.SetActive(true);
            }
        });


        quitGameButton?.onClick.AddListener(() => Application.Quit());
        creatorListButton?.onClick.AddListener(OnCreatorListButtonClick);
        creatorListCloseButton?.onClick.AddListener(OnCreatorListCloseButtonClick);
    }
    private void OnCreatorListCloseButtonClick()
    {
        creatorList.gameObject.SetActive(false);
    }
    private void InitializeLoadImage()
    {
        loadImageConfirmButton?.onClick.AddListener(OnLoadConfirmButtonClick);
        loadImageConfirmButton?.onClick.AddListener(() => StartCoroutine(TransitionToScene(SceneLoader.GameScene.RestArea)));
        loadImageCancelButton?.onClick.AddListener(StartNewGame);
        loadImageQuitButton?.onClick.AddListener(OnLoadCancelButtonClick);
    }
    private void OnLoadConfirmButtonClick()
    {
        SaveSystem.GameLoad();
    }
    private void OnLoadCancelButtonClick()
    {
        InitUseButton(startGameButton);
        InitUseButton(creatorListButton);
        loadImage.gameObject.SetActive(false);
    }
    private void OnCreatorListButtonClick()
    {
        creatorList.rectTransform.position = creatorListPos.position;
        creatorList.gameObject.SetActive(true);
    }
    private void StartNewGame()
    {
        StartCoroutine(TransitionToScene(SceneLoader.GameScene.TradingArea));
    }
    private bool HadSaveFile()
    {
        string savePath = Path.Combine(Application.persistentDataPath, "global_save.json");
        if (!File.Exists(savePath)) return false;
        else return true;
    }


}