using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButton : MonoBehaviour,
    IPointerEnterHandler, 
    IPointerExitHandler, 
    IPointerClickHandler,
    IPointerDownHandler,
    IPointerUpHandler
{
	[Header("事件广播")]
	public VoidEventSO menuToOption;
	
	[SerializeField] MenuButtonController menuButtonController;
	[SerializeField] Animator animator;
	[SerializeField] AnimatorFunctions animatorFunctions;
	[SerializeField] int thisIndex;
    
    private bool isMouseOver = false; //鼠标是否在按钮上
    private bool isMouseDown = false; //鼠标是否按下
    
    private SceneLoadManager sceneLoadManager;
    
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

                sceneLoadManager.TryLoadToTargetSceneAsync(SceneLoadManager.SceneDisplayID.Level_1);
                Debug.Log("开始游戏");
                break;
            case 1:
                //TODO:切换场景继续游戏
                Debug.Log("继续游戏");
                break;
            case 2:
                //唤出optionsUI
                menuToOption.OnEventRaise();
                break;
            case 3:
                //退出游戏
                Debug.Log("退出游戏");
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
