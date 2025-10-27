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
        PlayClickSfx();
        UI_Computer.SetActive(true);
        openComputer.OnEventRaise();
    }

    public override void PlayHoverSfx()
    {
        AudioManager.Instance.AudioOncePlay(AudioManager.Instance.computerHoverSfx);
    }

    public override void PlayClickSfx()
    {
        AudioManager.Instance.AudioOncePlay(AudioManager.Instance.windowsRaiseSfx);
    }
}