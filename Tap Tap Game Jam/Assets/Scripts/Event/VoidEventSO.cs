using UnityEngine;
using UnityEngine.Events;

//无参数的事件，用来简单触发
[CreateAssetMenu(menuName = "Event/VoidEventSO")]
public class VoidEventSO : ScriptableObject
{
    public UnityAction OnEventRaise;

    public void RaiseEvent()
    {
        OnEventRaise?.Invoke();
    }
}

