using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EmailButton : BounceButton
{
    private bool isOpenWindows = false;
    
    [Header("窗口")]
    public Animator animator;
    public GameObject windows;
    public GameObject darkMask;
    
    [Header("新消息")]
    public Animator newEmail;
    public bool haveNewEmail = false;

    public override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        windows.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    private void Update()
    {

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
        
        StartCoroutine(DarkMaskCoroutine(true));
        animator.SetBool("Open", true);
        AnimateScale(originalScale);
        isOpenWindows = true;
        windows.GetComponent<CanvasGroup>().blocksRaycasts = true;
        rectTransform.SetAsLastSibling();
    }

    public void CloseWindows()
    {
        StartCoroutine(DarkMaskCoroutine(false));
        animator.SetBool("Open", false);
        isOpenWindows = false;
        windows.GetComponent<CanvasGroup>().blocksRaycasts = false;
        rectTransform.SetAsFirstSibling();
    }
    
    private IEnumerator DarkMaskCoroutine(bool isDark)
    {
        float timer = 0f;

        while (timer < 0.35f)
        {
            timer += Time.deltaTime;
            darkMask.GetComponent<CanvasGroup>().alpha = 
                isDark ? Mathf.Lerp(0, 0.93f,timer/0.35f): Mathf.Lerp(0.93f,0, timer / 0.35f);
        }
        yield return null;
    }
}
