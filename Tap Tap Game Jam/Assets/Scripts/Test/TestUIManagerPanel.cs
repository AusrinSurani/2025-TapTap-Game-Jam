using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestUIManagerPanel : BaseUIPanel
{
    public Button CloseBtn;
    private void Awake()
    {
        CloseBtn.onClick.AddListener(()=>
        {
            UIManager.Instance.HidePanelByPath<TestUIManagerPanel>("UI/TestUIManagerPanel");
        });
    }
    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("OnEnter TestUIManagerPanel");
    }
    public override void OnExit()
    {
        base.OnExit();
        Debug.Log("OnExit TestUIManagerPanel");
    }
    public override void OnPause()
    {
        base.OnPause();
        Debug.Log("OnPause TestUIManagerPanel");
    }

    public override void OnResume()
    {
        base.OnResume();
        Debug.Log("OnResume TestUIManagerPanel");
    }
}
