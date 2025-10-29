using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Creators : MonoBehaviour
{
    public RectTransform creatorList;
    public float duration;
    public float defaultSpeed;

    private float currentSpeed;
    private Vector2 position;
    
    public MenuButton[] buttons;
    
    private void Start()
    {
        StartCoroutine(RaiseList());
    }

    private void Update()
    {
        float yPosition = creatorList.position.y + Time.deltaTime * currentSpeed;
        creatorList.position = new Vector3(creatorList.position.x, yPosition , creatorList.position.z);
    }

    private IEnumerator RaiseList()
    {
        yield return new WaitForSeconds(1f);
        currentSpeed = defaultSpeed;
        yield return new WaitForSeconds(duration);
        currentSpeed = 0;
        yield return new WaitForSeconds(3f);
        
        foreach (var t in buttons)
        {
            t.canControl = true;
        }
        
        StartCoroutine(Vanish());
    }

    private IEnumerator Vanish()
    {
        float timer = 0f;
        while (timer < 1f)
        {
            timer += Time.deltaTime;
            GetComponent<Image>().color = new Color(1,1,1,Mathf.Lerp(1f,0f,timer / 1));

            if (timer >= 0.95f)
            {
                gameObject.SetActive(false);
            }
            
            yield return null;
        }
    }
}
