using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;

public class LoadingCoverFader : MonoBehaviour
{
    public float fadeSpeed=1f;
    private CanvasGroup _coverGroup;

    public TextMeshProUGUI showText;
    private void Awake()
    {
        _coverGroup = GetComponent<CanvasGroup>();
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

    public void ShowText(string text)
    {
        //TODO:打字效果
    }

    //TODO
    public bool GetIsTextShowEnd()
    {
        return false;
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
        this.gameObject.SetActive(false);
    }
    /**/
}
