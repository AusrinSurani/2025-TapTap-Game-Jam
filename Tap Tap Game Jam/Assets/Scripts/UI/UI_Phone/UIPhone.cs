using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIPhone : BaseUI
{
    [Header("事件监听")]
    public VoidEventSO openPhone;
    public VoidEventSO chapterChangeEvent;

    [Header("资讯")] 
    public News[] news1;
    public News[] news2;
    public News[] news3;
    
    public GameObject mask;

    private News[] currentNews = null;
    
    private void Start()
    {
        mask.SetActive(false);
        RefreshInformation();
    }

    private void OnEnable()
    {
        openPhone.OnEventRaise += StartMove;
        openPhone.OnEventRaise += UseMask;
        chapterChangeEvent.OnEventRaise += RefreshInformation;
    }

    private void OnDisable()
    {
        openPhone.OnEventRaise -= StartMove;
        openPhone.OnEventRaise -= UseMask;
        chapterChangeEvent.OnEventRaise -= RefreshInformation;
    }
    
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            mask.SetActive(false);
            MoveBack(false);
        }
    }

    public void OnExitButtonClick()
    {
        mask.SetActive(false);
        MoveBack(false);
    }

    private void UseMask()
    {
        mask.SetActive(true);
    }

    private void RefreshInformation()
    {
        int currentDay = GameFlowManager.Instance.currentDay;

        switch (currentDay)
        {
            //资讯随着天数刷新
            case 1:
                Refresh(true,false,false);
                currentNews = news1;
                break;
            case 2:
                Refresh(false,true,false);
                currentNews = news2;
                break;
            case 3:
                Refresh(false,false,true);
                currentNews = news3;
                break;
        }
        
        Debug.Log(currentDay+"day,资讯设置完毕");
        return;

        void Refresh(bool n1, bool n2, bool n3)
        {
            foreach (var t in news1)
            {
                t.gameObject.SetActive(n1);
            }

            foreach (var t in news2)
            {
                t.gameObject.SetActive(n2);
            }

            foreach (var t in news3)
            {
                t.gameObject.SetActive(n3);
            }
        };
    }

    public bool GetHaveAllRead()
    {
        foreach (var t in currentNews)
        {
            if (t.GetIsRead() == false)
            {
                return false;
            }
        }
        return true;
    }
}