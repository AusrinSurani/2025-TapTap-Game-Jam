using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlankForWords : MonoBehaviour
{
    public bool haveWord;
    public string lostWord;
    public Sprite completeSprite;

    private void OnEnable()
    {
    }

    public void CheckAllComplete()
    {
        GetComponentInParent<SpriteRenderer>().sprite = completeSprite;
        GetComponentInParent<BlankController>().CheckComplete();
    }
    
    public void RefreshImage()
    {
        if(haveWord)
            GetComponentInParent<SpriteRenderer>().sprite = completeSprite;
    }
}