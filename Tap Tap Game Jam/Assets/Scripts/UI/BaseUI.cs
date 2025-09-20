using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUI : MonoBehaviour
{
    [Header("移动设置")]
    //这是一个Anchored Position
    [SerializeField] public Vector2 startPosition;
    [SerializeField] public Vector2 targetPosition;
    [SerializeField] public float duration = 1.0f;
    [SerializeField] public float delay = 0.0f;
    [SerializeField] public bool useEasing = true;

    
    private RectTransform rectTransform;
    private Coroutine moveCoroutine;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    
    public void StartMove()
    {
        gameObject.SetActive(true);
        // 如果已经有一个移动正在进行，先停止它
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        moveCoroutine = StartCoroutine(MoveRoutine(startPosition, targetPosition, false));
    }
    

    //将UI元素从当前位置移动回其原始起点
    public void MoveBack()
    {
        Vector2 currentPos = rectTransform.anchoredPosition;
        Vector2 destination = startPosition;

        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine = StartCoroutine(MoveRoutine(currentPos, destination, true));
    }

    
    private IEnumerator MoveRoutine(Vector2 start, Vector2 end, bool disable)
    {
        if (delay > 0)
        {
            yield return new WaitForSeconds(delay);
        }

        float elapsedTime = 0f;
        
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            float t = elapsedTime / duration;
            
            if (useEasing)
            {
                t = Mathf.SmoothStep(0f, 1f, t);
            }
            rectTransform.anchoredPosition = Vector2.Lerp(start, end, t);

            yield return null;
        }
        rectTransform.anchoredPosition = end;
        moveCoroutine = null;
        gameObject.SetActive(!disable);
    }
}
