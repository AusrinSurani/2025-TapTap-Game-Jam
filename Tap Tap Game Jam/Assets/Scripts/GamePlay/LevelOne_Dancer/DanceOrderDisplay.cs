using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
            SpriteRenderer =this.transform.GetChild(1).GetComponent<SpriteRenderer>();
        }
        else if(ShowOrder == DanceGamePlay.DanceOrder.Down)
        { 
            this.transform.GetChild(2).gameObject.SetActive(true);
            SpriteRenderer = this.transform.GetChild(2).GetComponent<SpriteRenderer>();
        }
        else if (ShowOrder == DanceGamePlay.DanceOrder.Left)
        {
            this.transform.GetChild(3).gameObject.SetActive(true);
            SpriteRenderer = this.transform.GetChild(3).GetComponent<SpriteRenderer>();
        }
        else if (ShowOrder == DanceGamePlay.DanceOrder.Right)
        {
            this.transform.GetChild(4).gameObject.SetActive(true);
            SpriteRenderer = this.transform.GetChild(4).GetComponent<SpriteRenderer>();
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
        else if (allowInverse&&(((int)getOrder * 2) == (int)ShowOrder || ((int)ShowOrder) * 2 == (int)getOrder))
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

    public Image TimerBG;
    public Image TimerImage;
    private Color _bgColor;
    private Color _imageColor;
    [Header("Flash Effect")]
    public float AlphaSpeed=1f;
    private bool _bAlphaRiseUp;
    public float minAlpha_FlashEffect; 
    private float _curAlpha_FlashEffect;
    public float intervalTime_FlashEffect; 
    private Color _spriteColor;
    private void Update()
    {
        //没收到交互则计时
        if (!_bGetInteract)
        { 
            _disappearTimer += Time.deltaTime;
            TimerImage.fillAmount = (1f - _disappearTimer / DisappearTime);

            //TODO：闪烁效果
            if(_curAlpha_FlashEffect<minAlpha_FlashEffect)
            {
                //开始递增
                _bAlphaRiseUp = true;
            }
            else if (_curAlpha_FlashEffect>1f+ intervalTime_FlashEffect)
            {
                _bAlphaRiseUp = false;
            }

            if (_bAlphaRiseUp)
                _curAlpha_FlashEffect += Time.deltaTime* AlphaSpeed;
            else
                _curAlpha_FlashEffect -= Time.deltaTime * AlphaSpeed;

            _spriteColor = SpriteRenderer.color;
            _spriteColor.a = _curAlpha_FlashEffect;
            SpriteRenderer.color = _spriteColor;
        }
        else
        {
            if (TimerBG.color.a > 0)
            {
                _bgColor = TimerBG.color;
                _bgColor.a -= Time.deltaTime;
                TimerBG.color = _bgColor;
            }
            if (TimerImage.color.a > 0)
            {
                _imageColor = TimerImage.color;
                _imageColor.a -= Time.deltaTime;
                TimerImage.color = _imageColor;
            } 
        }
        if(DisappearTime<_disappearTimer)
        {
            _disappearTimer = 0;
            TimerImage.fillAmount = 0f;
            //通知 DanceGamePlay Error
            _bGetInteract = true; 
            this.transform.GetChild(5).gameObject.SetActive(true);
            ParentGamePlay.FailCurOrderInput();
            Invoke(nameof(DestroySelf), 0.5f);
        }
    }

    public void OnEndGamePlay()
    {
        //尚未交互的直接销毁
        if(!_bGetInteract)
        {
            _bGetInteract = true;
            _disappearTimer = 0;
            DestroySelf();
        }
        else
        {
            //等待自然销毁
        }
    }

    private void DestroySelf()
    {
        Destroy(this.gameObject);
    }
}
