using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;

public class LoadingCoverFader : MonoBehaviour
{
    public float fadeSpeed=1f;
    private CanvasGroup _coverGroup;

    public TextMeshProUGUI DisplayTextPro;
    private void Awake()
    {
        _coverGroup = GetComponent<CanvasGroup>();
        DisplayTextPro.text = string.Empty;
    }

    /* private void OnEnable()
     {
         if(SceneLoadManager.Instance!=null)
         {
             SceneLoadManager.Instance.onSceneLoadBegin.AddListener(BeginFadeIn);
             SceneLoadManager.Instance.onSceneLoadEnd.AddListener(BeginFadeOut);
         }
     }
     private void OnDisable()
     {
         if (SceneLoadManager.Instance != null)
         {
             SceneLoadManager.Instance.onSceneLoadBegin.RemoveListener(BeginFadeIn);
             SceneLoadManager.Instance.onSceneLoadEnd.RemoveListener(BeginFadeOut);
         }
     }*/

    public bool BFading;

    public bool BTextTyping;

    public void ShowText(string text)
    { 
        _preShowTextContent = text;
        //showText.text = text;
        BTextTyping = true;
        if (_textTypingIE != null)
            StopCoroutine(_textTypingIE);
        _textTypingIE = TextType();
        StartCoroutine(_textTypingIE);
    }
    private IEnumerator _textTypingIE;
    private string _preShowTextContent;
    //打字间隔
    private WaitForSeconds _typeWaitTime = new WaitForSeconds(0.1f);
    //打字结束后过渡等待滞留时间
    private WaitForSeconds _finishWaitTime = new WaitForSeconds(2f);
    private IEnumerator TextType()
    { 
        DisplayTextPro.gameObject.SetActive(true);
        DisplayTextPro.text = string.Empty;
        parasShowText.gameObject.SetActive(false);
        resumeTip.gameObject.SetActive(false);
        BTextTyping = true;
        for(int i = 0; i < _preShowTextContent.Length; i++) 
        { 
            DisplayTextPro.text += _preShowTextContent[i];
            yield return _typeWaitTime;
        }
        BTextTyping = false;
    } 
    //TODO:响应点击，快速展示所有内容
    public IEnumerator TextType(string content)
    {
        _preShowTextContent = string.Empty;
        DisplayTextPro.gameObject.SetActive(true);
        DisplayTextPro.text = string.Empty;
        parasShowText.gameObject.SetActive(false);
        resumeTip.gameObject.SetActive(false);
        if (parasShowText.gameObject.activeSelf)
            parasShowText.text = string.Empty;
        _preShowTextContent = content;
        BTextTyping = true;
        for (int i = 0; i < _preShowTextContent.Length; i++)
        {
            DisplayTextPro.text += _preShowTextContent[i];
            yield return _typeWaitTime;
        }
        yield return _finishWaitTime;
        BTextTyping = false;
    }
    public bool GetIsTextShowEnd()
    {
        return BTextTyping;
    }

    public TextMeshProUGUI parasShowText;
    public TextMeshProUGUI resumeTip;

    private WaitForSeconds _longParasWaitTimeForEachCharacters = new WaitForSeconds(0.1f);
    private WaitForSeconds _normalParasWaitTimeForEachCharacters = new WaitForSeconds(0.15f);
    public IEnumerator TextTypeByParagraph(List<string> contentByPages)
    {
        if(DisplayTextPro.gameObject.activeSelf)
            DisplayTextPro.text = string.Empty;
        DisplayTextPro.gameObject.SetActive(false);
        parasShowText.gameObject.SetActive(true);
        parasShowText.text = string.Empty;
        int pageCurIndex = 0;
        while (pageCurIndex < contentByPages.Count)
        {
            _BGetSpaceDown = false;
            resumeTip.gameObject.SetActive(false);
            string[] paras = contentByPages[pageCurIndex].Split('\n');
            BTextTyping = true;
            parasShowText.text = string.Empty;

            yield return new WaitForSeconds(1.5f);
            for (int i = 0; i < paras.Length; i++)
            {
                parasShowText.text += paras[i]+"\n";
                //Debug.Log(paras[i] + "  : " + paras[i].Length);
                if (paras[i].Length < 3)
                {
                    //空符号过短不等待
                }
                //太长少等待
                else if (paras[i].Length > 50)
                {
                    for (int j = 0; j < paras[i].Length; j++)
                    {
                        yield return _longParasWaitTimeForEachCharacters;
                        if(_BGetSpaceDown)
                        {
                            _BGetSpaceDown = false;
                            break;
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < paras[i].Length; j++)
                    {
                        yield return _normalParasWaitTimeForEachCharacters;
                        if (_BGetSpaceDown)
                        {
                            _BGetSpaceDown = false;
                            break;
                        }
                    }
                }
            }

            resumeTip.gameObject.SetActive(true);
            //等待空格
            while (!Input.GetKeyDown(KeyCode.Space))
            {
                yield return null;
            }
            pageCurIndex++;
            yield return new WaitForSeconds(0.5f);
        }

        resumeTip.gameObject.SetActive(false);
        BTextTyping = false;
        parasShowText.text = string.Empty;
        //结束
    }

    private bool _BGetSpaceDown;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            _BGetSpaceDown = true;
        }
    }


    /*public void BeginFadeIn()
    {
        this.gameObject.SetActive(true);
        _coverGroup.alpha = 0f;
        if (_fadeIE != null)
            StopCoroutine(_fadeIE);
        _fadeIE = FadeIn();
        StartCoroutine(_fadeIE);
    }
    public void BeginFadeOut()
    {
        _coverGroup.alpha = 1f;
        if (_fadeIE != null)
            StopCoroutine(_fadeIE);
        _fadeIE = FadeOut();
        StartCoroutine(_fadeIE);
    }*/

    private IEnumerator _fadeIE;

    private WaitForSeconds _waitTime=new WaitForSeconds(0.5f);
    public IEnumerator FadeIn()
    {
        this.gameObject.SetActive(true);
        _coverGroup.alpha = 0f;
        BFading = true;
        while (_coverGroup.alpha<1f)
        {
            _coverGroup.alpha += Time.deltaTime * fadeSpeed; 
            //_coverGroup.alpha = Mathf.Lerp(_coverGroup.alpha, 1f, Time.deltaTime * fadeSpeed); 
           
            yield return null; 
            if (_coverGroup.alpha > 0.99f)
            {
                _coverGroup.alpha = 1f;
                yield return _waitTime;
                yield break;
            }
        }
    }
    public IEnumerator FadeOut()
    {
        _coverGroup.alpha = 1f;
        while (_coverGroup.alpha > 0f)
        {
            _coverGroup.alpha -= Time.deltaTime * fadeSpeed; 
            //_coverGroup.alpha = Mathf.Lerp(_coverGroup.alpha, 0f, Time.deltaTime * fadeSpeed);
            yield return null;
            if(_coverGroup.alpha<0.01f)
            {
                _coverGroup.alpha = 0f;
                break;
            }
        }
        BFading = false;

        //检查是否有残余文字，将其清楚
        if (DisplayTextPro.gameObject.activeSelf)
            DisplayTextPro.text = string.Empty;
        if (parasShowText.gameObject.activeSelf)
            parasShowText.text = string.Empty;
        this.gameObject.SetActive(false);
    }
     
}
