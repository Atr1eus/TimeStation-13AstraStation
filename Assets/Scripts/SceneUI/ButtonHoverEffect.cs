using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

[RequireComponent(typeof(EventTrigger))]
public class ButtonHoverEffect : MonoBehaviour
{
    [SerializeField] private RectTransform restImage;
    [SerializeField] private float moveDistance = 50f;
    [SerializeField] private float moveDuration = 0.3f;
    [SerializeField] private Ease easeType = Ease.OutQuad;

    private Vector2 originalPos;
    private Tween currentTween; // 跟踪当前动画

    void Start()
    {
        if (!TryGetComponent<EventTrigger>(out _))
            gameObject.AddComponent<EventTrigger>();

        originalPos = restImage.anchoredPosition;
        SetupEventTriggers();
    }

    private void SetupEventTriggers()
    {
        var trigger = GetComponent<EventTrigger>();

        var enterEntry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerEnter,
            callback = new EventTrigger.TriggerEvent()
        };
        enterEntry.callback.AddListener(_ => OnPointerEnter());

        var exitEntry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerExit,
            callback = new EventTrigger.TriggerEvent()
        };
        exitEntry.callback.AddListener(_ => OnPointerExit());

        trigger.triggers.Add(enterEntry);
        trigger.triggers.Add(exitEntry);
    }

    private void OnPointerEnter()
    {
        // 立即终止之前的动画
        currentTween?.Kill();

        currentTween = restImage.DOAnchorPosX(
            originalPos.x + moveDistance,
            moveDuration
        )
        .SetEase(easeType)
        .SetAutoKill(true);
    }

    private void OnPointerExit()
    {
        // 立即终止之前的动画
        currentTween?.Kill();

        currentTween = restImage.DOAnchorPosX(
            originalPos.x,
            moveDuration
        )
        .SetEase(easeType)
        .SetAutoKill(true);
    }

    void OnDestroy()
    {
        currentTween?.Kill();
    }
}