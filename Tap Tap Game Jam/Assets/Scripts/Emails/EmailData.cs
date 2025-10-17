using UnityEngine;

[CreateAssetMenu(fileName = "New Mail Data", menuName = "Data/Mail")]
public class MailData:ScriptableObject
{
    public string id;
    public string title;
    public string sender;
    [TextArea(5,10)]
    public string content;
    public bool isRead;
}