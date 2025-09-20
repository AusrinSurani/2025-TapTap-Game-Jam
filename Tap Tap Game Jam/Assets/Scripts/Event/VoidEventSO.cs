using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/VoidEventSO")]
public class VoidEventSO : ScriptableObject
{
    public UnityAction OnEventRaise;

    public void RaiseEvent()
    {
        OnEventRaise?.Invoke();
    }
}

