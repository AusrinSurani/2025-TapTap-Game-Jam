using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
public class BackButton : BounceButton
{
    [Header("事件广播")]
    public VoidEventSO optionToMenu;

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        optionToMenu?.OnEventRaise();
    }
}