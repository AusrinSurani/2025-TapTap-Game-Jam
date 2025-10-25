using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlotController : Singleton<ItemSlotController>
{
    public GameObject container;
    public GameObject inventoryItemPrefab;
    public List<DraggableItem> slots;

    protected override void Awake()
    {
        base.Awake();
        slots.Clear();
        slots.AddRange(GetComponentsInChildren<DraggableItem>()); 
    }

    public void ResetSlotsPosition(DraggableItem itemToDelete)
    {
        slots.Clear();
        slots.AddRange(GetComponentsInChildren<DraggableItem>());
        slots.Remove(itemToDelete);

        for (int i = 0; i < slots.Count; i++)
        {
            float yPosition = -1.5f * i + 4 + container.transform.position.y;
            Vector3 targetPosition = new Vector3(container.transform.position.x, yPosition, 9.938788f);
            
            GameObject newItem = Instantiate(inventoryItemPrefab, targetPosition, Quaternion.identity );
            
            if(slots[i].inventoryItemData != null)
                newItem.GetComponent<DraggableItem>().SetUp(slots[i].inventoryItemData);
            
            if(slots[i].inventoryWordsData != null)
                newItem.GetComponent<DraggableItem>().SetUpWords(slots[i].inventoryWordsData);
            
            newItem.transform.SetParent(container.transform);
            
            Destroy(slots[i].gameObject);
        }
    }
}
