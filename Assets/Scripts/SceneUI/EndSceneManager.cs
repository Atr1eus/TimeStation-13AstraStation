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
    [SerializeField] private Button BacktoMenuButton;

    [Header("End Attributes")]
    public Image SK_EndImage;    // 好结局图片
    public Image BestTravel_EndImage;     // 坏结局图片
    public Image Poor_EndImage;
    public GameObject Text1;
    public GameObject Text2;
    public GameObject Text3;
    int gold;
    int SKs;
    int Rounds;

    protected override void Awake()
    {
        base.Awake();
    }
    // Start is called before the first frame update
    void Start()
    {
        BacktoMenuButton.onClick.AddListener(() => StartCoroutine(TransitionToScene(SceneLoader.GameScene.MainArea)));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
