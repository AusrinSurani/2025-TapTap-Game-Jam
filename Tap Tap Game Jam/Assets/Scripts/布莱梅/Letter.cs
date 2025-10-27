using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Letter : MonoBehaviour
{
    public GameObject berman;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("InventoryItem"))
        {
            Debug.Log("enter");
            Debug.Log(other.name);
            Debug.Log(other.GetComponent<DraggableItem>().inventoryWordsData.word);
            if (other.GetComponent<DraggableItem>().inventoryWordsData.word == "樱花")
            {
                berman.GetComponent<Bremen>().TriggerComplete();
                other.GetComponent<DraggableItem>().ResetPositions();
            }
        }
    }
}