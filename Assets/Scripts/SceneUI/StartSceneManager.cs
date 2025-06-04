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

    protected override void Awake()
    {
        isStartScene = true;
        base.Awake();
        isStartScene = false;
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
    }
    private void InitializeButtonsStart()
    {
        InitializeLoadButton();

    }
    private void InitializeLoadButton()
    {
        string savePath = Path.Combine(Application.persistentDataPath, "global_save.json");

    }

}