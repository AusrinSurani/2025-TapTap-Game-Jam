using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    [Header("事件监听")] 
    public VoidEventSO menuToOption;
    public VoidEventSO optionToMenu;
    
    [SerializeField] private UI_option uiOption;
    [SerializeField] private MenuButtonController uiMenu;

    private void OnEnable()
    {
        menuToOption.OnEventRaise += OnMenuToOption;
        optionToMenu.OnEventRaise += OnOptionToMenu;
    }

    private void OnDisable()
    {
        menuToOption.OnEventRaise -= OnMenuToOption;
        optionToMenu.OnEventRaise -= OnOptionToMenu;
    }

    private void Start()
    {
        //游戏开始先显示menu
        uiMenu.StartMove();
    }
    private void OnMenuToOption()
    {
        SwitchUI(uiOption, uiMenu);
    }

    private void OnOptionToMenu()
    {
        SwitchUI(uiMenu, uiOption);
    }

    private void SwitchUI(BaseUI uiToShow, BaseUI uiToHide)
    {
        uiToHide.MoveBack();
        uiToShow.StartMove();
    }
}
