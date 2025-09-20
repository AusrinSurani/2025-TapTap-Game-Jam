using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUI : MonoBehaviour
{
    [Header("�ƶ�����")]
    //����һ��Anchored Position
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
        // ����Ѿ���һ���ƶ����ڽ��У���ֹͣ��
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        moveCoroutine = StartCoroutine(MoveRoutine(startPosition, targetPosition, false));
    }
    

    //��UIԪ�شӵ�ǰλ���ƶ�����ԭʼ���
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
