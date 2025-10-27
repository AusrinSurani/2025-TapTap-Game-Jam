using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class EmailController : MonoBehaviour
{
    [Header("事件监听")] public VoidEventSO chapterChangeEvent;
    
    [Header("隐藏信件")]
    public GameObject email4;
    public GameObject email5;
    
    private int numOfEmails = 5;
    const string HAVE_RAED_EMAIL_DATA_FILE = "HaveReadEmailData.txt";

    private EmailNameButton[] emailNameButtons;
    public bool haveAllRead = false;

    private void Awake()
    {
        emailNameButtons = GetComponentsInChildren<EmailNameButton>();
        
        if(File.Exists(Path.Combine(Application.persistentDataPath,HAVE_RAED_EMAIL_DATA_FILE)))
        {
            LoadEmailsData();
        }
        else
        {
            //第一次玩没有数据，准备默认的数据
            SetDefaultAudioSettingData();
        }
    }

    private void OnEnable()
    {
        chapterChangeEvent.OnEventRaise += OnChapterChange;
    }

    public void OnDisable()
    {
        chapterChangeEvent.OnEventRaise -= OnChapterChange;
    }

    private void OnChapterChange()
    {
        int currentDay = GameFlowManager.Instance.currentDay;
        
        //邮件随着天数增加
        switch (currentDay)
        {
            case 1:
                email4.SetActive(false);
                email5.SetActive(false);
                break;
            case 2:
                email5.SetActive(false);
                break;
        }
        
        Debug.Log(currentDay+"day,邮件设置完毕");
        
        LoadEmailsData();
    }

    [Serializable]public class HaveReadEmailData
    {
        public List<bool> haveReadEmailData = new List<bool>();
        public bool haveAllRead;
    }
    
    public void SetDefaultAudioSettingData()
    {
        var data = new HaveReadEmailData();

        for (int i = 0; i < 5; i++)
        {
            data.haveReadEmailData.Add(false);
        }

        haveAllRead = false;
        
        SaveSystem.SaveByJson(HAVE_RAED_EMAIL_DATA_FILE, data);
        ApplyData();
    }
    
    public void SaveEmailsData()
    {
        emailNameButtons = GetComponentsInChildren<EmailNameButton>();
        
        var data = new HaveReadEmailData();
        
        haveAllRead = true;
        foreach (var t in emailNameButtons)
        {
            data.haveReadEmailData.Add(t.haveRead);
            if (!t.haveRead)
            {
                haveAllRead = false;
            }
        }
        
        //补足所有的邮件状态
        if (emailNameButtons.Length < numOfEmails)
        {
            for (int i = 0; i < numOfEmails - emailNameButtons.Length; i++)
            {
                data.haveReadEmailData.Add(false);
            }
        }
        
        data.haveAllRead = haveAllRead;
        SaveSystem.SaveByJson(HAVE_RAED_EMAIL_DATA_FILE, data);
    }
    
    public void LoadEmailsData()
    {
        emailNameButtons = GetComponentsInChildren<EmailNameButton>();
        var savedData = SaveSystem.LoadFromJson<HaveReadEmailData>(HAVE_RAED_EMAIL_DATA_FILE);

        for (int i = 0; i < emailNameButtons.Length; i++)
        {
            emailNameButtons[i].haveRead = savedData.haveReadEmailData[i];
            haveAllRead = savedData.haveAllRead;
        }
        
        ApplyData();
    }

    private void ApplyData()
    {
        //刷新贴图，刷新有无新消息的动画

        foreach (var t in emailNameButtons)
        {
            t.ReFresh();
        }

        GetComponentInParent<EmailButton>().newEmail.SetBool("New", !haveAllRead);
    }

#if UNITY_EDITOR
    [UnityEditor.MenuItem("Developer/Delete Emails Data")]
#endif
    public static void DeleteDataOfEmails()
    {
        SaveSystem.DeleteSavedFile(HAVE_RAED_EMAIL_DATA_FILE);
    }
}