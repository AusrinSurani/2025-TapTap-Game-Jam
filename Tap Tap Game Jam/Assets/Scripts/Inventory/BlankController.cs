using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlankController : MonoBehaviour
{
    public BlankForWords[] blankForWords;
    private bool haveAllComplete = false;
    public Sprite allCompleteSprite;
    
    public InventoryItemData inventoryItemData;
    public GameObject inventoryItemPrefab;
    public GameObject container;
    
    [Header("地图碎片")]
    public bool haveGetMap = false;
    public GameObject mapSprite;

    private void Awake()
    {
        blankForWords = GetComponentsInChildren<BlankForWords>();
    }

    private void OnEnable()
    {
        foreach (BlankForWords word in blankForWords)
        {
            word.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    private void Start()
    {
        
    }

    private void Update()
    {
    }
        
    public void CheckComplete()
    {
        //有一个不全就为false
        haveAllComplete = true;
        foreach (BlankForWords blankForWord in blankForWords)
        {
            if (!blankForWord.haveWord)
                haveAllComplete = false;
        }

        if (haveAllComplete && !haveGetMap)
        {
            GetComponent<SpriteRenderer>().sprite = allCompleteSprite;

            foreach (BlankForWords blankForWord in blankForWords)
            {
                blankForWord.gameObject.SetActive(false);
            }
            
            haveGetMap  = true;
            //TODO:获取地图碎片
            GenerateItem(inventoryItemData);
            GetComponent<SpriteRenderer>().color = new Color(1,1,1,0);
            StartCoroutine(RaiseCoroutine(1f));
        }
    }
    
    public void GenerateItem(InventoryItemData itemData)
    {
        int numOfItems = container.transform.childCount;
        
        float yPosition = -1.5f * numOfItems + 4 + container.transform.position.y;
        Vector3 targetPosition = new Vector3(container.transform.position.x, yPosition, 9.938788f);

        GameObject newItem = Instantiate(inventoryItemPrefab, targetPosition, Quaternion.identity );
        
        newItem.GetComponent<DraggableItem>().SetUp(itemData);
        newItem.transform.SetParent(container.transform);
    }
    
    private IEnumerator RaiseCoroutine(float duration)
    {
        mapSprite.SetActive(true);
        mapSprite.GetComponent<SpriteRenderer>().color = Color.clear;
        float timer = 0;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            mapSprite.GetComponent<SpriteRenderer>().color =new Color (1,1,1,Mathf.Lerp(0, 1, timer / duration)) ;
            yield return null;
        }
    }
}