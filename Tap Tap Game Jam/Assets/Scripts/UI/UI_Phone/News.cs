using UnityEngine;

public class News : MonoBehaviour
{
    public bool[] isRead;
    public string[] selfTalk;

    public bool isAllWordsRead = false;

    public bool GetIsRead()
    {
        isAllWordsRead = true;
        
        foreach (var t in isRead)
        {
            if (t == false) return false;
        }
        
        return true;
    }

    public void ShowSelfTalk(int i)
    {
        DialogManager.Instance.ShowMessage(selfTalk[i]);
        isRead[i] = true;
        isAllWordsRead = GetIsRead();
    }
}