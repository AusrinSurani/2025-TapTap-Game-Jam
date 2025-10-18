using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonOfDoor : Interactable
{
    public GameObject dancer;
    public TextAsset dialogWithDancer0;
    public TextAsset dialogWithDancer1;

    private bool isDancerCome = false;
    
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (isDancerCome)
        {
            DialogManager.Instance.ShowMessage("我最好等处理完现在的患者再接待下一位");
            return;
        }
        
        //TODO:触发事件，可能要黑屏，然后患者出现
        StartCoroutine(OnDancerComeIn());
    }

    private void Update()
    {
        
    }

    private IEnumerator DancerComeIn()
    {
        if (UIManager.Instance.coverFader != null)
        { 
            yield return UIManager.Instance.coverFader.FadeIn();
         
            yield return UIManager.Instance.coverFader.TextType("舞女来了");

            dancer.SetActive(true);
            isDancerCome = true;
           
            yield return UIManager.Instance.coverFader.FadeOut();
        }
    }

    private IEnumerator OnDancerComeIn()
    {
        yield return DancerComeIn();
        
        DialogManager.Instance.StartDialog(dialogWithDancer0);
    }
}
