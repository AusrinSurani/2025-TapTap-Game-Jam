using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ScrollRect))]
public class SmoothScrollRect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,IPointerDownHandler, IPointerUpHandler
{
    private ScrollRect scrollRect;
    [SerializeField]private bool isMouseOver = false;
    [SerializeField]private bool isDragging = false;

    [Tooltip("滚动速度")]
    public float scrollSpeed = 70f;

    [Tooltip("平滑系数")]
    public float smoothFactor = 0.1f;

    private float targetVerticalPosition = 1f;
    
    void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
        // 禁用 ScrollRect 自带的滚动，但保留拖拽功能
        scrollRect.scrollSensitivity = 0;
        targetVerticalPosition = scrollRect.verticalNormalizedPosition;
    }

    void Update()
    {
        if (isDragging)
        {
            targetVerticalPosition = scrollRect.verticalNormalizedPosition;
            return;
        }
        
        if (isMouseOver)
        {
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");

            if (Mathf.Abs(scrollInput) > 0.01f)
            {
                // 注意：滚轮向上是正值，对应位置向上移动，所以是加
                targetVerticalPosition += scrollInput * scrollSpeed * Time.unscaledDeltaTime;
                targetVerticalPosition = Mathf.Clamp01(targetVerticalPosition);
            }
        }

        // 使用 Lerp 平滑地移动到目标位置
        scrollRect.verticalNormalizedPosition = Mathf.Lerp(
            scrollRect.verticalNormalizedPosition,
            targetVerticalPosition,
            smoothFactor
        );
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOver = true;
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOver = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
    }

    public void OnHandleWorking()
    {
        isDragging = true;
    }

    public void OnHandleDontWork()
    {
        isDragging = false;
    }
}