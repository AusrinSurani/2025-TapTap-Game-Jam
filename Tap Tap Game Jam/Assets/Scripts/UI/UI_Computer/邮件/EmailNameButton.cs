using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EmailNameButton : BounceButton
{
    [Header("信件文本")]
    public MailData  mailData;

    public TextMeshProUGUI title;
    public TextMeshProUGUI from;
    public TextMeshProUGUI content;

    public Sprite spriteOfNotRead;
    public Sprite spriteOfHaveRead;

    public bool haveRead =  false;

    private void Start()
    {
        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = 
            mailData.title.Substring(0,6) + "...";
        
        from.text = "";
        content.text = "";
        title.text = "";
        GetComponent<Image>().sprite = haveRead ? spriteOfHaveRead : spriteOfNotRead;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        from.text = "From " + mailData.sender + "\n" + "To @尼安德·华莱士";
        content.text = mailData.content;
        title.text = mailData.title;
        haveRead = true;
        SetDefaultSprite(spriteOfHaveRead);
        
        GetComponentInParent<EmailController>().SaveEmailsData();
        GetComponentInParent<EmailController>().LoadEmailsData();
    }

    public void ReFresh()
    {
        GetComponent<Image>().sprite = haveRead ? spriteOfHaveRead : spriteOfNotRead;
    }

    public override Sprite GetDefaultSprite()
    {
        return haveRead ? spriteOfHaveRead : spriteOfNotRead;
    }
}
