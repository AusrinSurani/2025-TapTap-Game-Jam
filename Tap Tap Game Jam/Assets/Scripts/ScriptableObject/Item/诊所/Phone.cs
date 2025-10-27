using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Phone : Interactable
{
    public GameObject UI_Phone;
    
    [Header("事件广播")] 
    public VoidEventSO openPhone;

    public override void OnPointerClick(PointerEventData eventData)
    {
        PlayClickSfx();
        UI_Phone.SetActive(true);
        openPhone.OnEventRaise();
    }
}