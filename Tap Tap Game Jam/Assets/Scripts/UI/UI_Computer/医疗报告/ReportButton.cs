using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ReportButton : BounceButton
{
    [Header("事件监听")]
    public VoidEventSO chapterChangeEvent;
    
    private Animator animator;
    private bool isOpenWindows = false;
    
    public GameObject windows;
    public GameObject darkMask;
    
    [Header("信息控制")] 
    public int currentInformation = 0;
    public ReportInformation[] informationList;
    public TextMeshProUGUI informationText;
    public TMP_Dropdown dropdown;
    
    private void OnEnable()
    {
        chapterChangeEvent.OnEventRaise += RefreshInformation;
    }

    public void OnDisable()
    {
        chapterChangeEvent.OnEventRaise -= RefreshInformation;
    }
    
    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rectTransform = GetComponent<RectTransform>();
        windows.GetComponent<CanvasGroup>().blocksRaycasts = false;
        
        RefreshInformation();
    }

    private void Update()
    {
        
    }

    private void RefreshInformation()
    {
        //只有治疗结束了，才有医疗报告
        if(!GameFlowManager.Instance.currentIsOver)
        { 
            currentInformation = 0;
            
            informationText.text = informationList[currentInformation].information;
            dropdown.ClearOptions();
            dropdown.AddOptions(informationList[currentInformation].options);
            
            return;
        }
        
        switch (GameFlowManager.Instance.currentChapter)
        {
            case ChapterOfGame.NoOne:
                currentInformation = 0;
                break;
            case ChapterOfGame.ChapterDancer:
                currentInformation = 1;
                break;
            case ChapterOfGame.ChapterWaiter:
                currentInformation = 2;
                break;
            case ChapterOfGame.ChapterProgrammer:
                currentInformation = 3;
                break;
            default:
                currentInformation = 0;
                break;
        }
        
        informationText.text = informationList[currentInformation].information;
        dropdown.ClearOptions();
        dropdown.AddOptions(informationList[currentInformation].options);
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
    
    //按下提交患者诊断报告进入下一天
    public void Submit()
    {
        //如果没有治疗结束或者没有选择最终诊断，则无法提交
        if (!GameFlowManager.Instance.currentIsOver || dropdown.value == 0)
        {
            DialogManager.Instance.ShowMessage("治疗还未开始，或者没有选择最终诊断");
            return;
        }
        
        GameFlowManager gameFlowManager = GameFlowManager.Instance;

        gameFlowManager.ChangeChapter(ChapterOfGame.NoOne, false, gameFlowManager.currentDay + 1);
        StartCoroutine(AfterSubmit());
    }

    private IEnumerator AfterSubmit()
    {
        UIManager.Instance.coverFader.gameObject.SetActive(true);
        CloseWindows();
        GetComponentInParent<UI_Computer>().MoveBack(false);
        yield return UIManager.Instance.coverFader.FadeIn();
        yield return UIManager.Instance.coverFader.FadeOut();
        UIManager.Instance.coverFader.gameObject.SetActive(false);
    }
}