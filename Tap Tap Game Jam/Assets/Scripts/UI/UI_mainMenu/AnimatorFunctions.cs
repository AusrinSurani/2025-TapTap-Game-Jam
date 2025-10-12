<<<<<<< HEAD
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
=======
﻿using UnityEngine;
>>>>>>> 9d7854228de69e87ce428786b426272b895e5e43

/// <summary>
/// 按钮动画的触发函数
/// </summary>
public class AnimatorFunctions : MonoBehaviour
{
    public bool disableOnce;

    private AudioManager audioManager;

    private void Start()
    {
        audioManager = AudioManager.Instance;
    }

    void PlaySound(AudioClip whichSound)
	{
		if(!disableOnce)
		{
            //menuButtonController.audioSource.PlayOneShot (whichSound);
            AudioManager.Instance.AudioOncePlay(whichSound);
		}else
		{
			disableOnce = false;
		}
        
	} 
}	
