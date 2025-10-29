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
        openComputer.OnEventRaise += StartMoveWithCheckEnd2;
    }

    private void OnDisable()
    {
        openComputer.OnEventRaise -= StartMoveWithCheckEnd2;
    }

    private void Start()
    {
        if (GameFlowManager.Instance.currentIsOver == true 
            && GameFlowManager.Instance.currentDay == 3
            &&SceneLoadManager.Instance.bGameEnd_FindTruth)
        { 
            errorWindow.SetActive(true);
        }
    } 

    private void StartMoveWithCheckEnd2()
    {
        if (GameFlowManager.Instance.currentIsOver == true 
            && GameFlowManager.Instance.currentDay == 3
            &&SceneLoadManager.Instance.bGameEnd_FindTruth)
        { 
            errorWindow.SetActive(true);
            //故障效果
            StartCoroutine(OnClick());
        }
        
        //正常情况直接Move
        StartMove();
    }

    private IEnumerator OnClick()
    {
        yield return new WaitForSeconds(1f);
        errorWindow.GetComponent<ErrorWindow>()?.SetMaterialWrongOnce();
    }
}