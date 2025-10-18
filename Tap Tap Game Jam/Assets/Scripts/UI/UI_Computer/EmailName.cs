using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class EmailName : BounceButton
{
    [Header("信件文本")]
    public MailData  mailData;

    public TextMeshProUGUI title;
    public TextMeshProUGUI from;
    public TextMeshProUGUI content;

    private void Start()
    {
        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = 
            mailData.title.Substring(0,4) + "...";
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        from.text = "From " + mailData.sender;
        content.text = mailData.content;
        title.text = mailData.title;
    }
}
