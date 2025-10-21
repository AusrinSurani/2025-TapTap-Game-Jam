using UnityEngine;

public class InventoryScroller : MonoBehaviour
{
    [Header("滚动设置")]
    public Transform container;
    public float scrollSpeed = 5f;
    public float smoothness = 0.1f;

    [Header("边界限制 (局部坐标)")]
    public float minY_local = -5f; // Container相对于其父级的Y坐标下限
    public float maxY_local = 5f;  // Container相对于其父级的Y坐标上限

    private Camera mainCamera;
    private Vector3 lastMousePosition;
    private bool isDraggingBackground = false;

    private Vector3 targetLocalPosition;

    void Awake()
    {
        mainCamera = Camera.main;
        if (container != null)
        {
            // 初始化目标位置为容器的初始局部位置
            targetLocalPosition = container.localPosition;
        }
    }

    void Update()
    {
        HandleInput();

        if (container != null)
        {
            // ★★★ 核心改动：插值计算局部坐标 ★★★
            container.localPosition = Vector3.Lerp(container.localPosition, targetLocalPosition, Time.deltaTime / smoothness);
        }
    }

    void HandleInput()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scrollInput) > 0.01f)
        {
            // 只更新目标局部位置的Y值
            targetLocalPosition.y -= scrollInput * scrollSpeed;
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(GetMouseWorldPos(), Vector2.zero);
            if (hit.collider == null) return;

            if (hit.collider.CompareTag("InventoryItem"))
            {
                isDraggingBackground = false;
                return;
            }
            
            if (hit.collider == GetComponent<Collider2D>())
            {
                isDraggingBackground = true;
                lastMousePosition = GetMouseWorldPos();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDraggingBackground = false;
        }

        if (isDraggingBackground && Input.GetMouseButton(0))
        {
            Vector3 delta = GetMouseWorldPos() - lastMousePosition;
            // 拖拽的垂直偏移量可以直接加到局部坐标的Y值上
            targetLocalPosition.y += delta.y;
            lastMousePosition = GetMouseWorldPos();
        }

        // ★★★ 核心改动：在局部坐标系下限制边界 ★★★
        targetLocalPosition.y = Mathf.Clamp(targetLocalPosition.y, minY_local, maxY_local);
    }

    private Vector3 GetMouseWorldPos()
    {
        return mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }
}