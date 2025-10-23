using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowTextPiece : MonoBehaviour
{
    private void Start()
    {
        //test
        SetThisCurrentInput();
        SetShowText("Welcome to Bioroid Bioroid AI System 60.06.6 LTS \r\n");
        //endtest
    }

    public TextMeshProUGUI showingText;

    //输入中片段，需要闪烁光标▃
    public bool BCurrentInputPiece;
     
    #region TextType
    public bool BTextTyping;

    public void SetShowText(string textContent)
    {
        _preShowTextContent = textContent;
        //showText.text = text;
        BTextTyping = true;
        if (_textTypingIE != null)
            StopCoroutine(_textTypingIE);
        _textTypingIE = TextType();
        StartCoroutine(_textTypingIE);
    }
    private IEnumerator _textTypingIE;
    private string _preShowTextContent;
    private string _haveShowTextContent; 
    //打字间隔
    private WaitForSeconds _typeWaitTime = new WaitForSeconds(0.1f);
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
    public bool GetIsTextShowEnd()
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

    public IEnumerator EndCursorFlashIE;
    private bool _bCursorHide;

    private WaitForSeconds _cursorHideTime = new WaitForSeconds(0.5f);
    private string _tempTextString;
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
                showingText.text = _haveShowTextContent;
            }
            else
            {
                _tempTextString = _haveShowTextContent;
                _tempTextString += '▃';
                showingText.text =_tempTextString;

            }

            //光标闪烁
            yield return null;
        }
    }
}
