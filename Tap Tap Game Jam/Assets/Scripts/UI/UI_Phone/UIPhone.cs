using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPhone : BaseUI
{
    [Header("事件监听")]
    public VoidEventSO openPhone;

    public GameObject mask;

    private void Start()
    {
        mask.SetActive(false);
    }

    private void OnEnable()
    {
        openPhone.OnEventRaise += StartMove;
        openPhone.OnEventRaise += UseMask;
    }

    private void OnDisable()
    {
        openPhone.OnEventRaise -= StartMove;
        openPhone.OnEventRaise -= UseMask;
    }
    
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            mask.SetActive(false);
            MoveBack();
        }
    }

    private void UseMask()
    {
        mask.SetActive(true);
    }

    public void ShowInformationOf_Beta()
    {
        DialogManager.Instance.ShowMessage("默尔索·贝塔……这是报复社会吗？\n话说好像之前也有叫贝塔的客人来着……");
    }

    public void ShowInformationOf_Nina()
    {
        DialogManager.Instance.ShowMessage("是很有名的舞蹈家吧。\n之前有客人答谢时送了她的演出门票，她的舞蹈只能说是麻雀啄了牛屁股——确实牛逼。");
    }
    
    public void ShowInformationOf_GoldenString()
    {
        DialogManager.Instance.ShowMessage("看起来像是新的作品？\n国庆日那两天客人太多，都没怎么关注新闻。算起来……好久没休假了。");
    }
    
    public void ShowInformationOf_Restaurant()
    {
        DialogManager.Instance.ShowMessage("威尔逊大饭店是曼庚生意最好的饭馆，他们家的出品都很赞，居然开了一百年了啊。\n靠，是咖啡喝多了吗，怎么又饿了？");
    }
    
    public void ShowInformationOf_Narina()
    {
        DialogManager.Instance.ShowMessage("这几年不知道从哪杀出来的天后，经常在电视上看到她。\n听说她来自一个很冷的国度，音译的话是叫“莫思鸽”？奇怪的名字。");
    }
}