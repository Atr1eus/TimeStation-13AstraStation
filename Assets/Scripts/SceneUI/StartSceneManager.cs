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
    [SerializeField] private Button startButton;
    [SerializeField] private Button loadButton;
    [SerializeField] private Button quitButton;

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
        loadButton?.onClick.AddListener(() =>
        {
            SaveSystem.GameLoad();

            StartCoroutine(TransitionToScene(SceneLoader.GameScene.RestArea));
        });
        startButton?.onClick.AddListener(() =>
        {
            StartCoroutine(TransitionToScene(SceneLoader.GameScene.TradingArea));
        });
        quitButton?.onClick.AddListener(() => Application.Quit());
    }
    private void InitializeButtonsStart()
    {
        InitializeLoadButton();

    }
    private void InitializeLoadButton()
    {
        string savePath = Path.Combine(Application.persistentDataPath, "global_save.json");
        if (!File.Exists(savePath)) BanButton(loadButton);
        else UseButton(loadButton);
    }

}