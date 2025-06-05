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
    public float scaleDuration = 0.5f; // 缩放持续时间
    public Vector3 targetScale = Vector3.one; // 目标缩放大小
    [Tooltip("是否启用缩放动画")]
    public bool enableScaleAnimation = true;
    [Tooltip("初始缩放比例(0表示从零开始)")]
    public float startImageScale = 0f;

    [Tooltip("目标缩放比例")]
    public float targetImageScale = 1f;
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



    private IEnumerator AnimateButtonAppearance(Button button)
    {
        // 确保组件启用
        button.image.enabled = true;
        if (button.GetComponentInChildren<Text>() != null)
            button.GetComponentInChildren<Text>().enabled = true;

        // 初始状态设置
        button.image.color = new Color(1, 1, 1, 0); // 完全透明
        button.transform.localScale = Vector3.zero; // 初始缩放为0

        // 并行执行淡入和缩放动画
        StartCoroutine(FadeInButton(button));
        yield return StartCoroutine(ScaleButton(button));
    }

    private IEnumerator FadeInButton(Button button)
    {
        float elapsedTime = 0f;
        Image buttonImage = button.image;
        Text buttonText = button.GetComponentInChildren<Text>();

        Color startColor = new Color(1, 1, 1, 0);
        Color targetColor = Color.white;

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            buttonImage.color = new Color(1, 1, 1, alpha);

            if (buttonText != null)
            {
                buttonText.color = new Color(buttonText.color.r,
                                           buttonText.color.g,
                                           buttonText.color.b,
                                           alpha);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 确保最终状态
        buttonImage.color = targetColor;
        if (buttonText != null)
        {
            buttonText.color = new Color(buttonText.color.r,
                                       buttonText.color.g,
                                       buttonText.color.b,
                                       1);
        }
    }
    private IEnumerator ScaleButton(Button button)
    {
        float elapsedTime = 0f;
        Vector3 startScale = Vector3.zero;

        while (elapsedTime < scaleDuration)
        {
            button.transform.localScale = Vector3.Lerp(startScale, targetScale, elapsedTime / scaleDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 确保最终状态
        button.transform.localScale = targetScale;
    }
    private IEnumerator AnimateButtonDisappearance(Button button)
    {
        // 并行执行淡出和缩放动画
        StartCoroutine(FadeOutButton(button));
        yield return StartCoroutine(ScaleDownButton(button));

        // 动画完成后禁用组件
        button.image.enabled = false;
        if (button.GetComponentInChildren<Text>() != null)
            button.GetComponentInChildren<Text>().enabled = false;
    }

    private IEnumerator FadeOutButton(Button button)
    {
        float elapsedTime = 0f;
        Image buttonImage = button.image;
        Text buttonText = button.GetComponentInChildren<Text>();

        Color startColor = buttonImage.color;
        Color targetColor = new Color(1, 1, 1, 0); // 完全透明

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            buttonImage.color = new Color(1, 1, 1, alpha);

            if (buttonText != null)
            {
                buttonText.color = new Color(buttonText.color.r,
                                           buttonText.color.g,
                                           buttonText.color.b,
                                           alpha);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 确保最终状态
        buttonImage.color = targetColor;
        if (buttonText != null)
        {
            buttonText.color = new Color(buttonText.color.r,
                                       buttonText.color.g,
                                       buttonText.color.b,
                                       0);
        }
    }

    private IEnumerator ScaleDownButton(Button button)
    {
        float elapsedTime = 0f;
        Vector3 startScale = button.transform.localScale;
        Vector3 targetScale = Vector3.zero;

        while (elapsedTime < scaleDuration)
        {
            button.transform.localScale = Vector3.Lerp(startScale, targetScale, elapsedTime / scaleDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 确保最终状态
        button.transform.localScale = targetScale;
    }

    private IEnumerator AnimateFade(Image image, float startAlpha, float targetAlpha, bool activeAfter)
    {
        // 设置初始状态
        Color color = image.color;
        color.a = startAlpha;
        image.color = color;
        if (activeAfter) image.gameObject.SetActive(true);
        if (enableScaleAnimation)
        {
            image.transform.localScale = new Vector3(
                activeAfter ? startImageScale : targetImageScale,
                activeAfter ? startImageScale : targetImageScale,
                1f
            );
        }

        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            // 计算当前进度
            float progress = elapsedTime / fadeDuration;

            // 淡入淡出
            color.a = Mathf.Lerp(startAlpha, targetAlpha, progress);
            image.color = color;

            // 缩放动画
            if (enableScaleAnimation)
            {
                float currentScale = Mathf.Lerp(
                    activeAfter ? startImageScale : targetImageScale,
                    activeAfter ? targetImageScale : startImageScale,
                    progress
                );
                image.transform.localScale = new Vector3(currentScale, currentScale, 1f);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 确保最终状态
        color.a = targetAlpha;
        image.color = color;

        if (enableScaleAnimation)
        {
            image.transform.localScale = new Vector3(
                activeAfter ? targetImageScale : startImageScale,
                activeAfter ? targetImageScale : startImageScale,
                1f
            );
        }

        // 淡出后禁用对象
        if (!activeAfter)
        {
            image.gameObject.SetActive(false);
        }

    }

    public void UseButBanButton(Button button)
    {
        UseButton(button);
        BanButton(button);
    }
    public void InitCloseImage(Image image)
    {
        image.gameObject.SetActive(false);
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
    }
    public void InitCloseButton(Button button)
    {
        button.interactable = false;
        button.image.enabled = false;
        if (button.GetComponentInChildren<Text>() != null)
            button.GetComponentInChildren<Text>().enabled = false;

    }
    public void BanButton(Button button)
    {
        button.interactable = false;
        button.image.color = Color.gray;
    }
    public void UnuseButton(Button button)
    {
        button.interactable = false;
        StartCoroutine(AnimateButtonDisappearance(button));
    }
    public void UseButton(Button button)
    {

        button.interactable = true;

        if (button.image.color != Color.gray) // 完全透明)
            StartCoroutine(AnimateButtonAppearance(button));
        else
        {
            button.enabled = true;
            button.image.color = new Color(1, 1, 1, 1);
        }
    }
    public void UseImage(Image image)
    {
        if (image.color.a == 0)
            StartCoroutine(AnimateFade(image, 0, 1, true));
    }
    public void UnuseImage(Image image)
    {
        if (image.color.a == 1)
            StartCoroutine(AnimateFade(image, 1, 0, false));
    }
}
