using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DancerDreamTest : MonoBehaviour
{
    public DanceGamePlay gamePlay;
    public TextMeshProUGUI roundShowText;

    private void Update()
    {
        roundShowText.text = gamePlay.CurRound.ToString();
    }
}
