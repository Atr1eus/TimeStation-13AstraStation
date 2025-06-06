using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SceneManager : MonoBehaviour
{

    [SerializeField] protected Text currentRoundText;
    [SerializeField] protected Text diskText;
    [SerializeField] protected Text positionText;
    [SerializeField] protected Text customerText;

    protected virtual void Start()
    {
        if (currentRoundText != null)
            currentRoundText.text = RoundManager.Instance.currentRound.ToString();
    }
    protected virtual void Update()
    {
        if (diskText != null)
            diskText.text = PlayerController.Instance.gold.ToString() + "/" + PlayerController.Instance.roundLimitMoney[RoundManager.Instance.currentRound].ToString();
    }

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
    [Header("交叉过渡设置")]
    [SerializeField] private float crossFadeDuration = 1.0f;
    [SerializeField] private float moveUpDistance = 100f;
    [SerializeField] private AnimationCurve crossFadeCurve = AnimationCurve.Linear(0, 0, 1, 1);

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
    [Header("升级动画设置")]
    [SerializeField] private float upgradeAnimDuration = 0.5f; // 动画持续时间
    [SerializeField] private float upgradeMoveDistance = 50f; // 上移距离
    [SerializeField] private AnimationCurve upgradeFadeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1); // 动画曲线

    // 获取或添加CanvasGroup组件
    private CanvasGroup EnsureCanvasGroup(GameObject obj)
    {
        var group = obj.GetComponent<CanvasGroup>();
        if (group == null) group = obj.AddComponent<CanvasGroup>();
        return group;
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

    public void StartCrossFade(GameObject fadingOutObject, GameObject fadingInObject)
    {
        StartCoroutine(CrossFadeAnimation(fadingOutObject, fadingInObject));
    }

    public IEnumerator CrossFadeAnimation(GameObject fadingOutObj, GameObject fadingInObj)
    {
        // 确保初始状态正确
        fadingInObj.SetActive(true);

        // 将渐显对象置于渐隐对象下方
        fadingInObj.transform.SetAsFirstSibling();

        // 获取或添加必要的组件
        CanvasGroup fadeOutGroup = GetOrAddCanvasGroup(fadingOutObj);
        CanvasGroup fadeInGroup = GetOrAddCanvasGroup(fadingInObj);

        // 初始状态设置
        fadeOutGroup.alpha = 1f;
        fadeInGroup.alpha = 0f;
        Vector3 fadeOutStartPos = fadingOutObj.transform.localPosition;
        Vector3 fadeOutTargetPos = fadeOutStartPos + Vector3.up * moveUpDistance;

        float elapsedTime = 0f;

        while (elapsedTime < crossFadeDuration)
        {
            float progress = elapsedTime / crossFadeDuration;
            float curveProgress = crossFadeCurve.Evaluate(progress);

            // 处理渐隐对象
            fadeOutGroup.alpha = 1f - curveProgress;
            fadingOutObj.transform.localPosition = Vector3.Lerp(
                fadeOutStartPos,
                fadeOutTargetPos,
                curveProgress
            );

            // 处理渐显对象
            fadeInGroup.alpha = curveProgress;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 确保最终状态
        fadeOutGroup.alpha = 0f;
        fadeInGroup.alpha = 1f;
        fadingOutObj.transform.localPosition = fadeOutTargetPos;
        fadingOutObj.SetActive(false);
    }

    private CanvasGroup GetOrAddCanvasGroup(GameObject obj)
    {
        CanvasGroup group = obj.GetComponent<CanvasGroup>();
        if (group == null)
        {
            group = obj.AddComponent<CanvasGroup>();
        }
        return group;
    }

    public IEnumerator PlayUpgradeAnimation(GameObject currentObj, GameObject nextObj)
    {
        // 确保所有子UI激活
        foreach (var text in nextObj.GetComponentsInChildren<Text>(true))
        {
            text.gameObject.SetActive(true);
        }

        // 设置层级关系
        nextObj.transform.SetAsFirstSibling();
        nextObj.SetActive(true);

        // 获取CanvasGroup组件
        CanvasGroup currentGroup = EnsureCanvasGroup(currentObj);
        CanvasGroup nextGroup = EnsureCanvasGroup(nextObj);

        // 初始状态
        currentGroup.alpha = 1f;
        nextGroup.alpha = 0f;
        Vector3 startPos = currentObj.transform.localPosition;

        float timer = 0f;
        while (timer < upgradeAnimDuration)
        {
            float progress = upgradeFadeCurve.Evaluate(timer / upgradeAnimDuration);

            // 当前对象上移+淡出
            currentGroup.alpha = 1 - progress;
            currentObj.transform.localPosition = startPos + Vector3.up * (upgradeMoveDistance * progress);

            // 新对象淡入
            nextGroup.alpha = progress;

            timer += Time.deltaTime;
            yield return null;
        }

        // 最终状态
        currentGroup.alpha = 0f;
        nextGroup.alpha = 1f;
        currentObj.transform.localPosition = startPos + Vector3.up * upgradeMoveDistance;
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
    public void InitUseImage(Image image)
    {
        image.gameObject.SetActive(true);
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
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
    public void WhiteBanButton(Button button)
    {
        button.interactable = false;
    }
    public void UnuseButton(Button button)
    {
        button.interactable = false;
        StartCoroutine(AnimateButtonDisappearance(button));
    }
    public void UseButton(Button button)
    {
        if (button.image.color != Color.gray && button.interactable == false) // 完全透明)
            StartCoroutine(AnimateButtonAppearance(button));
        else
        {
            button.enabled = true;
            button.image.color = new Color(1, 1, 1, 1);
        }

        button.interactable = true;

    }
    public void InitUseButton(Button button)
    {
        button.enabled = true;
        button.image.color = new Color(1, 1, 1, 1);
        button.interactable = true;
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
