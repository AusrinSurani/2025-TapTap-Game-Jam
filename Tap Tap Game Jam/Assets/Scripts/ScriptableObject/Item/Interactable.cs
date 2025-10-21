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
        outLine.SetActive(true);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        outLine.SetActive(false);
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(itemData?.descriptions.Length == 0);
        
        if(itemData?.descriptions.Length == 0)
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
}