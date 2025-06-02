using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestSceneManager : SceneManager
{
    [Header("UI??")]
    [SerializeField] private Button loadTradingSceneButton;
    [SerializeField] private Button SaveButton;
    [SerializeField] private Button LoadButton;
    protected override void Awake()
    {
        base.Awake();
        InitializeButtonsAwake();
    }
    protected void Start()
    {
        InitializeButtonsStart();
    }

    private void InitializeButtonsAwake()
    {
        loadTradingSceneButton?.onClick.AddListener(() =>
        {
            StartCoroutine(TransitionToScene(SceneLoader.GameScene.TradingArea));
        });
    }
    private void InitializeButtonsStart()
    {

        SaveButton?.onClick.AddListener(() => SaveSystem.Instance.GameSave());
        LoadButton?.onClick.AddListener(() => SaveSystem.Instance.GameLoad());
    }
}