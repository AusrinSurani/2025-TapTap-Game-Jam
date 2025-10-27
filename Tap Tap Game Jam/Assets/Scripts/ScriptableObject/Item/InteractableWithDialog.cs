using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InteractableWithDialog : Interactable
{
    private GameObject dialogMask;
    
    [Header("显示物品特写的UI")]
    public GameObject imageForItem;
    private CanvasGroup canvasGroup;
    private bool isShowing = false;
    
    public float raiseDuration = 0.7f;
    public float backDuration = 0.5f;

    public bool needDialog = true;
    
    public override void Start()
    {
        dialogMask = DialogManager.Instance.mask;
        
        canvasGroup = imageForItem.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
    }

    public virtual void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Space) ||DialogManager.Instance.IsTyping() || !DialogManager.Instance.IsOnLastMessage()
            || !DialogManager.Instance.IsDialogEnded())
        {
            return;
        }
            
        if (isShowing)
        {
            if (itemData.closeUp != null)
            {
                //配合文本一起消失
                canvasGroup.blocksRaycasts = false;
                StartCoroutine(FadeCoroutine(1, 0, backDuration));
            }
                
            isShowing = false;
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if(dialogMask.activeSelf == false)
            return;
        
        base.OnPointerEnter(eventData);
    }
    
    public override void OnPointerClick(PointerEventData eventData)
    {
        PlayClickSfx();
        
        if (dialogMask.activeSelf == false)
        {
            return;
        }

        if (!needDialog)
        {
            base.OnPointerClick(eventData);
        }
        else
        {
            DialogManager.Instance.StartDialog(itemData.dialog);
        }
        
        //显示特写图
        if (itemData.closeUp != null)
        {
            imageForItem.SetActive(true);
            imageForItem.GetComponent<Image>().sprite = itemData.closeUp;
            StartCoroutine(FadeCoroutine(0, 1, raiseDuration));
        }
        
        isShowing = true;
    }

    private IEnumerator FadeCoroutine(float start, float end, float duration)
    {
        canvasGroup.alpha = 0;
        float timer = 0;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, end, timer / duration);
            yield return null;
        }
    }
}
