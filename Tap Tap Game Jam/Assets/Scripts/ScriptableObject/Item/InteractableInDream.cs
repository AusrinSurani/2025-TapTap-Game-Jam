using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InteractableInDream : Interactable
{
    public GameObject dialogMask;
    
    [Header("显示物品特写的UI")]
    public GameObject imageForItem;
    private CanvasGroup canvasGroup;
    private bool isShowing = false;

    [Header("切换色调的背景")] 
    public bool needSwitch = true;
    public GameObject background;
    private Animator animator;
    public bool BNoAnimator;
    
    public float raiseDuration;
    public float backDuration;

    public override void Start()
    {
        base.Start();
        dialogMask = DialogManager.Instance.mask;
        
        canvasGroup = imageForItem.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;

        if (needSwitch)
        {
            if (!BNoAnimator)
                animator = background?.GetComponent<Animator>();
        }
    }

    public virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(DialogManager.Instance.IsTyping())
                return;
            
            if (isShowing)
            {
                if (itemData.closeUp != null)
                {
                    //配合文本一起消失
                    canvasGroup.blocksRaycasts = false;
                    StartCoroutine(FadeCoroutine(1, 0, backDuration));
                }
                
                if (needSwitch)
                {
                    if (!BNoAnimator)
                        animator.SetBool("Darker",false);
                    else
                        background?.gameObject.SetActive(false);
                }
                
                isShowing = false;
                OnInteractFinished?.Invoke();
            }
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if(dialogMask.activeSelf == false)
            return;
        
        base.OnPointerEnter(eventData);
    }

    public UnityEvent OnInteractFinished;
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (dialogMask.activeSelf == false)
        {
            return;
        }
        
        base.OnPointerClick(eventData);
        
        //显示特写图
        if (itemData.closeUp != null)
        {
            imageForItem.SetActive(true);
            imageForItem.GetComponent<Image>().sprite = itemData.closeUp;
            StartCoroutine(FadeCoroutine(0, 1, raiseDuration));
        }
        
        if (needSwitch)
        {
            if (!BNoAnimator)
                animator.SetBool("Darker",true);
            else 
                background?.gameObject.SetActive(true);
        }
        
        isShowing = true;
    }

    private IEnumerator FadeCoroutine(float start, float end, float duration)
    {
        canvasGroup.alpha = 0;
        float timer = 0;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, end, timer / duration);
            yield return null;
        }
    }
}
