using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : SingletonMonoBehaviour<SceneLoader>
{
    public enum GameScene
    {
        MainArea,
        TradingArea,
        RestArea
    }

    [System.Serializable]
    public class SceneConfig //场景数据
    {
        public GameScene sceneType;
        public string sceneName;
    }

    [SerializeField] private SceneConfig[] scenes;
    private static Image activeFadeImage;
    private static CanvasGroup activeFadeCanvasGroup;
    private bool isPreloading;
    public Dictionary<GameScene, string> sceneDictionary = new Dictionary<GameScene, string>();

    public void InitializeSceneDictionary()
    {
        foreach (var scene in scenes)
        {
            sceneDictionary.Add(scene.sceneType, scene.sceneName);
        }
    }



    public void LoadScene(GameScene scene, Image fadeImage, CanvasGroup fadeCanvasGroup, LoadSceneMode mode = LoadSceneMode.Single)
    {
        if (sceneDictionary.TryGetValue(scene, out string sceneName))
        {
            StartCoroutine(LoadSceneCoroutine(sceneName, fadeImage, fadeCanvasGroup));
        }
    }

    protected override void Awake()
    {
        base.Awake();
        InitializeSceneDictionary();
    }

    private IEnumerator LoadSceneCoroutine(string sceneName, Image fadeImage, CanvasGroup fadeCanvasGroup)
    {
        // 开始预加载
        isPreloading = true;
        AsyncOperation op = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;

        // 并行执行淡入和预加载
        yield return StartCoroutine(ParallelFadeAndLoad(op, fadeCanvasGroup));

        // 激活场景
        op.allowSceneActivation = true;
        isPreloading = false;
    }
    private IEnumerator ParallelFadeAndLoad(AsyncOperation op, CanvasGroup cg)
    {
        float fadeProgress = 0f;
        float minFadeTime = 0.5f; // 最小淡入时间

        // 同时监控淡入和加载进度
        while (fadeProgress < 1f || op.progress < 0.9f)
        {
            // 更新淡入进度
            if (fadeProgress < 1f)
            {
                fadeProgress += Time.unscaledDeltaTime / minFadeTime;
                cg.alpha = Mathf.Clamp01(fadeProgress);
            }

            // 更新加载进度
            if (op.progress >= 0.9f && fadeProgress >= 1f)
            {
                break;
            }

            yield return null;
        }

        cg.alpha = 1f; // 确保完全变黑
    }
    public IEnumerator FadeScreen(float targetAlpha, float duration)
    {
        if (activeFadeCanvasGroup == null || activeFadeImage == null) yield break;

        float startAlpha = activeFadeCanvasGroup.alpha;
        float time = 0f;

        //根据Alpha值判断是否阻止玩家点击按键
        activeFadeCanvasGroup.blocksRaycasts = targetAlpha > 0.01f;
        activeFadeImage.raycastTarget = targetAlpha > 0.01f;

        //由初始Alpha向目标Alpha进行插值
        while (time < duration)
        {
            time += Time.unscaledDeltaTime;
            activeFadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            yield return null;
        }
        //确保最终Alpha正确
        activeFadeCanvasGroup.alpha = targetAlpha;
    }

    public void LoadRestScene(Image fadeImage, CanvasGroup fadeCanvasGroup)
    {
        Load(GameScene.RestArea, fadeImage, fadeCanvasGroup);
    }
    public void LoadTradingScene(Image fadeImage, CanvasGroup fadeCanvasGroup)
    {
        Load(GameScene.TradingArea, fadeImage, fadeCanvasGroup);
    }

    public static void Load(GameScene scene, Image fadeImage, CanvasGroup fadeCanvasGroup, LoadSceneMode mode = LoadSceneMode.Single)
    {
        if (Instance != null)
        {
            Instance.LoadScene(scene, fadeImage, fadeCanvasGroup, mode);
        }
        else
        {
            Debug.LogError("SceneLoader instance not found!");
        }
    }
}