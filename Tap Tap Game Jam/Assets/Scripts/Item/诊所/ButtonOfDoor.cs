using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonOfDoor : Interactable
{
    public GameObject dancer;
    
    public override void OnPointerClick(PointerEventData eventData)
    {
        //TODO:触发事件，可能要黑屏，然后患者出现
        StartCoroutine(DancerComeIn());
    }

    private IEnumerator DancerComeIn()
    {
        if (UIManager.Instance.coverFader != null)
        { 
            yield return UIManager.Instance.coverFader.FadeIn();
         
            yield return UIManager.Instance.coverFader.TextType("舞女来了");

            dancer.SetActive(true);
           
            yield return UIManager.Instance.coverFader.FadeOut();
        }
    }
}
