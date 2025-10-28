using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_Computer : BaseUI
{
    [Header("事件监听")]
    public VoidEventSO openComputer;

    public GameObject dreamSystem;
    
    public GameObject errorWindow;
    
    private void OnEnable()
    {
        openComputer.OnEventRaise += StartMove;
    }

    private void OnDisable()
    {
        openComputer.OnEventRaise -= StartMove;
    }

    private void Start()
    {
        if (GameFlowManager.Instance.currentIsOver == true 
            && GameFlowManager.Instance.currentDay == 3
            &&SceneLoadManager.Instance.bGameEnd_FindTruth)
        { 
            errorWindow.SetActive(true);
            //故障效果
            errorWindow.GetComponent<ErrorWindow>()?.SetMaterialWrongOnce();
        }
    } 

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MoveBack();
        }
    }
}