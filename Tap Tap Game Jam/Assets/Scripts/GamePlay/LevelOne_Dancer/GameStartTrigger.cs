using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartTrigger : MonoBehaviour
{
    public void ShowBeginPanel()
    {
        UIManager.Instance.ShowPanelByPath<BeginDanceGamePlayPanel>("UI/DanceGamePlay/BeginDanceGamePlayPanel"); 
    }
}
