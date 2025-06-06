using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.IO;
using Unity.VisualScripting;

public class EndSceneManager : SceneManager
{

    [Header("End Attributes")]
    public Image BestTravel_EndImage;     // 好结局图片
    public Image SK_EndImage;     // 坏结局图片
    public Image Poor_EndImage;
    public Button returnToMainButton;

    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        BestTravel_EndImage.gameObject.SetActive(false);
        SK_EndImage.gameObject.SetActive(false);
        Poor_EndImage.gameObject.SetActive(false);


        returnToMainButton.onClick.RemoveAllListeners();

        if (FinalManager.FinalNum == 0) BestTravel_EndImage.gameObject.SetActive(true);
        else if (FinalManager.FinalNum == 1) SK_EndImage.gameObject.SetActive(true);
        else if (FinalManager.FinalNum == 2) Poor_EndImage.gameObject.SetActive(true);
        else StartCoroutine(TransitionToScene(SceneLoader.GameScene.RestArea));
        returnToMainButton?.onClick?.AddListener(() =>
        {
            StartCoroutine(TransitionToScene(SceneLoader.GameScene.MainArea));
        });
    }

    // Update is called once per frame
    protected override void Update()
    {

    }
}
