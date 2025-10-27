using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowTextPiece : MonoBehaviour
{
    private void Start()
    {
    }

    public Text showingText;

    //输入中片段，需要闪烁光标▃
    public bool BCurrentInputPiece;
     
    #region TextType
    public bool BTextTyping;

    public void SetShowTextForType(string textContent,bool bLongText)
    { 
        if (bLongText)
            sizeFitter.enabled=true;
        else
            sizeFitter.enabled=false;;
        _preShowTextContent = textContent;
        //showText.text = text;
        BTextTyping = true;
        if (_textTypingIE != null)
            StopCoroutine(_textTypingIE);
        _textTypingIE = TextType();
        StartCoroutine(_textTypingIE);
    }
    public void SetShowTextForType(string textContent,Color tcolor,bool bLongText)
    {
        if (bLongText)
            sizeFitter.enabled=true;
        else
            sizeFitter.enabled=false;;
        _preShowTextContent = textContent;
        showingText.color = tcolor;
        BTextTyping = true;
        if (_textTypingIE != null)
            StopCoroutine(_textTypingIE);
        _textTypingIE = TextType();
        StartCoroutine(_textTypingIE);
    }


    public void SetShowTextForType(string textContent, Color tcolor,bool notClearText,float waitBeforeTypeTime,bool bLongText)
    {
        if (bLongText)
            sizeFitter.enabled=true;
        else
            sizeFitter.enabled=false;;
        _preShowTextContent = textContent;
        showingText.color = tcolor;
        BTextTyping = true;
        if (_textTypingIE != null)
            StopCoroutine(_textTypingIE);
        _textTypingIE = TextType(textContent,notClearText,waitBeforeTypeTime);
        StartCoroutine(_textTypingIE);
    }
    private IEnumerator _textTypingIE;
    private string _preShowTextContent;
    private string _haveShowTextContent; 
    //打字间隔
    private WaitForSeconds _typeWaitTime = new WaitForSeconds(0.05f);
    //打字结束后过渡等待滞留时间
    private WaitForSeconds _finishWaitTime = new WaitForSeconds(2f);

    public void SetWaitTime(float typeCharacterInterval,float typeEndWaitInterval)
    {
        _typeWaitTime = new WaitForSeconds(typeCharacterInterval);
        _finishWaitTime = new WaitForSeconds(typeEndWaitInterval);
    }
    private IEnumerator TextType()
    {
        BTextTyping = true;
        showingText.text = string.Empty;
        for (int i = 0; i < _preShowTextContent.Length; i++)
        {
            _haveShowTextContent += _preShowTextContent[i];
            //showingText.text = _haveShowTextContent;
            yield return _typeWaitTime;
        }
        BTextTyping = false;
    }
    public IEnumerator TextType(string content)
    {
        _preShowTextContent = content;
        BTextTyping = true;
        showingText.text = string.Empty;
        for (int i = 0; i < _preShowTextContent.Length; i++)
        {
            _haveShowTextContent += _preShowTextContent[i];
            //showingText.text += _preShowTextContent[i];
            yield return _typeWaitTime;
        }
        yield return _finishWaitTime;
        BTextTyping = false;
    }
     
    public IEnumerator TextType(string content,bool bNotClearText,float waitTimeBeforeType)
    { 

        yield return new WaitForSeconds(waitTimeBeforeType);
        _preShowTextContent = content;
        BTextTyping = true;
        if(!bNotClearText)
            showingText.text = string.Empty;
        for (int i = 0; i < _preShowTextContent.Length; i++)
        {
            _haveShowTextContent += _preShowTextContent[i];
            //showingText.text += _preShowTextContent[i];
            yield return _typeWaitTime;
        }
        yield return _finishWaitTime;
        BTextTyping = false;
    }
    public bool GetIsTextShowTyping()
    {
        return BTextTyping;
    }
    #endregion

    public void SetThisCurrentInput()
    {
        BCurrentInputPiece = true;
        if(EndCursorFlashIE==null)
        {
            BCurrentInputPiece = true;
            EndCursorFlashIE = CursorFlash();
            StartCoroutine(EndCursorFlashIE);
        }
        else
        {
            //没有正在运行闪烁
            if (!BCurrentInputPiece)
                StopCoroutine(EndCursorFlashIE);
            EndCursorFlashIE = CursorFlash();
            StartCoroutine(EndCursorFlashIE);

        }
    }
    public void ResetThisInputStatus()
    {
        if (EndCursorFlashIE != null)
            StopCoroutine(EndCursorFlashIE);
        BCurrentInputPiece = false;
        showingText.text = _haveShowTextContent;
    }

    public IEnumerator EndCursorFlashIE;
    private bool _bCursorHide;

    private WaitForSeconds _cursorHideTime = new WaitForSeconds(0.5f);
    private string _tempTextString;

    public ContentSizeFitter sizeFitter;
    private IEnumerator CursorFlash()
    {
        _bCursorHide = false;
        while(BCurrentInputPiece)
        {
            //Typing常量，TypingEnd时闪烁计时
            if(BTextTyping)
            {
                _bCursorHide = false;
            }
            else
            {
                //等待间隔时间，切换状态
                yield return _cursorHideTime;
                _bCursorHide = !_bCursorHide;
            }
            if(_bCursorHide)
            {
                showingText.text = _haveShowTextContent+ "<color=#FFFFFF> </color>";
            }
            else
            {
                _tempTextString = _haveShowTextContent;
                _tempTextString += "<color=#FFFFFF>▃</color>";
                showingText.text =_tempTextString;

            }

            //光标闪烁
            yield return null;
        }
    }

    public void SetShowTextNoType(string content,bool bLongText)
    {
        if (bLongText)
            sizeFitter.enabled=true;
        else
            sizeFitter.enabled=false;; 
        _haveShowTextContent = content;
        //初始光标为 亮 状态
        _bCursorHide = true;
        showingText.text = _haveShowTextContent+ "<color=#FFFFFF>▃</color>";
    } 
    public void SetShowTextNoType(string content, Color tcolor,bool bLongText)
    {
        if (bLongText)
            sizeFitter.enabled=true;
        else
            sizeFitter.enabled=false;;
        _haveShowTextContent = content;
        //初始光标为 亮 状态
        _bCursorHide = true;
        showingText.color = tcolor;
        showingText.text = _haveShowTextContent + "<color=#FFFFFF>▃</color>"; 
    }


    public void SetShowTextMixedMode(string notypeContent,string typeContent,Color color_notype ,Color tcolor,float waitTimeBeforeType)
    {
        //默认开启自适应
        sizeFitter.enabled = true;
        LayoutRebuilder.ForceRebuildLayoutImmediate(this.transform.GetComponent<RectTransform>());

        _haveShowTextContent ="<color=#"+ColorUtility.ToHtmlStringRGB(color_notype)+">"+ notypeContent+"</color>";
        //初始光标为 亮 状态
        _bCursorHide = true;
        showingText.text = _haveShowTextContent;
        showingText.color = tcolor;
        SetShowTextForType(typeContent, tcolor,true, waitTimeBeforeType,false);
    }
    public void SetShowTextMixedMode(string notypeContent, string typeContent, Color color_notype, Color tcolor, float waitTimeBeforeType,bool bLongText)
    {
        if (bLongText)
            sizeFitter.enabled=true;
        else
            sizeFitter.enabled=false;;
        _haveShowTextContent = "<color=#" + ColorUtility.ToHtmlStringRGB(color_notype) + ">" + notypeContent + "</color>";
        //初始光标为 亮 状态
        _bCursorHide = true;
        showingText.text = _haveShowTextContent;
        showingText.color = tcolor;
        SetShowTextForType(typeContent, tcolor, true, waitTimeBeforeType, false);
    }
}
