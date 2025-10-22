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

    public DanceGamePlay gamePlayPart;

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

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            BackToConsoleRoom();
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
    public float curtainScaleChangeSpeed=1f;


    public void RaiseCurtain()
    {
        gamePlayPart.JoystickCtr.SetAnimatorStatus(JoystickController.JoystickStatus.Middle);
        StartCoroutine(CurtainMove());
    }

    public void DownCurtain()
    {
        gamePlayPart.JoystickCtr.SetAnimatorStatus(JoystickController.JoystickStatus.MiddleOff);
        StartCoroutine(CurtainMoveBack());

    }
    private Vector3 _tempCurtainSizeScale;
    public float UpOffsetSpeed;

    [SerializeField]
    private float _curtainMoveTimer;
    public float CurtainMoveTotalTime=3f;
    private IEnumerator CurtainMove()
    {
        while (_curtainMoveTimer<CurtainMoveTotalTime)
        {
            _curtainMoveTimer += Time.deltaTime;
            _tempCurtainSizeScale = leftCurtain.transform.localScale;
            _tempCurtainSizeScale.x -= Time.deltaTime * curtainScaleChangeSpeed;
            leftCurtain.transform.localScale = _tempCurtainSizeScale;
            _tempCurtainSizeScale = rightCurtain.transform.localScale;
            _tempCurtainSizeScale.x -= Time.deltaTime * curtainScaleChangeSpeed;
            rightCurtain.transform.localScale = _tempCurtainSizeScale;
            leftCurtain.transform.Translate(Vector2.left * Time.deltaTime * curtainMoveSpeed+Vector2.up*Time.deltaTime* UpOffsetSpeed);
            rightCurtain.transform.Translate(Vector2.right * Time.deltaTime * curtainMoveSpeed + Vector2.up * Time.deltaTime* UpOffsetSpeed);
            yield return null;
        }
        leftCurtain.transform.parent.gameObject.SetActive(false);

        //允许交互
        ShowInteractItem();
    }

    private IEnumerator CurtainMoveBack()
    {
        HideInteractItem();
        leftCurtain.transform.parent.gameObject.SetActive(true);
        while (_curtainMoveTimer>0f)
        {
            _curtainMoveTimer -= Time.deltaTime;
            _tempCurtainSizeScale = leftCurtain.transform.localScale;
            _tempCurtainSizeScale.x += Time.deltaTime * curtainScaleChangeSpeed;
            leftCurtain.transform.localScale = _tempCurtainSizeScale;
            _tempCurtainSizeScale = rightCurtain.transform.localScale;
            _tempCurtainSizeScale.x += Time.deltaTime * curtainScaleChangeSpeed;
            rightCurtain.transform.localScale = _tempCurtainSizeScale;
            leftCurtain.transform.Translate(Vector2.right * Time.deltaTime * curtainMoveSpeed - Vector2.up * Time.deltaTime * UpOffsetSpeed);
            rightCurtain.transform.Translate(Vector2.left * Time.deltaTime * curtainMoveSpeed - Vector2.up * Time.deltaTime * UpOffsetSpeed);
            yield return null;
        }
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
        gamePlayPart.JoystickCtr.SetAnimatorStatus(JoystickController.JoystickStatus.MiddleOff);
        gamePlayPart.JoystickCtr.transform.GetChild(0).gameObject.SetActive(false);
    }
    public void TurnOffSceneLight()
    { 
        sceneLight_Off.SetActive(true);
        sceneLight_On.SetActive(false); 
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.AudioOncePlay(AudioManager.Instance.sceneLightSwitchAudioPiece);
        }
        gamePlayPart.JoystickCtr.SetAnimatorStatus(JoystickController.JoystickStatus.Middle);
        //灯光
        gamePlayPart.JoystickCtr.transform.GetChild(0).gameObject.SetActive(true);
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

    public GameObject background_noLight;
    public GameObject backgound_haveLight;
    public GameObject backgound_border;
    public void ReplaceSceneSpirte()
    {
        background_noLight.SetActive(false);
        backgound_haveLight.SetActive(true);
        backgound_border.SetActive(false); 
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.AudioOncePlay(AudioManager.Instance.spotLightSwitchAudioPiece);
        }
    }

    public void RebackSceneSpirte()
    {
        background_noLight.SetActive(true);
        backgound_haveLight.SetActive(false);
        backgound_border.SetActive(true);
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.AudioOncePlay(AudioManager.Instance.spotLightSwitchAudioPiece);
            //关闭背景音乐
            AudioManager.Instance.ClearTargetAudioPiece(AudioManager.Instance.dancerBGMAudioPiece);
        }
    }

    public GameObject InteractPart; 

    public void HideInteractItem()
    {
        InteractPart.gameObject.SetActive(false); 
    }
    public void ShowInteractItem()
    {
        InteractPart.gameObject.SetActive(true); 

    }

    public PlayerController PlayerCtr;
    public Transform PlayerTransformRef;
    public void MovePlayer()
    {
        PlayerCtr.AutoMoveToTargetVector2(PlayerTransformRef.position);
        //添加监听
        PlayerCtr.OnPlayerAutoMoveFinished.AddListener(ResumeDirector);
    } 

    private void ResumeDirector()
    {
        DirectResume();
        //移除监听
        PlayerCtr.OnPlayerAutoMoveFinished.RemoveListener(ResumeDirector);
    }
    public void BackToConsoleRoom()
    {           
        //标记完成诊断
        GameFlowManager.Instance.ChangeChapter(GameFlowManager.Instance.currentChapter, true, GameFlowManager.Instance.currentDay);
        SceneLoadManager.Instance.TryLoadToTargetSceneAsync(SceneLoadManager.SceneDisplayID.ConsultationRoom, null, false);
    }
}
