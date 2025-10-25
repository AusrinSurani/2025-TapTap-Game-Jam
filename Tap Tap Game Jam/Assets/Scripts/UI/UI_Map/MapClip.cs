using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapClip : MonoBehaviour
{
    public InventoryItemData mapData;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("InventoryItem"))
        {
            if (other.GetComponent<DraggableItem>().inventoryItemData == mapData)
            {
                gameObject.SetActive(false);
                Destroy(other.gameObject);
            }
        }
    }
}