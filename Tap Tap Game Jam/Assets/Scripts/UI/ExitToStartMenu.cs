public class ExitToStartMenu : Singleton<ExitToStartMenu>
{
    private void Start()
    {
        SceneLoadManager.Instance?.onSceneLoadEnd.AddListener(SetEnable);
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        SceneLoadManager.Instance?.onSceneLoadEnd.RemoveListener(SetEnable);

    }

    public bool canPress = true;
    
    public void TransToStartMenu()
    {
        if(!canPress)
            return;
        
        DialogManager.Instance.GetComponentInChildren<ClickableTextController>().ResetWordsDict();

        if (GameFlowManager.Instance.currentIsOver)
        {
            GameFlowManager.Instance.ChangeChapter(GameFlowManager.Instance.currentChapter, true, GameFlowManager.Instance.currentDay);
            GameFlowManager.Instance.SaveChapterData();
        }
        else
        {
            GameFlowManager.Instance.ChangeChapter(ChapterOfGame.NoOne, false, GameFlowManager.Instance.currentDay);
            GameFlowManager.Instance.SaveChapterData();
        }
        //BGM关闭
        AudioManager.Instance?.ClearAllBGMAudioPiece();



        SceneLoadManager.Instance.ResetSceneLoadStatus();
        SceneLoadManager.Instance.TryLoadToTargetSceneAsync(
            SceneLoadManager.SceneDisplayID.StartMenu, null, false);
    }

    public void SetDisable()
    {
        canPress = false;
    }

    public void SetEnable()
    {
        canPress = true;
    }
}