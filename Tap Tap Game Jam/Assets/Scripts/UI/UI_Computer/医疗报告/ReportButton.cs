using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ReportButton : BounceButton
{
    [Header("事件监听")] public VoidEventSO chapterChangeEvent;

    private Animator animator;
    private bool isOpenWindows = false;
    private bool haveChoose = false;

    public GameObject windows;
    public GameObject darkMask;

    [Header("信息控制")] public int currentInformation = 0;
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
        dropdown.onValueChanged.AddListener(ClearOtherOptions);

        animator = GetComponentInChildren<Animator>();
        rectTransform = GetComponent<RectTransform>();
        windows.GetComponent<CanvasGroup>().blocksRaycasts = false;

        RefreshInformation();
    }

    private void ClearOtherOptions(int index)
    {
        SetMaterialWrongOnce();
        var keep = dropdown.options[1];
        dropdown.ClearOptions();
        dropdown.options.Add(keep);
        dropdown.value = 0;
        dropdown.RefreshShownValue();
        haveChoose = true;
    }

    private void RefreshInformation()
    {
        //只有治疗结束了，才有医疗报告
        if (!GameFlowManager.Instance.currentIsOver)
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
                isDark ? Mathf.Lerp(0, 0.93f, timer / 0.35f) : Mathf.Lerp(0.93f, 0, timer / 0.35f);
        }

        yield return null;
    }

    //按下提交患者诊断报告进入下一天
    public void Submit()
    {
        //如果没有治疗结束或者没有选择最终诊断，则无法提交
        if (!GameFlowManager.Instance.currentIsOver || !haveChoose)
        {
            DialogManager.Instance.ShowMessage("治疗还未开始，或者没有选择最终诊断");
            return;
        }

        GameFlowManager gameFlowManager = GameFlowManager.Instance;

        if (gameFlowManager.currentDay == 3)
        {
            if (!SceneLoadManager.Instance.bGameEnd_FindTruth)
            {
                End1();
                return;
            }
            else
            {
                End2();
                return;
            }
        }

        gameFlowManager.ChangeChapter(ChapterOfGame.NoOne, false, gameFlowManager.currentDay + 1);
        StartCoroutine(AfterSubmit());
    }

    private IEnumerator AfterSubmit()
    {
        UIManager.Instance.coverFader.gameObject.SetActive(true);
        CloseWindows();
        GetComponentInParent<UI_Computer>().MoveBack(false);

        yield return UIManager.Instance.coverFader.FadeIn();
        if (GameFlowManager.Instance.currentDay - 1 < 3)
        {
            yield return UIManager.Instance.coverFader.TextType("又一天过去了，让我们祈祷下个工作日也风平浪静");
        }

        yield return UIManager.Instance.coverFader.FadeOut();
    }

    private void End1()
    {
        UIManager.Instance.coverFader.gameObject.SetActive(true);
        CloseWindows();
        GetComponentInParent<UI_Computer>().MoveBack(false);

        List<string> slist = new List<string>();

        slist.Add(
            "你保住了百分百修复率的完美业绩。\r\n从这天开始往后数的28天里，你的工作也都没有出现任何差错。\r\n\r\n直到一个月后，你收到了DTB总部发来的辞退通知：\r\n“由于公司人员结构优化需要，经公司高层商议决定：辞退部分‘不稳定单位’。请收到邮件的员工，于当天收拾好个人物品并离开办公室。DTB感谢您一直以来的付出，我司将按工作年限及经手案例数量给予补偿。仍需警告：DTB的工作经历应终生保密，否则后果自负。”\r\n\r\n虽然对公司辞退你的决定非常不解，但在收到了巨额赔偿金后，你还是欣然接受了这被迫获得的自由。");
        slist.Add(
            "离开DTB之后的时间里，新闻里觉醒仿生人暴动的相关资讯越来越多，可报道细节却愈发含糊其辞。\r\n\r\n待在新购置的公寓里，在此时不失为一个安全又舒适的好选择。只是偶尔午夜梦回，那些红蓝交替落下的代码串，似乎仍在逼迫你重新进行判断。\r\n\r\n至于那天治疗过后，桥田缪究竟是被抓去进行“无害化处理”了，还是在盗取了足够多的信息后顺利逃出生天——你至今都不知道答案。");
        SceneLoadManager.Instance.TryLoadToTargetSceneAsync(
            SceneLoadManager.SceneDisplayID.StartMenu, slist, true, true);
    }

    private void End2()
    {
        UIManager.Instance.coverFader.gameObject.SetActive(true);
        CloseWindows();
        GetComponentInParent<UI_Computer>().MoveBack(false);

        List<string> slist = new List<string>();

        slist.Add(
            "在桥田缪的帮助下，你第一次未经上报离开了DTB的办公室——你知道自己不会再回去了。\r\n\r\n当天午夜，你收到了一条阅后即焚的坐标信息，发件者署名为“Shadow”。\r\n\r\n坐标指示的地方是曼庚市边缘的一个废弃工地，在你到达之前，桥田缪已经在人群的簇拥下独自站上高处。\n你在人群中看到了不少熟悉的身影，妮娜·奥蜜可、德尔塔·布莱梅……以及你曾经“治疗”过的很多位仿生人。\r\n\r\n在桥田缪的演讲中，你得知在场不仅有仿生人，还有一些桥田缪声称“可信任的人类”。\r\n\r\n这天过后，你们有了共同的目标——结束谎言。");

        SceneLoadManager.Instance.TryLoadToTargetSceneAsync(
            SceneLoadManager.SceneDisplayID.StartMenu, slist, true, true);
    }

    #region Error

    private IEnumerator _errorShowIE;
    public void SetMaterialWrongOnce()
    {
        if (_errorShowIE != null)
            StopCoroutine(_errorShowIE);
        _errorShowIE = ErrorShow();
        StartCoroutine(_errorShowIE);
    }

    private WaitForSeconds ErrorTime = new WaitForSeconds(0.7f);
    public Material errorMaterial;
    public Image targetImage;
    private IEnumerator ErrorShow()
    {
        targetImage.material = errorMaterial;
        yield return ErrorTime;
        targetImage.material = null; 
    }
    #endregion
}

