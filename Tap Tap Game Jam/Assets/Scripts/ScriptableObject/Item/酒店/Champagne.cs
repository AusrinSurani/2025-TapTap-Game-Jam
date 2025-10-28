using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Champagne : TravelPlan
{
    [Header("香槟的标签")]
    public bool hasNextCloseUp = true;
    private bool isInNextCloseUp = false;
    public Sprite noteSprite;

    public override void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Space) ||DialogManager.Instance.IsTyping() || !DialogManager.Instance.IsOnLastMessage()
            || !DialogManager.Instance.IsDialogEnded())
        {
            return;
        }

        if (hasNextCloseUp && !isInNextCloseUp && specialImage.activeSelf)
        {
            DialogManager.Instance.ShowMessage(itemData.nextDescriptions);
            
            //切换背面图
            specialImage.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
            specialImage.GetComponent<SpriteRenderer>().sprite = noteSprite;
            StartCoroutine(RaiseCoroutine(specialImage,raiseDuration));
            
            //唤醒碰撞体
            foreach (var blankForWord in specialImage.GetComponent<BlankController>().blankForWords)
            {
                blankForWord.GetComponent<BoxCollider2D>().enabled = true;
                //顺便更新图像
                blankForWord.RefreshImage();
            }
            
            isInNextCloseUp = true;
            return;
        }
            
        if (isShowing)
        {
            if (specialImage != null)
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
                isInNextCloseUp =  false;
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
        
        DialogManager.Instance.StartDialog(itemData.dialog);
        
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
}