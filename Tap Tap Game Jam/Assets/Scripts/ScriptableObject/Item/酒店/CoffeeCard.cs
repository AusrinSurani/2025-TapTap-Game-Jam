using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeCard : TravelPlan
{
    [Header("咖啡卡片反面")]
    public bool hasNextCloseUp = true;
    private bool isInNextCloseUp = false;
    public Sprite coffeeSprite;

    public override void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Space) ||DialogManager.Instance.IsTyping() || !DialogManager.Instance.IsOnLastMessage())
        {
            return;
        }

        if (hasNextCloseUp && !isInNextCloseUp && specialImage.activeSelf == true)
        {
            DialogManager.Instance.ShowMessage(itemData.nextDescriptions);
            
            //切换背面图
            specialImage.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
            specialImage.GetComponent<SpriteRenderer>().sprite = coffeeSprite;
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
}
