using UnityEngine;
using System.Collections; // --- 新增 ---：为了使用协程（Coroutines）

public class DraggableItem : MonoBehaviour
{
    private Vector3 offset;
    private Camera mainCamera;

    private Transform originalParent;
    private Vector3 originalLocalPosition;
    private bool isReturning = false;

    public bool isSuit = false; 
    public InventoryItemData inventoryItemData;
    public SpriteRenderer icon;

    void Awake()
    {
        mainCamera = Camera.main;
    }

    public void SetUp(InventoryItemData  _inventoryItemData)
    {
        inventoryItemData = _inventoryItemData;
        icon.sprite = inventoryItemData.closeUp;
    }

    void OnMouseDown()
    {
        // 如果物品正在返回，则不允许开始新的拖拽
        if (isReturning) return;

        StopAllCoroutines();

        originalParent = transform.parent;
        originalLocalPosition = transform.localPosition;

        offset = transform.position - GetMouseWorldPos();
 
        transform.SetParent(null); 
    }

    void OnMouseDrag()
    {

        if (isReturning) return;

        transform.position = GetMouseWorldPos() + offset;
    }
    
    void OnMouseUp()
    {

        if (originalParent == null) return;

        // -----------------------------------------------------------
        // ★★★ 这是你需要写逻辑的地方 ★★★
        //
        // 在这里，你需要检测鼠标松开时的位置
        // 比如用 Raycast 或 OnTriggerEnter 来判断是否在合法的放置区
        // 然后相应地设置 isSuit 的值。
        //
        // 示例: 
        // isSuit = CheckIfDropLocationIsValid(); 
        //
        // -----------------------------------------------------------


        if (isSuit == false)
        {

            StartCoroutine(SmoothReturnToOrigin());
        }
        else
        {
            // 放置成功！
            // 你需要在这里处理放置成功的逻辑
            // 比如，把它设置到一个新的父级（装备槽）
            // transform.SetParent(newSlotParent);
            // 或者，如果只是放回原处，就让它留在原处
            transform.SetParent(originalParent);
        }

        isSuit = false;
    }

    /// <summary>
    /// 丝滑返回原位的协程
    /// </summary>
    private IEnumerator SmoothReturnToOrigin()
    {
        isReturning = true;

        transform.SetParent(originalParent);

        Vector3 startLocalPosition = transform.localPosition;
        
        float duration = 0.25f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
   
            float t = elapsedTime / duration;
    
            t = 1 - Mathf.Pow(1 - t, 3); 

            transform.localPosition = Vector3.Lerp(startLocalPosition, originalLocalPosition, t);
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalLocalPosition;
        
        isReturning = false;
        originalParent = null; 
    }

    /// <summary>
    /// 获取鼠标世界坐标
    /// </summary>
    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        

        mousePoint.z = 10f;
        
        return mainCamera.ScreenToWorldPoint(mousePoint);
    }
}