using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CollectableItem : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    public SpriteRenderer outline;
    public InventoryItemData itemData;
    public GameObject inventoryItemPrefab;
    public GameObject container;

    private void Start()
    {
        outline.enabled = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //TODO:消失，生成一个物体在container那里
        StartCoroutine(VanishCoroutine());
        GenerateItem(itemData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        outline.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        outline.enabled = false;
    }

    private IEnumerator VanishCoroutine()
    {
        float timer = 0;

        while (timer < 0.3f)
        {
            timer += Time.deltaTime;
            GetComponent<SpriteRenderer>().color = 
                new Color(1, 1, 1, Mathf.Lerp(1, 0, timer / 0.3f));
        }
        
        yield return new WaitForSeconds(0.3f);
        
        gameObject.SetActive(false);
    }

    private void GenerateItem(InventoryItemData itemData)
    {
        int numOfItems = container.transform.childCount;
        
        float yPosition = -1.5f * numOfItems + 4 + container.transform.position.y;

        GameObject newItem = Instantiate(inventoryItemPrefab, new Vector3(
            container.transform.position.x, yPosition, 9.938788f), Quaternion.identity );
        
        newItem.GetComponent<DraggableItem>().SetUp(itemData);
        newItem.transform.SetParent(container.transform);
    }
}
