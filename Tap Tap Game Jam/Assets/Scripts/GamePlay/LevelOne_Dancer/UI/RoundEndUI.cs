using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundEndUI : MonoBehaviour
{ 
    public Button nextRoundBtn;
    public Button restartRoundBtn;
    public Button endGameBtn;
     
    /// <summary>
    /// 传入0为nextRoundBtn,1为restartRoundBtn,2为endGameBtn
    /// </summary>
    /// <param name="btnIndex"></param>
    public void SetButtonShow(int btnIndex)
    {
        nextRoundBtn.gameObject.SetActive(false);
        restartRoundBtn.gameObject.SetActive(false);
        endGameBtn.gameObject.SetActive(false);

        switch (btnIndex)
        {
            case 0:
                nextRoundBtn.gameObject.SetActive(true);
                break;
            case 1:
                restartRoundBtn.gameObject.SetActive(true);
                break;
            case 2:
                endGameBtn.gameObject.SetActive(true);
                break;
            default:
                break;
        }
        
    }
}
