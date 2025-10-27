using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//类似InteractableInDream，但是可以收集
public class CollectableItem : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    public SpriteRenderer outline;
    public CanvasGroup imageForItem;
    public InventoryItemData inventoryItemData;
    public GameObject inventoryItemPrefab;
    public GameObject container;

    public GameObject dialogMask;

    [Header("特写过渡参数")] 
    public float raiseDuration;
    public float backDuration;
    
    private bool isShowing = false;
    
    private void Start()
    {
        outline.enabled = false;
        dialogMask = DialogManager.Instance.mask;
    }
    
    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Space) ||DialogManager.Instance.IsTyping() || !DialogManager.Instance.IsOnLastMessage())
        {
            return;
        }
            
        if (isShowing)
        {
            //配合文本一起消失
            imageForItem.blocksRaycasts = false;
            StartCoroutine(FadeCoroutine(1, 0, backDuration));

            isShowing = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(dialogMask.activeSelf == false)
            return;
        
        //TODO:消失，生成一个物体在container那里,然后触发对话,显示特写
        isShowing = true;
        DialogManager.Instance.ShowMessage(inventoryItemData.descriptions);
        StartCoroutine(VanishCoroutine());
        StartCoroutine(FadeCoroutine(0,1,raiseDuration));
        GenerateItem(inventoryItemData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(dialogMask.activeSelf == false)
            return;
        outline.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        outline.enabled = false;
    }

    private IEnumerator VanishCoroutine()
    {
        float timer = 0;

        while (timer < 0.5f)
        {
            timer += Time.deltaTime;
            GetComponent<SpriteRenderer>().color = 
                new Color(1, 1, 1, Mathf.Lerp(1, 0, timer / 0.5f));
        }
        
        //TODO：直到玩家按下空格，并等待特写淡出才销毁
        yield return new WaitUntil(GetIsNotShowing);
        
        yield return new WaitForSeconds(backDuration);
        
        gameObject.SetActive(false);
    }

    private bool GetIsNotShowing()
    {
        return !isShowing;
    }

    private void GenerateItem(InventoryItemData itemData)
    {
        int numOfItems = container.transform.childCount;
        
        float yPosition = -1.5f * numOfItems + 4 + container.transform.position.y;

        GameObject newItem = Instantiate(inventoryItemPrefab, new Vector3(
            container.transform.position.x, yPosition, 9.938788f), Quaternion.identity );
        
        newItem.tag = "ItemFlag";
        newItem.GetComponent<DraggableItem>().SetUp(itemData);
        newItem.transform.SetParent(container.transform);
    }
    
    private IEnumerator FadeCoroutine(float start, float end, float duration)
    {
        imageForItem.GetComponent<Image>().sprite = inventoryItemData.closeUp;
        imageForItem.alpha = 0;
        float timer = 0;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            imageForItem.alpha = Mathf.Lerp(start, end, timer / duration);
            yield return null;
        }
    }
}