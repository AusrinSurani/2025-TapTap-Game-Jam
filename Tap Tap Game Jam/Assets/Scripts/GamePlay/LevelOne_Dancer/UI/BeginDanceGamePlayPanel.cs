using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class BeginDanceGamePlayPanel : BaseUIPanel
{
    public Button ConfirmBtn;
    public Button CanceBtn;
    public void Start()
    {
        ConfirmBtn.onClick.AddListener(() =>
        {
            //GameObject.Find("DanceGamePlay").GetComponent<DanceGamePlay>().StartDanceGamePlay();
            //隐藏交互物品
            GameObject.Find("DancerDirector").GetComponent<DacnerDirector>().HideInteractItem();
            GameObject.Find("DancerDirector").GetComponent<PlayableDirector>().Play();
            UIManager.Instance.HidePanelByPath<BeginDanceGamePlayPanel>("UI/DanceGamePlay/BeginDanceGamePlayPanel");
        });
        CanceBtn.onClick.AddListener(() =>
        { 
            UIManager.Instance.HidePanelByPath<BeginDanceGamePlayPanel>("UI/DanceGamePlay/BeginDanceGamePlayPanel");
        });
    }
}
