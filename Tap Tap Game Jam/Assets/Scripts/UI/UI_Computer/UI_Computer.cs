using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Computer : BaseUI
{
    [Header("事件监听")]
    public VoidEventSO openComputer;

    public GameObject dreamSystem;
    
    private void OnEnable()
    {
        openComputer.OnEventRaise += StartMove;
    }

    private void OnDisable()
    {
        openComputer.OnEventRaise -= StartMove;
    }
    
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MoveBack();
        }
    }
}