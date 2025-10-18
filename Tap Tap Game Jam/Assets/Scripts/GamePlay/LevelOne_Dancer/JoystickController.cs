using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickController : MonoBehaviour
{
    public Animator JoystickAnimator;
    public enum JoystickStatus
    {
        Middle,
        MiddleOff,
        Up,
        Down,
        Left,
        Right,
    }

    public void SetAnimatorStatus(JoystickStatus status)
    {
        ResetAllAnimatorStatus();
        JoystickAnimator.SetBool(status.ToString(), true);
    }

    public void ResetAllAnimatorStatus()
    { 
        JoystickAnimator.SetBool(JoystickStatus.Middle.ToString(), false);
        JoystickAnimator.SetBool(JoystickStatus.MiddleOff.ToString(), false);
        JoystickAnimator.SetBool(JoystickStatus.Up.ToString(), false);
        JoystickAnimator.SetBool(JoystickStatus.Down.ToString(), false);
        JoystickAnimator.SetBool(JoystickStatus.Left.ToString(), false);
        JoystickAnimator.SetBool(JoystickStatus.Right.ToString(), false);
    }
}
