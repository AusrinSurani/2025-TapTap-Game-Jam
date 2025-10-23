using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Map : MonoBehaviour
{
    [Header("移动设置")]
    [SerializeField] public Vector2 startPosition;
    [SerializeField] public Vector2 targetPosition;
    [SerializeField] public float duration = 1.0f;
    [SerializeField] public float delay = 0.0f;
    [SerializeField] public bool useEasing = true;

    private Transform playerTrans;
    private Coroutine moveCoroutine;

    void Awake()
    {
        playerTrans = GetComponentInParent<PlayerController>().transform;
    }
    
    public void StartMove()
    {
        gameObject.SetActive(true);
        //终断当前的移动
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        moveCoroutine = StartCoroutine(MoveRoutine(
            startPosition + new Vector2(playerTrans.position.x, playerTrans.position.y), 
            targetPosition + new Vector2(playerTrans.position.x, playerTrans.position.y), 
            false));
    }

    public virtual void MoveBack(bool isVanish = false)
    {
        Vector2 currentPos = transform.position;
        Vector2 destination = startPosition + new Vector2(playerTrans.position.x, playerTrans.position.y);

        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine = StartCoroutine(MoveRoutine(currentPos, destination, isVanish));
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
            transform.position = Vector2.Lerp(start, end, t);

            yield return null;
        }
        transform.position = end;
        moveCoroutine = null;
        gameObject.SetActive(!disable);
    }
}
