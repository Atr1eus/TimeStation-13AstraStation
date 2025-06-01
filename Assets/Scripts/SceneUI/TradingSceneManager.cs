using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TradingSceneManager : SceneManager
{
    [SerializeField] private GameManager manager;
    [Header("UI×é¼þ")]
    [SerializeField] private Button loadRestSceneButton;
    [SerializeField] private Button nextNpcButton;

    protected override void Awake()
    {
        base.Awake();
        InitializeButtons();
    }

    private void InitializeButtons()
    {
        loadRestSceneButton?.onClick.AddListener(() =>
        {
            StartCoroutine(TransitionToScene(SceneLoader.GameScene.RestArea));
        });
    }
}