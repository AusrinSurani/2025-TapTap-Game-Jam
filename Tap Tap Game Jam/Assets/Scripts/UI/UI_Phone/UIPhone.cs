using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPhone : BaseUI
{
    [Header("事件监听")]
    public VoidEventSO openPhone;

    public GameObject mask;

    private void Start()
    {
        mask.SetActive(false);
    }

    private void OnEnable()
    {
        openPhone.OnEventRaise += StartMove;
        openPhone.OnEventRaise += UseMask;
    }

    private void OnDisable()
    {
        openPhone.OnEventRaise -= StartMove;
        openPhone.OnEventRaise -= UseMask;
    }
    
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            mask.SetActive(false);
            MoveBack();
        }
    }

    private void UseMask()
    {
        mask.SetActive(true);
    }
}
