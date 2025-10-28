using UnityEngine;

public class UI_Dialog : BaseUI
{
    [Header("事件广播")] 
    public VoidEventSO beginDialogEvent;
    public VoidEventSO endDialogEvent;
    
    public override void StartMove()
    {
        base.StartMove();
        beginDialogEvent.RaiseEvent();
    }
    
    public override void MoveBack(bool isVanish = true)
    {
        base.MoveBack(isVanish);
        endDialogEvent.RaiseEvent();
    }
}
