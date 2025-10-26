using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapClip : MonoBehaviour
{
    public InventoryItemData mapData;
    public SpriteRenderer flag;
    public bool haveMap;
    public bool haveFlag;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("InventoryItem"))
        {
            if (other.GetComponent<DraggableItem>().inventoryItemData == mapData)
            {
                GetComponent<SpriteRenderer>().color = Color.clear;
                haveMap  = true;
                GetComponentInParent<UI_Map>().numOfMap++;
                
                other.transform.SetParent(DestoryContainer.Instance.transform);
                other.gameObject.SetActive(false);
 
                ItemSlotController.Instance.ResetSlotsPosition(other.GetComponent<DraggableItem>());
            }
        }
        
        
        if (other.CompareTag("ItemFlag"))
        {
            /*Debug.Log(other.GetComponent<DraggableItem>().inventoryItemData.ID);
            Debug.Log(mapData.ID);
            */

            if (other.GetComponent<DraggableItem>().inventoryItemData.ID == mapData.ID)
            {
                flag.color = new Color(1,1,1,1);
                haveFlag = true;
                
                GetComponentInParent<UI_Map>().numOfFlag++;
                
                other.transform.SetParent(DestoryContainer.Instance.transform);
                other.gameObject.SetActive(false);
 
                ItemSlotController.Instance.ResetSlotsPosition(other.GetComponent<DraggableItem>());
            }
        }
    }
}