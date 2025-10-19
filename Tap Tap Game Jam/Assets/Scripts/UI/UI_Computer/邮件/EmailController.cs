using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class EmailController : MonoBehaviour
{
    private int numOfEmails = 5;
    const string HAVE_RAED_EMAIL_DATA_FILE = "HaveReadEmailData.txt";

    private EmailNameButton[] emailNameButtons;
    private bool haveAllRead = false;

    private void Start()
    {
        emailNameButtons = GetComponentsInChildren<EmailNameButton>();
        
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