using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DressingRoom_EnterSceneTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance?.AudioLoopPlay(AudioManager.Instance.dressingRoomBGM,true,0.3f);
    }

    
}
