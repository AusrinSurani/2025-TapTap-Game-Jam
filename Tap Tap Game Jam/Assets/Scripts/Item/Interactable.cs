using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Interactable:MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public ItemData itemData;
    public GameObject outLine;

    private void Start()
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
        if(DialogManager.Instance.dialogBox.activeSelf == true)
            return;
        
        Debug.Log(itemData.itemName);
        DialogManager.Instance.ShowMessage(itemData.description);
    }

    public void OnValidate()
    {
        if(itemData == null)
            return;
        
        gameObject.GetComponent<SpriteRenderer>().sprite = itemData.icon;
        outLine.GetComponent<SpriteRenderer>().sprite = itemData.icon;
    }
}