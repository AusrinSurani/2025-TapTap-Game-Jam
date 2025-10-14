using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DacnerDirector : MonoBehaviour
{
    public PlayableDirector DancerDirector;

    public GameObject sceneLight_Off;
    public GameObject sceneLight_On;

    public GameObject spotLight_Center;
    public GameObject spotLight_Path;

    private void Update()
    {
        //test
        if(Input.GetKeyDown(KeyCode.B))
        {
            DancerDirector.Play();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            DirectResume();
        }
        //endtest
    }

    public bool BShowing;
    public void OnDirectorBegin()
    {
        BShowing = true;
    }
    
    public void OnDirectorEnd()
    { 
        BShowing = false;
    }

    public void DirectPause()
    {
        DancerDirector.Pause();
    }
    public void DirectResume()
    {
        DancerDirector.Resume();
    }

    public GameObject leftCurtain;
    public GameObject rightCurtain;
    public float curtainMoveSpeed;
    public void RaiseCurtain()
    {
        StartCoroutine(CurtainMove());
    } 
    private IEnumerator CurtainMove()
    {
        while (leftCurtain.transform.localPosition.x > -15f || rightCurtain.transform.localPosition.x < 15f)
        {
            leftCurtain.transform.Translate(Vector2.left * Time.deltaTime * curtainMoveSpeed);
            rightCurtain.transform.Translate(Vector2.right * Time.deltaTime * curtainMoveSpeed);
            yield return null;
        }
        leftCurtain.transform.parent.gameObject.SetActive(false);
    }
    //舞台大灯
    public void TurnOnSceneLight()
    {
        sceneLight_Off.SetActive(false);
        sceneLight_On.SetActive(true);
        if(AudioManager.Instance!=null)
        {
            AudioManager.Instance.AudioOncePlay(AudioManager.Instance.sceneLightSwitchAudioPiece);
        }
    }
    public void TurnOffSceneLight()
    { 
        sceneLight_Off.SetActive(true);
        sceneLight_On.SetActive(false); 
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.AudioOncePlay(AudioManager.Instance.sceneLightSwitchAudioPiece);
        }
    }
    //射灯
    public void TurnOnSpotLight()
    {
        spotLight_Center.SetActive(true);
        if (spotLight_Path != null)
            spotLight_Path.SetActive(true);
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.AudioOncePlay(AudioManager.Instance.spotLightSwitchAudioPiece);
        }
    }
    public void TurnOffSpotLight()
    { 
        spotLight_Center.SetActive(false);
        if(spotLight_Path!=null)
            spotLight_Path.SetActive(false);
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.AudioOncePlay(AudioManager.Instance.spotLightSwitchAudioPiece);
        }
    }

    public void PlayDancerBGM()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.AudioLoopPlay(AudioManager.Instance.dancerBGMAudioPiece);
        }
    }
}
