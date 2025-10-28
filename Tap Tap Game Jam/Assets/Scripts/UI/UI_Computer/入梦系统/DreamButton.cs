using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DreamButton : BounceButton
{
    [Header("事件监听")] public VoidEventSO chapterChangeEvent;

    [Header("要关闭的指引")] public GameObject guide;
    
    private Animator animator;
    private bool isOpenWindows = false;
    
    public GameObject windows;
    public GameObject darkMask;
    public Animator machineAnimator;
    
    [Header("表格")]
    public int currentConfig = 0;
    public SceneLoadManager.SceneDisplayID sceneToLoad;
    public DreamSystemConfig[] dreamSystemConfig;
    public TextMeshProUGUI nameOfPatient;
    public TextMeshProUGUI keyWords;
    public TextMeshProUGUI initialDiagnosis;
    public TextMeshProUGUI advice;
    
    [Header("第二章的初始剧情")]
    public TextAsset chapter2;
    
    private void OnEnable()
    {
        chapterChangeEvent.OnEventRaise += RefreshData;
    }

    public void OnDisable()
    {
        chapterChangeEvent.OnEventRaise -= RefreshData;
    }
    
    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rectTransform = GetComponent<RectTransform>();
        windows.GetComponent<CanvasGroup>().blocksRaycasts = false;

        RefreshData();
    }

    private void RefreshData()
    {
        switch (GameFlowManager.Instance.currentChapter)
        {
            case ChapterOfGame.NoOne:
                currentConfig = 0;
                break;
            case ChapterOfGame.ChapterDancer:
                currentConfig = 1;
                break;
            case ChapterOfGame.ChapterWaiter:
                currentConfig = 2;
                break;
            case ChapterOfGame.ChapterProgrammer:
                currentConfig = 3;
                break;
            default:
                currentConfig = 0;
                break;
        }
        
        Debug.Log("Config更新：" + currentConfig);
        sceneToLoad = dreamSystemConfig[currentConfig].idOfSceneToLoad;
        
        nameOfPatient.text = dreamSystemConfig[currentConfig].nameOfPatient;
        keyWords.text = dreamSystemConfig[currentConfig].keyWords;
        initialDiagnosis.text = dreamSystemConfig[currentConfig].initialDiagnosis;
        advice.text = dreamSystemConfig[currentConfig].advice;
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
        //如果没有患者，或者说患者治疗结束了，都不能再入梦了
        if (GameFlowManager.Instance.currentChapter == ChapterOfGame.NoOne)
        {
            DialogManager.Instance.ShowMessage("问诊结束后才能进行入梦治疗，现在还不是时候");
            return;
        }
        else if(GameFlowManager.Instance.currentIsOver&& GameFlowManager.Instance.currentChapter != ChapterOfGame.NoOne)
        {
            DialogManager.Instance.ShowMessage("治疗已经结束了，不需要再入梦了");
            return;
        }
        
        guide.SetActive(false);
        AudioManager.Instance.PauseTargetAudioPiece(AudioManager.Instance.consultingBGM);
        StartCoroutine(LoadCoroutine());
    }

    private IEnumerator LoadCoroutine()
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

        if (sceneToLoad == SceneLoadManager.SceneDisplayID.WaiterDream)
        {
            StartCoroutine(LoadToChapter2());
        }
        else
        {
            SceneLoadManager.Instance.TryLoadToTargetSceneAsync(sceneToLoad, "入梦", true);
        }
    }

    private IEnumerator LoadToChapter2()
    {
        SceneLoadManager.Instance.TryLoadToTargetSceneAsync(sceneToLoad, "入梦", true);

        yield return new WaitForSeconds(0.5F);
        DialogManager.Instance.StartDialog(chapter2);
    }
}   