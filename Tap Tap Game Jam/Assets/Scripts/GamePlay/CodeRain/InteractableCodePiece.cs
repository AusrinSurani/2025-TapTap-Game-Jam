using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractableCodePiece : CodePiece,IPointerClickHandler
{
    public Color headCharacterColor_RedError;
    public Color longCodeColor_RedError;
    public Color headCharacterColor_BlueSpecial;
    public Color longCodeColor_BlueSpecial;

    public enum InteractableCodePieceType
    {
        Error_Red,
        Special_Blue
    }

    public InteractableCodePieceType PieceType;

    public bool BHaveInteract;

    private void Start()
    {
        BHaveInteract = false; 
    }

    public CodeGamePlay parentGamePlay;//

    public void OnPointerClick(PointerEventData eventData)
    {
        //被左键点击，未被交互过
        if(eventData.button==PointerEventData.InputButton.Left
            &&eventData.clickCount>0
            &&!BHaveInteract)
        {
            BHaveInteract = true;
            parentGamePlay.GetCodePiece(PieceType);
            //隐藏/销毁
            HideSelf();
        }
    }

    public void HideSelf()
    {
        //TODO:销毁效果 
        if (_disolveIE != null && !_bDisolving)
            StopCoroutine(_disolveIE);
        _disolveIE = DisolveSelf();
        if(this.gameObject.activeSelf)
            StartCoroutine(_disolveIE);

    }

    public void ForceStopCorotine()
    {
        if (_disolveIE != null)
            StopCoroutine(_disolveIE);
    }

    public CanvasGroup selfCanvasGroup;
    private IEnumerator _disolveIE;
    private bool _bDisolving;
    private IEnumerator DisolveSelf()
    {
        _bDisolving = true;
        //停止移动与变换
        SelfRb.gravityScale = 0;
        SelfRb.velocity = Vector3.zero;
        BNotChangeCodeContent = true;

        while (selfCanvasGroup.alpha>0)
        {
            selfCanvasGroup.alpha -= Time.deltaTime;
            yield return null;
        }
        this.gameObject.SetActive(false);
        //Destroy(this.gameObject);
    }
    private Color _tempColor;
    public void SetInteractType(InteractableCodePieceType t)
    {
        PieceType = t;
        if (PieceType == InteractableCodePieceType.Error_Red)
        {
            headText.color = headCharacterColor_RedError;
            _tempColor = longCodeColor_RedError;
            _tempColor.a = 0;
            fadeEffect.color1 = _tempColor;
            fadeEffect.color2 = longCodeColor_RedError;
            //速度降低
            StartVelocity.y = -75f;
            this.SelfRb.gravityScale = 40f;
        }
        else if (PieceType == InteractableCodePieceType.Special_Blue)
        {
            headText.color = headCharacterColor_BlueSpecial;
            _tempColor = longCodeColor_BlueSpecial;
            _tempColor.a = 0;
            fadeEffect.color1 = _tempColor;
            fadeEffect.color2 = longCodeColor_BlueSpecial;
            //速度降低
            StartVelocity.y = -75f;
            this.SelfRb.gravityScale = 25f;
        } 
    }
     
}
