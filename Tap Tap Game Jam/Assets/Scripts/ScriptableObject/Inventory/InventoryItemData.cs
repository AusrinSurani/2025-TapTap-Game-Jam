using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory Item Data", menuName = "Data/Inventory Item")]
public class InventoryItemData : ScriptableObject
{
    public Sprite  sprite;
    public Sprite closeUp;
    [TextArea(3, 10)]
    public string[] descriptions;
}