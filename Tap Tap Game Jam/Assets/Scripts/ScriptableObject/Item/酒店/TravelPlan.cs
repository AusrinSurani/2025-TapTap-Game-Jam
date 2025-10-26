using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TravelPlan : Interactable
{
    [Header("可交互特写")]
    public GameObject specialImage;
    protected bool isShowing = false;

    [Header("参数")]
    public float raiseDuration;
    public float backDuration;
    
    public override void Start()
    {
        base.Start();
        specialImage.SetActive(false);
    }

    public virtual void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Space) || DialogManager.Instance.IsTyping() || !DialogManager.Instance.IsOnLastMessage())
        {
            return;
        }
            
        if (isShowing)
        {
            if (specialImage != null && specialImage.activeSelf == true)
            {
                if (specialImage.GetComponent<BlankController>().haveGetMap && 
                    specialImage.GetComponent<BlankController>().mapSprite != null)
                {
                    //地图特写也一起消失
                    StartCoroutine(MapFadeCoroutine());
                }
                else
                {
                    //特写配合文本一起消失
                    StartCoroutine(FadeCoroutine(specialImage,backDuration));
                }
            }
                
            isShowing = false;
        }
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (specialImage.GetComponent<BlankController>().haveGetMap)
        {
            DialogManager.Instance.ShowMessage("我已经拿到了这里的地图碎片了");
            return;
        }
        
        base.OnPointerClick(eventData);

        if (specialImage != null && specialImage.GetComponent<SpriteRenderer>() != null)
        {
            specialImage.SetActive(true);
            
            foreach (var t in specialImage.GetComponent<BlankController>().blankForWords)
            {
                t.GetComponent<BoxCollider2D>().enabled = true;
            }
            
            specialImage.GetComponent<SpriteRenderer>().sprite = itemData.closeUp;
            StartCoroutine(RaiseCoroutine(specialImage,raiseDuration));
            isShowing = true;
        }
    }

    #region 淡入淡出
    
    protected IEnumerator RaiseCoroutine(GameObject imageGameObject,float duration)
    {
        float timer = 0;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            imageGameObject.GetComponent<SpriteRenderer>().color =new Color (1,1,1,Mathf.Lerp(0, 1, timer / duration)) ;
            yield return null;
        }
    }
    protected IEnumerator FadeCoroutine(GameObject imageGameObject,float duration)
    {
        float timer = 0;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            imageGameObject.GetComponent<SpriteRenderer>().color =new Color (1,1,1,Mathf.Lerp(1, 0, timer / duration)) ;

            if (duration - timer < 0.02f)
            {
                specialImage.SetActive(false);
            }
            yield return null;
        }
    }

    protected IEnumerator MapFadeCoroutine()
    {
        yield return FadeCoroutine(specialImage.GetComponent<BlankController>().mapSprite,backDuration);
        Destroy(specialImage.GetComponent<BlankController>().mapSprite);
        yield return new WaitForSeconds(backDuration);
        yield return FadeCoroutine(specialImage, backDuration);
    }

    #endregion
}