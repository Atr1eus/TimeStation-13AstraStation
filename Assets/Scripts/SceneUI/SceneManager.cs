using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SceneManager : MonoBehaviour
{
    [Header("场景过渡配置")]
    [SerializeField] protected Image fadeImage; //过渡背景
    [SerializeField] protected float fadeDuration = 0.3f; //过渡持续时间

    protected CanvasGroup fadeCanvasGroup;
    protected bool isTransitioning;
    protected bool isStartScene = false;
    protected virtual void Awake()
    {
        InitializeFadeImage();
        if (!isStartScene) StartCoroutine(SceneEnterTransition());
        else isStartScene = false;
    }
    #region 场景淡入淡出功能
    protected virtual void InitializeFadeImage()
    {
        if (fadeImage == null) return;

        RectTransform rt = fadeImage.rectTransform;
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.sizeDelta = Vector2.zero;
        rt.SetAsLastSibling();

        fadeCanvasGroup = fadeImage.GetComponent<CanvasGroup>() ??
                        fadeImage.gameObject.AddComponent<CanvasGroup>();

        fadeCanvasGroup.alpha = 0f;
        fadeImage.color = Color.black;
    }

    protected IEnumerator SceneEnterTransition() //进入场景时调用
    {
        if (fadeImage == null) yield break;

        // 初始状态
        fadeCanvasGroup.alpha = 1f;
        fadeImage.gameObject.SetActive(true);

        // 使用unscaledDeltaTime避免受Time.timeScale影响 插值淡出
        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            yield return null;
        }

        fadeCanvasGroup.alpha = 0f;
    }

    protected IEnumerator TransitionToScene(SceneLoader.GameScene scene)
    {
        if (isTransitioning) yield break;
        //进入场景切换状态
        isTransitioning = true;
        yield return SceneLoader.Instance.FadeScreen(1f, fadeDuration);
        SceneLoader.Instance.LoadScene(scene, fadeImage, fadeCanvasGroup);
        isTransitioning = false;
    }
    #endregion

    public void UseButBanButton(Button button)
    {
        UseButton(button);
        BanButton(button);
    }
    public void BanButton(Button button)
    {
        button.interactable = false;
        button.image.color = Color.gray;
    }
    public void UnuseButton(Button button)
    {
        button.interactable = false;
        button.image.enabled = false;
        button.GetComponentInChildren<Text>().enabled = false;
    }
    public void UseButton(Button button)
    {

        button.interactable = true;
        button.image.enabled = true;
        button.GetComponentInChildren<Text>().enabled = true;
        button.image.color = Color.white;
    }
    public void UseImage(Image image)
    {
        image.gameObject.SetActive(true);
    }
    public void UnuseImage(Image image)
    {
        image.gameObject.SetActive(false);
    }
}
