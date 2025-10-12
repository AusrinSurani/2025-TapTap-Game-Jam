using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SaveButton : BounceButton
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        AudioManager.Instance.AudioOncePlay(AudioManager.Instance.mainMenuBtnAudioPiece);
        GetComponentInParent<AudioSettingsController>().SaveAudioSettingData();
    }
}