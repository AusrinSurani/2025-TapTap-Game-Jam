using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUI : MonoBehaviour
{
    [Header("移动设置")]
    //这是Anchored Position
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
        //终断当前的移动
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        moveCoroutine = StartCoroutine(MoveRoutine(startPosition, targetPosition, false));
    }
    
    //UI移动回去
    public virtual void MoveBack(bool isVanish = true)
    {
        Vector2 currentPos = rectTransform.anchoredPosition;
        Vector2 destination = startPosition;

        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine = StartCoroutine(MoveRoutine(currentPos, destination, isVanish));
    }

    //平滑移动UI的协程
    public IEnumerator MoveRoutine(Vector2 start, Vector2 end, bool disable)
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
