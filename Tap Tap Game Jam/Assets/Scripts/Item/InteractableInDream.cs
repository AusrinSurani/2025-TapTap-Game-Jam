using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InteractableInDream : Interactable
{
    public GameObject imageForItem;
    private CanvasGroup canvasGroup;
    private bool isShowing = false;
    [SerializeField]private float duration;

    private void Awake()
    {
        
    }

    public override void Start()
    {
        base.Start();
        canvasGroup = imageForItem.GetComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isShowing)
            {
                //配合文本一起消失
                canvasGroup.blocksRaycasts = false;
                StartCoroutine(FadeCoroutine(1, 0));
                isShowing = false;
            }
        }
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        
        //显示特写图
        imageForItem.GetComponent<Image>().sprite = itemData.closeUp;
        StartCoroutine(FadeCoroutine(0, 1));
        imageForItem.GetComponent<CanvasGroup>().blocksRaycasts = true;
        isShowing = true;
    }

    private IEnumerator FadeCoroutine(float start, float end)
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
