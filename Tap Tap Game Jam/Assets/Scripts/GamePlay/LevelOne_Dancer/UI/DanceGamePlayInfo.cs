using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DanceGamePlayInfo : MonoBehaviour
{
    public Image playerIcon;
    public Image hp1;
    public Image hp2;
    public Image hp3;

    public Sprite healthFullImage;
    public Sprite healthEmptyImage; 

    //public TextMeshProUGUI failCountText;
    //public TextMeshProUGUI specialInputText;

    public void SetInfo(int failCount,bool bSpecialInput)
    {
        //TODO:动态效果
        if(failCount==0)
        {
            hp1.sprite= healthFullImage;
            hp2.sprite = healthFullImage;
            hp3.sprite = healthFullImage;
        }
        else if (failCount == 1)
        {
            hp1.sprite = healthFullImage;
            hp2.sprite = healthFullImage;
            hp3.sprite = healthEmptyImage;
        }
        else if (failCount == 2)
        {
            hp1.sprite = healthFullImage;
            hp2.sprite = healthEmptyImage;
            hp3.sprite = healthEmptyImage;
        }
        else if (failCount == 3)
        {
            hp1.sprite = healthEmptyImage;
            hp2.sprite = healthEmptyImage;
            hp3.sprite = healthEmptyImage;
        }


        /*failCountText.text = failCount.ToString();
        if (bSpecialInput)
            specialInputText.text = "有";
        else
            specialInputText.text = "无";*/
    }
}
