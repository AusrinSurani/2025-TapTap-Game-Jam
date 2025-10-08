using UnityEngine;

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
            audioManager.AudioOncePlay(AudioManager.Instance.mainMenuBtnAudioPiece);
		}else
		{
			disableOnce = false;
		}
	}
}	
