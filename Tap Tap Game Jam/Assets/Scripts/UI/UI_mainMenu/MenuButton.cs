using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static SceneLoadManager;

public class MenuButton : MonoBehaviour,
    IPointerEnterHandler, 
    IPointerExitHandler, 
    IPointerClickHandler,
    IPointerDownHandler,
    IPointerUpHandler
{
	[Header("事件广播")]
	public VoidEventSO menuToOption;
    public VoidEventSO quitGameEvent;
	
	[SerializeField] MenuButtonController menuButtonController;
	[SerializeField] Animator animator;
	[SerializeField] AnimatorFunctions animatorFunctions;
	[SerializeField] int thisIndex;
    
    private bool isMouseOver = false; //鼠标是否在按钮上
    private bool isMouseDown = false; //鼠标是否按下
    
    private SceneLoadManager sceneLoadManager;
    
    private bool isLoadingScene = false;
    
    private void Awake()
    {
        //TODO:把开头就会出现的按钮音效去掉
    }

    private void Start()
    {
        sceneLoadManager = SceneLoadManager.Instance;
    }

    void Update()
    {
        KeyBoardCheck();
    }

    #region KeyBoardControll

    private void KeyBoardCheck()
    {
        if(isLoadingScene)
            return;
        
        if(menuButtonController.index == thisIndex)
        {
            animator.SetBool ("selected", true);
			
            if(Input.GetAxis ("Submit") == 1)
            {
                //键盘按下
                HandleButtonPress(true);
            }
            else if (animator.GetBool ("pressed") && !isMouseDown)
            {
                //确保鼠标没有按下，不然只会在键盘按下时才有press动画
                HandleButtonPress(false);
                ExecuteButtonAction();
            }
        }
        else
        {
            //只有在鼠标没有停留在按钮上，并且当前没有被按下时，才取消 selected 状态
            if (!isMouseOver && !isMouseDown)
                animator.SetBool ("selected", false);
        }
    }

    private void HandleButtonPress(bool isPressed)
    {
        animator.SetBool("pressed", isPressed);
        if (!isPressed)
        {
            animatorFunctions.disableOnce = true;
        }
    }

    // 执行按钮对应的功能
    private void ExecuteButtonAction()
    {
        switch (thisIndex)
        {
            case 0:
                //TODO:切换场景开始游戏
                isLoadingScene = true;
                
                //数据选择
                SaveSystem.DeleteSavedFile("HaveReadEmailData.txt");
                GameFlowManager.Instance.SetDefaultChapterData();
                
                //场景切换
                //sceneLoadManager.TryLoadToTargetSceneAsync
                    //(SceneLoadManager.SceneDisplayID.ConsultationRoom, "开场白",true);
                
                //音乐切换
                AudioManager.Instance.ClearTargetAudioPiece(AudioManager.Instance.mainMenuBGM);
                AudioManager.Instance.AudioLoopPlay(AudioManager.Instance.consultingBGM);

                //开场白
                List<string> slist = new List<string>();
                slist.Add("“咔哒…”\r\n值得庆祝的是，你又一次准时推开了办公室的大门。\r\n\r\n曼庚城的清晨总在上演同样的滑稽剧情：\r\n街角的全息广告总因数据过载开始抽搐，仿生人环卫工会突然原地转圈用扫把横扫路人，新闻播报卡在“今日公共安全评分为1”这句上复读，还好没人相信1后面还跟着两个0。\r\n城市的立体交通更是场灾难，罢工、抗议、暴力事件，或只是一次“必要的月度检修”，都能让你的全勤奖泡汤。\r\n\r\n在DTB上班的这一年多时间里，你早就知道自己的生物钟和双腿比什么都可靠。");
                slist.Add("在曼庚城，人们的疾病早已超出了传统医疗的管辖范围。\r\n你的工作是钻进他们的脑子，在梦境里修复那些偏离正轨的意识。\r\n\r\n至于这座城市究竟生了什么病？那不归你管。\r\n你只想在下班前搞定今天的指标，祈祷别再碰上些延误工时的话痨。\r\n你的信条很简单：梦境治疗不问真相，只讲疗效。\r\n\r\n毕竟，一个安稳的梦境，远比追寻那些疯狂的“真相”来得实惠——当然，也安全得多。");
                sceneLoadManager.TryLoadToTargetSceneAsync(SceneDisplayID.ConsultationRoom, slist, false, false);
                /*sceneLoadManager.TryLoadToTargetSceneAsync
                    (SceneLoadManager.SceneDisplayID.ConsultationRoom, "开场白",true);*/
                Debug.Log("开始游戏");
                break;
            case 1:
                //TODO:切换场景继续游戏(从某一章开头开始)
                isLoadingScene = true;
                
                //数据选择
                GameFlowManager.Instance.currentIsOver = false;
                GameFlowManager.Instance.currentChapter = ChapterOfGame.NoOne;
                GameFlowManager.Instance.SaveChapterData();
                
                //场景切换
                sceneLoadManager.TryLoadToTargetSceneAsync
                    (SceneLoadManager.SceneDisplayID.ConsultationRoom, "继续",true);
                
                //音乐切换
                AudioManager.Instance.ClearTargetAudioPiece(AudioManager.Instance.mainMenuBGM);
                AudioManager.Instance.AudioLoopPlay(AudioManager.Instance.consultingBGM);
                break;
            case 2:
                //唤出optionsUI
                menuToOption.OnEventRaise();
                break;
            case 3:
                Debug.Log("退出游戏");
                //退出游戏,触发保存等事件
                quitGameEvent.RaiseEvent();
                Application.Quit();
                break;
        }
    }

    #endregion

    #region PointerControll
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOver = true;
        animator.SetBool("selected", true);
        menuButtonController.index = thisIndex;
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOver = false;
        if(menuButtonController.index != thisIndex && !isMouseDown)
        {
            animator.SetBool("selected", false);
        }
        
        //确保鼠标离开时 pressed == false，防止卡住
        animator.SetBool("pressed", false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isMouseDown = true;
        HandleButtonPress(true);
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        isMouseDown = false;
        HandleButtonPress(false);
        
        //确保鼠标离开时，如果键盘没有选中它，离开selected
        if(!isMouseOver && menuButtonController.index != thisIndex)
        {
            animator.SetBool("selected", false);
        }
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        ExecuteButtonAction();
    }
    
    #endregion
}
