public class ExitToStartMenu : Singleton<ExitToStartMenu>
{
    public bool canPress = true;
    
    public void TransToStartMenu()
    {
        if(!canPress)
            return;

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