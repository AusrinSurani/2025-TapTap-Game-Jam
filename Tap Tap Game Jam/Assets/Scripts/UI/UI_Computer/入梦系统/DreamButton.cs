using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DreamButton : BounceButton
{
    private Animator animator;
    private bool isOpenWindows = false;
    
    public GameObject windows;
    public GameObject darkMask;
    public Animator machineAnimator;
    
    [Header("表格")]
    public DreamSystemConfig dreamSystemConfig;
    public TextMeshProUGUI nameOfPatient;
    public TextMeshProUGUI keyWords;
    public TextMeshProUGUI initialDiagnosis;
    public TextMeshProUGUI advice;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rectTransform = GetComponent<RectTransform>();
        windows.GetComponent<CanvasGroup>().blocksRaycasts = false;
        
        nameOfPatient.text = dreamSystemConfig.nameOfPatient;
        keyWords.text = dreamSystemConfig.keyWords;
        initialDiagnosis.text = dreamSystemConfig.initialDiagnosis;
        advice.text = dreamSystemConfig.advice;
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

    public void LoadNextLevel()
    {
        StartCoroutine(BeforeLoadCoroutine());
    }

    private IEnumerator BeforeLoadCoroutine()
    {
        yield return DarkMaskCoroutine(false);
        animator.SetBool("Open", false);
        isOpenWindows = false;
        windows.GetComponent<CanvasGroup>().blocksRaycasts = false;

        GetComponentInParent<UI_Computer>().MoveBack(false);
        yield return new WaitForSeconds(0.5f);

        machineAnimator.SetTrigger("Lower");

        yield return new WaitForSeconds(2.6f);
        SceneLoadManager.Instance.ResetSceneLoadStatus();
        SceneLoadManager.Instance.TryLoadToTargetSceneAsync(dreamSystemConfig.idOfSceneToLoad, "入梦", true);
    }
}   