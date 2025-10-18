using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InteractableInDream : Interactable
{
    [Header("显示物品特写的UI")]
    public GameObject imageForItem;
    private CanvasGroup canvasGroup;
    private bool isShowing = false;
    
    [Header("切换色调的背景")]
    public GameObject background;
    private Animator animator;
    
    [SerializeField]private float raiseDuration;
    [SerializeField]private float backDuration;

    private void Awake()
    {
        
    }

    public override void Start()
    {
        base.Start();
        canvasGroup = imageForItem.GetComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0;
        
        animator = background.GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isShowing)
            {
                //配合文本一起消失
                canvasGroup.blocksRaycasts = false;
                StartCoroutine(FadeCoroutine(1, 0, backDuration));
                animator.SetBool("Darker",false);
                isShowing = false;
            }
        }
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        
        //显示特写图
        imageForItem.GetComponent<Image>().sprite = itemData.closeUp;
        StartCoroutine(FadeCoroutine(0, 1, raiseDuration));
        imageForItem.GetComponent<CanvasGroup>().blocksRaycasts = true;
        animator.SetBool("Darker",true);
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
