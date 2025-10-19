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
    private bool haveAllRead = false;

    private void Awake()
    {
        emailNameButtons = GetComponentsInChildren<EmailNameButton>();
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
        ChapterOfGame currentChapter = GameFlowManager.Instance.currentChapter;
        if (currentChapter == ChapterOfGame.ChapterWaiter)
        {
            email4.SetActive(true);
        }
        else if (currentChapter == ChapterOfGame.ChapterProgrammer)
        {
            email4.SetActive(true);
            email5.SetActive(true);
        }
        
        SaveEmailsData();
        LoadEmailsData();
    }

    private void Start()
    {
        /*Debug.Log(emailNameButtons.Length);
        Debug.Log(File.Exists(Path.Combine(Application.persistentDataPath,HAVE_RAED_EMAIL_DATA_FILE)));*/
        
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

    private void Update()
    {
    }

    [Serializable]public class HaveReadEmailData
    {
        public List<bool> haveReadEmailData = new List<bool>();
        public bool haveAllRead;
    }
    
    public void SetDefaultAudioSettingData()
    {
        var data = new HaveReadEmailData();

        for (int i = 0; i < numOfEmails; i++)
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
        
        data.haveAllRead = haveAllRead;
        SaveSystem.SaveByJson(HAVE_RAED_EMAIL_DATA_FILE, data);
    }
    
    public void LoadEmailsData()
    {
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
        
        for (int i = 0; i < emailNameButtons.Length; i++)
        {
            emailNameButtons[i].ReFresh();
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