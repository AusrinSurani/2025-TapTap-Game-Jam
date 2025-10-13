using System;
using UnityEngine.EventSystems;
using UnityEngine;

public class Computer : Interactable
{
    public GameObject UI_Computer;
    
    [Header("事件广播")] 
    public VoidEventSO openComputer;
    
    public override void OnPointerClick(PointerEventData eventData)
    {
        UI_Computer.SetActive(true);
        openComputer.OnEventRaise();
    }
}