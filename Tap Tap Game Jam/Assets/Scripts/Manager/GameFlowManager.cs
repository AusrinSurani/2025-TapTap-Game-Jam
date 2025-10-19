using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ChapterOfGame
{
    ChapterDancer,
    ChapterWaiter,
    ChapterProgrammer
}

public class GameFlowManager : Singleton<GameFlowManager>
{
    [Header("当前章节")] 
    public ChapterOfGame currentChapter;
    
    [Header("事件广播")]
    public VoidEventSO chapterChangeEvent;
    
    const string CHAPTER_DATA_FILE = "ChapterData.txt";
    
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
        //TEST
        if (Input.GetKeyDown(KeyCode.L))
        {
            ChangeChapter(ChapterOfGame.ChapterWaiter);
        }
    }

    public void ChangeChapter(ChapterOfGame chapter)
    {
        currentChapter = chapter;
        
        //改变章节立马保存，然后刷新所有物体数据
        SaveChapterData();
        
        chapterChangeEvent.RaiseEvent();
    }

    #region 保存游戏章节

    [Serializable]public class GameProgress
    {
        public ChapterOfGame chapter;
    }
    
    public void SetDefaultChapterData()
    {
        var data = new GameProgress();
        
        //第一次玩，从舞者篇开始
        data.chapter = ChapterOfGame.ChapterDancer;
        currentChapter = data.chapter;
        
        SaveSystem.SaveByJson(CHAPTER_DATA_FILE, data);
        
        //通知所有相关物体改变数据
        chapterChangeEvent.OnEventRaise();
    }

    public void SaveChapterData()
    {
        var data = new GameProgress();
        data.chapter = currentChapter;
        SaveSystem.SaveByJson(CHAPTER_DATA_FILE, data);
    }
    
    public void LoadChapterData()
    {
        var savedData = SaveSystem.LoadFromJson<GameProgress>(CHAPTER_DATA_FILE);
        currentChapter = savedData.chapter;
        
        //通知所有相关物体改变数据
        chapterChangeEvent.OnEventRaise();
    }

    #endregion
}