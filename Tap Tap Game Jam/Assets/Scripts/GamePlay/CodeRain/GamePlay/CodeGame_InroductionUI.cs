using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;

public class CodeGame_InroductionUI : MonoBehaviour
{

    public GuideMask guideMask;
    private void Awake()
    {
        //guideMask.Init();
       
    }
    private void Start()
    {
        
        
        
    }
    private void Update()
    { 
    }

    public RectTransform intro_levelOne;
    public RectTransform intro_levelTwo;

    public TextMeshProUGUI showingText;

    public bool BIntroShowEnd;

    #region IntroTextType
    public bool BTextTyping; 
    
    public IEnumerator IntroShow_LevelOne()
    {
        BIntroShowEnd = false;
        _finishWaitTime = new WaitForSeconds(0);
        _haveShowTextContent = string.Empty;
        _preShowTextContent = string.Empty;
        yield return TextTypeWithEndText("请用鼠标点击以捕获", "<color=red>红</color>", "<color=red>色</color>");
        yield return TextType("错误代码，清除【bug】", false); 
        BIntroShowEnd = true;
    }

    public IEnumerator IntroShow_LevelTwo()
    {
        BIntroShowEnd = false;
        _finishWaitTime = new WaitForSeconds(0);
        _haveShowTextContent = string.Empty;
        _preShowTextContent = string.Empty;
        yield return TextTypeWithEndText("请用鼠标点击以捕获", "<color=red>红</color>", "<color=red>色</color>");
        yield return TextType("错误代码，清除【bug】",false);
        yield return RebackTextType("请用鼠标点击以捕获");
        yield return TextType("错误代码，获取【真相】", "<color=#00CEFF>蓝</color>", " <color=#00CEFF>色</color>");
        BIntroShowEnd = true;
    }

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
    [SerializeField]
    private string _preShowTextContent;
    [SerializeField]
    [TextArea]
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
            showingText.text = _haveShowTextContent;
            yield return _typeWaitTime;
        }
        BTextTyping = false;
    }
    public IEnumerator TextType(string content,bool needResetString)
    {
        _preShowTextContent = content;
        BTextTyping = true;
        if(needResetString) 
            showingText.text = string.Empty;
        for (int i = 0; i < _preShowTextContent.Length; i++)
        {
            _haveShowTextContent += _preShowTextContent[i];
            showingText.text = _haveShowTextContent;
            yield return _typeWaitTime;
        }
        yield return _finishWaitTime;
        BTextTyping = false;
    }
    public IEnumerator TextTypeWithEndText(string content,string endContent_1,string endContent_2)
    {
        _preShowTextContent = content;
        BTextTyping = true;
        showingText.text = string.Empty;
        for (int i = 0; i < _preShowTextContent.Length; i++)
        {
            _haveShowTextContent += _preShowTextContent[i];
            showingText.text = _haveShowTextContent;
            yield return _typeWaitTime;
        }
        //添加末位字符
        _haveShowTextContent += endContent_1;
        showingText.text = _haveShowTextContent;
        yield return _typeWaitTime;
        _haveShowTextContent += endContent_2;
        showingText.text = _haveShowTextContent;
        yield return _typeWaitTime;
        yield return _finishWaitTime;
        BTextTyping = false;
    }

    public IEnumerator TextType(string content,string startContent_1,string startContent_2)
    {
        _preShowTextContent = content;
        BTextTyping = true;
        _haveShowTextContent = showingText.text;
        //添加第一个初始字符
        _haveShowTextContent += startContent_1;
        showingText.text = _haveShowTextContent;
        yield return _typeWaitTime;
        _haveShowTextContent += startContent_2;
        showingText.text = _haveShowTextContent;
        yield return _typeWaitTime;

        for (int i = 0; i < _preShowTextContent.Length; i++)
        {
            _haveShowTextContent += _preShowTextContent[i];
            showingText.text = _haveShowTextContent;
            yield return _typeWaitTime;
        }
        yield return _finishWaitTime;
        BTextTyping = false;
    }

    private int _tempTextCount;

    private bool _bGetRichMark;
    private string _tempString;
    private string _tempEndAddString;
    private IEnumerator RebackTextType(string targetContent )
    {
        _bGetRichMark = false;
        yield return null;
        //目标字符串不短于当前字符串 
        BTextTyping = true;
        _tempString = string.Empty;
        _tempEndAddString = string.Empty;
        //_haveShowTextContent = showingText.text;
        _tempTextCount = _haveShowTextContent.Length-targetContent.Length;
        //Debug.Log("Rebakc Count:"+ _tempTextCount);
        for (int i=0;i< _tempTextCount;i++)
        {
            if (_haveShowTextContent[_haveShowTextContent.Length - i - 1] =='>')
            {
                _bGetRichMark = true;
                _tempString += _haveShowTextContent[_haveShowTextContent.Length - i - 1];
                continue;
            }
            if (_bGetRichMark )
            {
                if (_haveShowTextContent[_haveShowTextContent.Length - i - 1] == '<')
                {
                    _tempString += _haveShowTextContent[_haveShowTextContent.Length - i - 1];
                    for(int j= _tempString.Length-1; j>=0;j--)
                    {
                        _tempEndAddString += _tempString[j];
                    }
                    _bGetRichMark = false; 
                }
                else
                {
                    _tempString += _haveShowTextContent[_haveShowTextContent.Length - i - 1];
                    continue;
                }

            } 
            showingText.text = _haveShowTextContent.Substring(0, _haveShowTextContent.Length - i-1);
            showingText.text += _tempEndAddString;
            yield return _typeWaitTime;
        }
        BTextTyping = false;
    }

    public bool GetIsTextShowEnd()
    {
        return BTextTyping;
    }
    #endregion
}
