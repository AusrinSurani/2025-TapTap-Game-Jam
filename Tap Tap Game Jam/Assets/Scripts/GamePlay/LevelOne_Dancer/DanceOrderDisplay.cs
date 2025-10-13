using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanceOrderDisplay : MonoBehaviour
{
    public DanceGamePlay.DanceOrder ShowOrder;
    public DanceGamePlay ParentGamePlay;
    public int OrderIndex;

    public float extraExistTime;

    public SpriteRenderer SpriteRenderer; 

    private bool _bGetInteract;

    public void SetOrderDisplay(DanceGamePlay.DanceOrder displayOrder)
    {
        ShowOrder = displayOrder;
        //Child(0)为背景
        if (ShowOrder==DanceGamePlay.DanceOrder.Up)
        {
            this.transform.GetChild(1).gameObject.SetActive(true);
        }
        else if(ShowOrder == DanceGamePlay.DanceOrder.Down)
        { 
            this.transform.GetChild(2).gameObject.SetActive(true);
        }
        else if (ShowOrder == DanceGamePlay.DanceOrder.Left)
        {
            this.transform.GetChild(3).gameObject.SetActive(true);
        }
        else if (ShowOrder == DanceGamePlay.DanceOrder.Right)
        {
            this.transform.GetChild(4).gameObject.SetActive(true);
        }
         
    }
    public int GetInteractOrder(DanceGamePlay.DanceOrder getOrder,bool allowInverse,bool mustInverse)
    {
        _bGetInteract = true;
        _disappearTimer = 0;
        //滞留一定时间后销毁
        Invoke(nameof(DestroySelf), extraExistTime);
        //正按
        if (getOrder == ShowOrder&&!mustInverse)
        {
            //Color.Green
            SpriteRenderer.color = Color.green;
            //ParentGamePlay.SuccessCurOrderInput(true);
            return 1;
        }
        //反按
        else if (allowInverse&&((int)getOrder * 2 == (int)ShowOrder || (int)ShowOrder * 2 == (int)getOrder))
        {
            //Color.Blue 
            SpriteRenderer.color = Color.blue;
            //ParentGamePlay.SuccessCurOrderInput(false);
            return 2; 
        }
        else
        {
            //ParentGamePlay.FailCurOrderInput();
            this.transform.GetChild(5).gameObject.SetActive(true);
            return 0;
        }
    }
    private float _disappearTimer;
    public float DisappearTime;
    private void Update()
    {
        //没收到交互则计时
        if(!_bGetInteract)
            _disappearTimer += Time.deltaTime;
        if(DisappearTime<_disappearTimer)
        {
            _disappearTimer = 0;
            //通知 DanceGamePlay Error
            _bGetInteract = true; 
            this.transform.GetChild(5).gameObject.SetActive(true);
            ParentGamePlay.FailCurOrderInput();
            Invoke(nameof(DestroySelf), 0.5f);
        }
    }

    private void DestroySelf()
    {
        Destroy(this.gameObject);
    }
}
