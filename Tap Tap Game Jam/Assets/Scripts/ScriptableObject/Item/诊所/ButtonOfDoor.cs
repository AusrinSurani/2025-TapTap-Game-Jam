using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonOfDoor : Interactable
{
    public GameObject dancer;
    public GameObject waiter;
    public GameObject programmer;
    public TextAsset dialogWithDancer0;
    public TextAsset dialogWithDancer1;
    public TextAsset dialogWithWaiter0;
    public TextAsset dialogWithWaiter1;
    public TextAsset dialogWithProgrammer0;
    public TextAsset dialogWithProgrammer1;

    private bool isPatientCome = false;
    private bool isStartEndDialog =  false;
    
    private Dictionary<ChapterOfGame, GameObject> characters = new Dictionary<ChapterOfGame, GameObject>();
    private Dictionary<ChapterOfGame, string> words = new Dictionary<ChapterOfGame, string>();
    private Dictionary<ChapterOfGame, TextAsset> beginTextAssets = new Dictionary<ChapterOfGame, TextAsset>();
    private Dictionary<ChapterOfGame, TextAsset> endTextAssets = new Dictionary<ChapterOfGame, TextAsset>();

    public void Awake()
    {
        characters.Add(ChapterOfGame.ChapterDancer,dancer);
        characters.Add(ChapterOfGame.ChapterWaiter,waiter);
        characters.Add(ChapterOfGame.ChapterProgrammer,programmer);
        
        words.Add(ChapterOfGame.ChapterDancer, "舞女");
        words.Add(ChapterOfGame.ChapterWaiter, "服务员");
        words.Add(ChapterOfGame.ChapterProgrammer, "黑客");
        
        beginTextAssets.Add(ChapterOfGame.ChapterDancer, dialogWithDancer0);
        endTextAssets.Add(ChapterOfGame.ChapterDancer, dialogWithDancer1);
        beginTextAssets.Add(ChapterOfGame.ChapterWaiter, dialogWithWaiter0);
        endTextAssets.Add(ChapterOfGame.ChapterWaiter, dialogWithWaiter1);
        beginTextAssets.Add(ChapterOfGame.ChapterProgrammer, dialogWithProgrammer0);
        endTextAssets.Add(ChapterOfGame.ChapterProgrammer, dialogWithProgrammer1);
    }
    
    private void Update()
    {
        if (GameFlowManager.Instance.currentChapter == ChapterOfGame.NoOne)
        {
            dancer.SetActive(false);
            waiter.SetActive(false);
            programmer.SetActive(false);
        }
        
        //每次从梦中回到现实，isStartEmdDialog会重置，故不用调整
        if (GameFlowManager.Instance.currentIsOver && isStartEndDialog == false) 
        {
            characters[GameFlowManager.Instance.currentChapter].SetActive(true);
            
            if(beginTextAssets[GameFlowManager.Instance.currentChapter] != null)
                DialogManager.Instance.StartDialog( endTextAssets[GameFlowManager.Instance.currentChapter]);
            else
            {
                DialogManager.Instance.ShowMessage("暂无剧情");
            }
            isStartEndDialog = true;
        }
    }
    
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (isPatientCome)
        {
            DialogManager.Instance.ShowMessage("我最好等处理完现在的患者再接待下一位");
            return;
        }
        
        //触发事件，黑屏，然后患者出现，对话
        StartCoroutine(OnDancerComeIn());
        
        //切换至舞女章节,但是治疗没结束,天数不变
        GameFlowManager gameFlowManager = GameFlowManager.Instance;
        gameFlowManager.ChangeChapter(gameFlowManager.listOfChapter[gameFlowManager.currentDay],false, gameFlowManager.currentDay);
    }

    private IEnumerator DancerComeIn()
    {
        if (UIManager.Instance.coverFader != null)
        { 
            yield return UIManager.Instance.coverFader.FadeIn();
            
            //TODO:像下面一样添加字典：chapter->string
            yield return UIManager.Instance.coverFader.TextType(words[GameFlowManager.Instance.currentChapter]);
            
            //TODO:添加一个字典：chapter->患者物体，用来激活人物
            characters[GameFlowManager.Instance.currentChapter].SetActive(true);
            isPatientCome = true;
           
            yield return UIManager.Instance.coverFader.FadeOut();
        }
    }

    private IEnumerator OnDancerComeIn()
    {
        yield return DancerComeIn();
        
        if(beginTextAssets[GameFlowManager.Instance.currentChapter] != null)
            DialogManager.Instance.StartDialog( beginTextAssets[GameFlowManager.Instance.currentChapter]);
        else
        {
            DialogManager.Instance.ShowMessage("暂无剧情");
        }
    }
}