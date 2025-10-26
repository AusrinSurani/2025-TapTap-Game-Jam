using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bremen : MonoBehaviour,IPointerClickHandler
{
    [Header("事件监听")] public VoidEventSO get7MapsEvent;
    
    [Header("所有对话")] 
    public TextAsset aboutVase;
    public TextAsset aboutTravelPlan7;
    public TextAsset lastDialog;
    
    private bool canTalkAboutVase = false;
    private bool haveTalkAboutVase = false;
    
    private bool canTalkAboutTravelPlan7 = false;
    private bool haveTalkAboutTravelPlan7 = false;

    private void OnEnable()
    {
        get7MapsEvent.OnEventRaise += () => canTalkAboutTravelPlan7 = true;
    }

    private void OnDisable()
    {
        get7MapsEvent.OnEventRaise -= () => canTalkAboutTravelPlan7 = false;
    }

    public void TriggerVase()
    {
        canTalkAboutVase = true;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (canTalkAboutTravelPlan7 && !haveTalkAboutTravelPlan7)
        {
            haveTalkAboutTravelPlan7 = true;
            DialogManager.Instance.StartDialog(aboutTravelPlan7);
            return;
        }
        
        if (canTalkAboutVase && !haveTalkAboutVase)
        {
            DialogManager.Instance.StartDialog(aboutVase);
            /*haveTalkAboutVase = true;*/
            return;
        }
    }
}