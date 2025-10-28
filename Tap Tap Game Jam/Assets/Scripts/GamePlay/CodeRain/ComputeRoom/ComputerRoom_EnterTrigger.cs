using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerRoom_EnterTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.AudioLoopPlay(AudioManager.Instance.computerRoomBGM,true,0.5f);
        }
    }
     
}
