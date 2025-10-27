using UnityEngine;

public class News : MonoBehaviour
{
    public bool[] isRead;
    public string[] selfTalk;

    public bool GetIsRead()
    {
        foreach (var t in isRead)
        {
            if (t == false) return false;
        }
        
        return true;
    }

    public void ShowSelfTalk(int i)
    {
        DialogManager.Instance.ShowMessage(selfTalk[i]);
        GetComponentInParent<UIPhone>().numOfHaveRead++;
        isRead[i] = true;
    }
}