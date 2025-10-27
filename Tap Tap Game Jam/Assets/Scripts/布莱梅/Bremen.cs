using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bremen : MonoBehaviour,IPointerClickHandler
{
    [Header("信纸--旅行计划7")]
    public Sprite completeLetter;
    public GameObject plan7;
    
    [Header("事件监听")] public VoidEventSO get7MapsEvent;
    
    [Header("所有对话")] 
    public TextAsset aboutVase;
    public TextAsset aboutTravelPlan7;
    public TextAsset lastDialog;
    
    private bool canTalkAboutVase = false;
    private bool haveTalkAboutVase = false;
    
    private bool canTalkAboutTravelPlan7 = false;
    private bool haveTalkAboutTravelPlan7 = false;

    private void OnEnable()
    {
        get7MapsEvent.OnEventRaise += () => canTalkAboutTravelPlan7 = true;
    }

    private void OnDisable()
    {
        get7MapsEvent.OnEventRaise -= () => canTalkAboutTravelPlan7 = false;
    }

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Space) || !DialogManager.Instance.IsDialogEnded())
        {
            return;
        }

        StartCoroutine(Plan7Vanish());
    }

    public void TriggerVase()
    {
        canTalkAboutVase = true;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (canTalkAboutTravelPlan7 && !haveTalkAboutTravelPlan7)
        {
            haveTalkAboutTravelPlan7 = true;
            DialogManager.Instance.StartDialog(aboutTravelPlan7);
            StartCoroutine(RaisePlan7());
            return;
        }
        
        if (canTalkAboutVase && !haveTalkAboutVase)
        {
            DialogManager.Instance.StartDialog(aboutVase);
            /*haveTalkAboutVase = true;*/
            return;
        }
    }

    private IEnumerator RaisePlan7()
    {
        yield return new WaitUntil(() =>
            DialogManager.Instance.GetComponentInChildren<ClickableTextController>().
                GetComponent<TextMeshProUGUI>().text.Contains("他从围裙口袋里掏出一个信封递给我"));
        
        plan7.SetActive(true);
        
        float timer = 0;

        while (timer < 0.7f)
        {
            timer += Time.deltaTime;
            plan7.GetComponent<SpriteRenderer>().color =
                new Color(1,1,1, Mathf.Lerp(0,1,timer/0.7f));
            yield return null;
        }
    }

    private IEnumerator Plan7Vanish()
    {
        float timer = 0;
        
        while (timer < 0.7f)
        {
            timer += Time.deltaTime;
            plan7.GetComponent<SpriteRenderer>().color =
                new Color(1,1,1, Mathf.Lerp(1,0,timer/0.7f));
            yield return null;
        }
    }

    public void TriggerComplete()
    {
        plan7.GetComponent<SpriteRenderer>().sprite = completeLetter;
    }
}