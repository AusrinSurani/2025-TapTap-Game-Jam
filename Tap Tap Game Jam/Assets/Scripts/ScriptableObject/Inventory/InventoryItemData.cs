using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory Item Data", menuName = "Data/Inventory Item")]
public class InventoryItemData : ScriptableObject
{
    public string id;
    public Sprite  sprite;
    public Sprite closeUp;
}
