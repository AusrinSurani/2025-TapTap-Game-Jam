using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ChapterOfGame
{
    NoOne,
    ChapterDancer,
    ChapterWaiter,
    ChapterProgrammer
}

public class GameFlowManager : Singleton<GameFlowManager>
{
    [Header("当前章节")] 
    public ChapterOfGame currentChapter;
    public int currentDay;
    public bool currentIsOver;
    
    [Header("事件广播")]
    public VoidEventSO chapterChangeEvent;
    
    const string CHAPTER_DATA_FILE = "ChapterData.txt";
    public  ChapterOfGame[] listOfChapter =
    {
        ChapterOfGame.NoOne ,
        ChapterOfGame.ChapterDancer,
        ChapterOfGame.ChapterWaiter,
        ChapterOfGame.ChapterProgrammer
    };

    private void Start()
    {
        if(File.Exists(Path.Combine(Application.persistentDataPath,CHAPTER_DATA_FILE)))
        {
            LoadChapterData();
        }
        else
        {
            //第一次玩没有数据，准备默认的数据
            SetDefaultChapterData();
        }
    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            chapterChangeEvent.RaiseEvent();
        }
    }

    public void ChangeChapter(ChapterOfGame chapter, bool isOver, int day)
    {
        currentChapter = chapter;
        currentIsOver = isOver;
        currentDay = day;
        
        Debug.Log(currentChapter);
        Debug.Log(currentIsOver);
        Debug.Log(currentDay);
        
        //改变章节立马保存，然后刷新所有物体数据
        SaveChapterData();
        
        chapterChangeEvent?.RaiseEvent();
    }

    #region 保存游戏章节

    [Serializable]public class GameProgress
    {
        public ChapterOfGame chapter;//当前章节
        public bool isOver;//当前章节有没有治疗结束
        public int day;//当前日期（用来判断邮箱的邮件数量）
    }
    
    public void SetDefaultChapterData()
    {
        var data = new GameProgress();
        
        //第一次玩，从舞者篇开始
        data.chapter = ChapterOfGame.NoOne;
        data.isOver = false;
        data.day = 1;
        
        currentChapter = data.chapter;
        currentIsOver = data.isOver;
        currentDay = data.day;
        
        SaveSystem.SaveByJson(CHAPTER_DATA_FILE, data);
        
        //通知所有相关物体改变数据
        chapterChangeEvent?.RaiseEvent();
    }

    public void SaveChapterData()
    {
        var data = new GameProgress 
            { chapter = currentChapter, isOver = currentIsOver, day = currentDay };
        SaveSystem.SaveByJson(CHAPTER_DATA_FILE, data);
    }
    
    public void LoadChapterData()
    {
        var savedData = SaveSystem.LoadFromJson<GameProgress>(CHAPTER_DATA_FILE);
        currentChapter = savedData.chapter;
        currentIsOver = savedData.isOver;
        currentDay = savedData.day;
        
        //通知所有相关物体改变数据
        chapterChangeEvent?.RaiseEvent();
    }

    #endregion
}