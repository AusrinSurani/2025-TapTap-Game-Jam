using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bed : TravelPlan
{
    [Header("事件广播")] public VoidEventSO getFirstMapEvent;
    
    private bool haveGetMap = false;
    
    public override void Update()
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
                    StartCoroutine(BedMapFadeCoroutine());
                }
            }
                
            isShowing = false;
        }
    }
    
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (haveGetMap)
        {
            DialogManager.Instance.ShowMessage("我已经拿到这里的地图了");
            return;
        }
        
        if (!haveGetMap)
        {
            haveGetMap = true;
            AudioManager.Instance.AudioOncePlay(AudioManager.Instance.getMap);
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
        
        if (specialImage != null)
        {
            specialImage.SetActive(true);
            isShowing = true;
        }
    }
    
    private IEnumerator BedMapFadeCoroutine()
    {
        getFirstMapEvent.RaiseEvent();
        specialImage.GetComponent<BlankController>().GenerateItem(specialImage.GetComponent<BlankController>().inventoryItemData);
        yield return FadeCoroutine(specialImage.GetComponent<BlankController>().mapSprite,backDuration);
        Destroy(specialImage.GetComponent<BlankController>().mapSprite);
    }
}