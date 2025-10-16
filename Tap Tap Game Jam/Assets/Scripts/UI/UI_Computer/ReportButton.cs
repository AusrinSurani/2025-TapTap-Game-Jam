using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ReportButton : BounceButton
{
    private Animator animator;
    private bool isOpenWindows = false;
    
    public GameObject windows;
    public GameObject backButton;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rectTransform = GetComponent<RectTransform>();
        windows.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    private void Update()
    {
        backButton.SetActive(isOpenWindows);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (isOpenWindows)
            return;
        base.OnPointerEnter(eventData);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if (isOpenWindows)
            return;
        base.OnPointerExit(eventData);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (isOpenWindows)
            return;
        
        base.OnPointerClick(eventData);
        
        animator.SetBool("Open", true);
        AnimateScale(originalScale);
        isOpenWindows = true;
        windows.GetComponent<CanvasGroup>().blocksRaycasts = true;
        rectTransform.SetAsLastSibling();
    }

    public void CloseWindows()
    {
        animator.SetBool("Open", false);
        isOpenWindows = false;
        windows.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
}
