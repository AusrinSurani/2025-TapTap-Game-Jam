using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Interactable:MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public ItemData itemData;
    public GameObject outLine;

    public virtual void Start()
    {
        outLine.SetActive(false);
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        //显示边沿
        PlayHoverSfx();
        outLine.SetActive(true);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        outLine.SetActive(false);
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
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
    }

    public void OnValidate()
    {
        if (itemData == null)
        {
            Debug.Log("没有物品数据");
            return;
        }

        if (itemData.icon == null)
        {
            Debug.Log("没有物品图");
        }

        if (itemData.outline == null)
        {
            Debug.Log("没有边框图");
        }

        if (outLine == null)
        {
            Debug.Log("未分配边框");
            return;
        }
        
        gameObject.GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = "Interactable-" + itemData.itemName;
        outLine.GetComponent<SpriteRenderer>().sprite = itemData.outline;
    }

    public virtual void PlayHoverSfx()
    {
        AudioManager.Instance.AudioOncePlay(AudioManager.Instance.hoverSfx);
    }

    public virtual void PlayClickSfx()
    {
        AudioManager.Instance.AudioOncePlay(AudioManager.Instance.clickSfx);
    }
}