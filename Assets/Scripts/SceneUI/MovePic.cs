using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MovePic : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    public int maxIndex;
    private Image img;
    private RectTransform rectTransform;
    private Canvas canvas;
    private Camera canvasCamera;
    private Canvas parentCanvas; // 新增：用于获取父Canvas
    Vector2 offsetPos;

    void Start()
    {
        img = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasCamera = canvas.worldCamera;
        parentCanvas = GetComponentInParent<Canvas>(); // 初始化父Canvas
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 拖动时降低层级
        UpperSiblingIndex();

        if (canvas.renderMode != RenderMode.ScreenSpaceCamera || canvasCamera == null)
        {
            rectTransform.position = new Vector3(
                Mathf.Clamp(Input.mousePosition.x, 0, Screen.width),
                Mathf.Clamp(Input.mousePosition.y, 0, Screen.height),
                0) + (Vector3)offsetPos;
        }
        else
        {
            Vector2 localPointerPosition;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform.parent as RectTransform,
                eventData.position,
                canvasCamera,
                out localPointerPosition))
            {
                rectTransform.localPosition = localPointerPosition + offsetPos;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // 点击时也降低层级（可选）
        UpperSiblingIndex();

        if (canvas.renderMode != RenderMode.ScreenSpaceCamera || canvasCamera == null)
        {
            offsetPos = (Vector2)(rectTransform.position) - (Vector2)Input.mousePosition;
        }
        else
        {
            Vector2 localPointerPosition;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform.parent as RectTransform,
                eventData.position,
                canvasCamera,
                out localPointerPosition))
            {
                offsetPos = (Vector2)rectTransform.localPosition - localPointerPosition;
            }
        }
    }

    // 新增方法：提高UI元素的层级
    private void UpperSiblingIndex()
    {
        if (rectTransform.parent != null)
        {
            int currentIndex = rectTransform.GetSiblingIndex();
            if (currentIndex < maxIndex) // 确保不会降到负数
            {
                rectTransform.SetSiblingIndex(currentIndex + 1);
            }
        }
    }
}