using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Map : Interactable
{
    public PolygonCollider2D bedCollider;
    
    [Header("事件监听")] 
    public VoidEventSO getFirstMapEvent;
    
    public GameObject map;
    public PlayerController  player;

    [Header("流程")] 
    public string[] firstMessage;
    public string[] secondMessage;
    public bool haveFirstMap = false;
    private bool isFirstlyInteract = true;
    public bool isInMap = false;

    private void OnEnable()
    {
        getFirstMapEvent.OnEventRaise += () => {haveFirstMap = true;};
    }

    private void OnDisable()
    {
        getFirstMapEvent.OnEventRaise -= () => {haveFirstMap = true;};
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (!haveFirstMap && isFirstlyInteract)
        {
            DialogManager.Instance.ShowMessage(firstMessage);
            
            isFirstlyInteract = false;
            return;
        }

        if (!haveFirstMap)
        {
            DialogManager.Instance.ShowMessage(secondMessage);
            return;
        }
        
        bedCollider.enabled = false;
        player.BNoGetInput = true;
        player.SetZeroVelocity();
        player.SetInputZero();
        map.GetComponent<UI_Map>().StartMove();
    }
}