using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item")]

public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public Sprite outline;
    public Sprite closeUp;
    public string description;
    public string[] descriptions;
    public string[] nextDescriptions;
}