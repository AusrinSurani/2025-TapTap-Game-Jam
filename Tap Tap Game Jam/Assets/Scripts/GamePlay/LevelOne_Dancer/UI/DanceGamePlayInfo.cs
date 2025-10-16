using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DanceGamePlayInfo : MonoBehaviour
{
    public Image playerIcon;
    public GameObject hp1;
    public GameObject hp2;
    public GameObject hp3;

    //public TextMeshProUGUI failCountText;
    //public TextMeshProUGUI specialInputText;

    public void SetInfo(int failCount,bool bSpecialInput)
    {
        if(failCount==0)
        {
            hp1.gameObject.SetActive(true);
            hp2.gameObject.SetActive(true);
            hp3.gameObject.SetActive(true);
        }
        else if (failCount == 1)
        {
            hp1.gameObject.SetActive(true);
            hp2.gameObject.SetActive(true);
            hp3.gameObject.SetActive(false);
        }
        else if (failCount == 2)
        {
            hp1.gameObject.SetActive(true);
            hp2.gameObject.SetActive(false);
            hp3.gameObject.SetActive(false);
        }
        else if (failCount == 3)
        {
            hp1.gameObject.SetActive(false);
            hp2.gameObject.SetActive(false);
            hp3.gameObject.SetActive(false);
        }


        /*failCountText.text = failCount.ToString();
        if (bSpecialInput)
            specialInputText.text = "有";
        else
            specialInputText.text = "无";*/
    }
}
