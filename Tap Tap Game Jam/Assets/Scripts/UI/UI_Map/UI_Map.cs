using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Map : MonoBehaviour
{
    [Header("事件广播")]
    public VoidEventSO get7MapsEvent;
    
    [Header("移动设置")]
    [SerializeField] public Vector2 startPosition;
    [SerializeField] public Vector2 targetPosition;
    [SerializeField] public float duration = 1.0f;
    [SerializeField] public float delay = 0.0f;
    [SerializeField] public bool useEasing = true;

    [Header("进展")] public PolygonCollider2D bedCollider;
    public GameObject mapItem;
    public GameObject bird;
    public int numOfMap = 0;
    public int numOfFlag = 0;
    public GameObject breman;
    
    private bool haveShowMessage = false;
    private bool haveShowBird = false;
    private bool haveGone = false;
    
    public Transform playerTrans;
    private Coroutine moveCoroutine;

    public GameObject button;

    private void Update()
    {
        if (numOfFlag == 7 && !haveShowBird)
        {
            bird.SetActive(true);
            haveShowBird = true;
            AudioManager.Instance.AudioOncePlay(AudioManager.Instance.getMap);
        }
        
        if (haveGone)
        {
            return;
        }
        
        if (numOfFlag == 7 && numOfMap == 7 && !haveGone)
        {
            haveGone = true;
            StartCoroutine(BeforeVanish());
            return;
        }
        
        if (numOfMap == 7 && !haveShowMessage)
        {
            get7MapsEvent.RaiseEvent();
            haveShowMessage = true;
            string[] message = {"好像还缺了点啥","七个地方，只有六个旅行计划？但是基本能找的地方都找过了。","去问问布莱梅看看吧"};
            DialogManager.Instance.ShowMessage(message);

            StartCoroutine(KeepPlayerIdle());
        }
    }

    private IEnumerator KeepPlayerIdle()
    {
        yield return new WaitUntil(()=> playerTrans.GetComponent<PlayerController>().BNoGetInput == false);
        playerTrans.GetComponent<PlayerController>().BNoGetInput = true;
        playerTrans.GetComponent<PlayerController>().SetZeroVelocity();
        playerTrans.GetComponent<PlayerController>().SetInputZero();
    }
    
    private IEnumerator BeforeVanish()
    {
        button.SetActive(false);
        yield return new WaitForSeconds(1f);
        
        Exit();
        mapItem.SetActive(false);

        string[] message = new []{"下雨了","回去的时候，去威尔逊大饭店喝一杯吧。"};
        DialogManager.Instance.ShowMessage(message);
        
        yield return new WaitUntil(() => DialogManager.Instance.dialogBox.activeSelf == false);
        yield return new WaitForSeconds(0.7f);
        
        AudioManager.Instance.ClearTargetAudioPiece(AudioManager.Instance.hotelBGM);
        AudioManager.Instance.AudioLoopPlay(AudioManager.Instance.consultingBGM);
        
        SceneLoadManager.Instance.ResetSceneLoadStatus();
        GameFlowManager.Instance.ChangeChapter(GameFlowManager.Instance.currentChapter, true, GameFlowManager.Instance.currentDay);
        SceneLoadManager.Instance.TryLoadToTargetSceneAsync(SceneLoadManager.SceneDisplayID.ConsultationRoom, "", false);
    }

    public void StartMove()
    {
        AudioManager.Instance.AudioOncePlay(AudioManager.Instance.raiseMap);
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
        AudioManager.Instance.AudioOncePlay(AudioManager.Instance.exitMap);
        
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

    public void Exit()
    {
        bedCollider.enabled = true;
        playerTrans.GetComponent<PlayerController>().BNoGetInput = false;
        MoveBack(false);
    }
}