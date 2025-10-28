using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PaperBehindPizza : TravelPlan
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (specialImage.GetComponent<BlankController>().haveGetMap)
        {
            DialogManager.Instance.ShowMessage("我已经拿到了这里的地图碎片了");
            return;
        }
        
        //第二章：大部分物品有专属音效
        switch (itemData.name)
        {
            case "书":
                AudioManager.Instance.AudioOncePlay(AudioManager.Instance.raiseBook);
                break;
            case "旅行计划" or "文件" or "比萨" or "咖啡卡片":
                AudioManager.Instance.AudioOncePlay(AudioManager.Instance.raisePaper);
                break;
            case "香槟":
                AudioManager.Instance.AudioOncePlay(AudioManager.Instance.raiseWine);
                break;
            default:
                PlayClickSfx();
                break;
        }
        
        if(itemData.descriptions == null|| itemData.descriptions.Length == 0)
        {
            //显示一句
            DialogManager.Instance.ShowMessage(itemData?.description);
        }
        else
        {
            //显示多句
            DialogManager.Instance.ShowMessage(itemData?.descriptions);
        }

        if (specialImage != null && specialImage.GetComponent<SpriteRenderer>() != null)
        {
            specialImage.SetActive(true);
            
            foreach (var t in specialImage.GetComponent<BlankController>().blankForWords)
            {
                t.GetComponent<BoxCollider2D>().enabled = true;
            }
            
            specialImage.GetComponent<SpriteRenderer>().sprite = itemData.closeUp;
            
            //唤醒碰撞体
            foreach (var blankForWord in specialImage.GetComponent<BlankController>().blankForWords)
            {
                blankForWord.GetComponent<BoxCollider2D>().enabled = true;
                //顺便更新图像
                blankForWord.RefreshImage();
            }
            
            StartCoroutine(RaiseCoroutine(specialImage,raiseDuration));
            isShowing = true;
        }
    }
}