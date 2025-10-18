using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DancerController : MonoBehaviour
{
    public Animator dancerAnimator;
    public enum DancerStatus
    {
        Idle,
        Up,
        Down,
        Left,
        Right,
        Special_1,
        Special_2,
        Jump,
        Wrong,
        Start,
        End,
        AutoDance
    }
    public DancerStatus curStatus;
    public SpriteRenderer dancerSpriteRenderer;
    public void SetDancerAnimatorStauts(DancerStatus targetStatus)
    {
        ResetAnimatorAllParams();
        if (targetStatus!=curStatus)
        {
            dancerAnimator.SetBool(targetStatus.ToString(),true);
        }
    }

    public void ResetAnimatorAllParams()
    {
        dancerAnimator.SetBool(DancerStatus.Idle.ToString(), false);
        dancerAnimator.SetBool(DancerStatus.Up.ToString(), false);
        dancerAnimator.SetBool(DancerStatus.Down.ToString(), false);
        dancerAnimator.SetBool(DancerStatus.Left.ToString(), false);
        dancerAnimator.SetBool(DancerStatus.Right.ToString(), false);
        dancerAnimator.SetBool(DancerStatus.Special_1.ToString(), false);
        dancerAnimator.SetBool(DancerStatus.Special_2.ToString(), false);
        dancerAnimator.SetBool(DancerStatus.Wrong.ToString(), false);
        dancerAnimator.SetBool(DancerStatus.Jump.ToString(), false);
        dancerAnimator.SetBool(DancerStatus.Start.ToString(), false);
        dancerAnimator.SetBool(DancerStatus.End.ToString(), false);
        dancerAnimator.SetBool(DancerStatus.AutoDance.ToString(), false);
    }

    private IEnumerator _errorShowIE;
    public void SetDanceMaterialWrongOnce()
    {
        if (_errorShowIE != null)
            StopCoroutine(_errorShowIE);
        _errorShowIE = ErrorShow();
        StartCoroutine(_errorShowIE);
    }

    private WaitForSeconds ErrorTime = new WaitForSeconds(0.5f);
    public Material errorMaterial;
    private IEnumerator ErrorShow()
    {  
        dancerSpriteRenderer.material.SetFloat("_Intensity", 0.015f);
        yield return ErrorTime;
        errorMaterial.SetFloat("_Intensity", 0f);
        dancerSpriteRenderer.material.CopyPropertiesFromMaterial(errorMaterial);
    }
    /*//输入判定允许的偏差值，计时单位 秒second
    public float inputAllowableDeviation;*/

    public DanceGamePlay parentGamePlay;
    //自动舞蹈中途调用
    public void AnimatorEventSpecialActionEnd()
    {
        parentGamePlay.SpecialActionEnd();
    }
     

     
}
