using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeGame_SceneController : MonoBehaviour
{
    private void Start()
    {
        if(AudioManager.Instance!=null)
        {
            AudioManager.Instance.ClearTargetAudioPiece(AudioManager.Instance.computerRoomBGM);
            AudioManager.Instance.AudioLoopPlay(AudioManager.Instance.rooftopBGM,true,0.5f);
        }
    }

    public void BeginEnterGameShow()
    {
        StartCoroutine(EnterGameShow());
    }


    public SpriteRenderer backgroundSpriteRender;
    public Color targetColor;
    public CodeRain codeRain;
    public CodeGamePlay codeGamePlay;
    public TerminalSystem terminalSystem;

    [SerializeField]
    private float _terminalUpLerpTimer; 
    public RectTransform targetRectTransfomr_TerminalSystem; 
    private IEnumerator EnterGameShow()
    {
        codeRain.gameObject.SetActive(true);
        //等待codeRain预热

        yield return new WaitForSeconds(2.5f);

        while ( backgroundSpriteRender.color.a<0.99f)
        {
            _terminalUpLerpTimer += Time.deltaTime * (1f / 20f);
            //终端上移
            terminalSystem.transform.position =Vector3.Lerp(terminalSystem.transform.position,targetRectTransfomr_TerminalSystem.position, _terminalUpLerpTimer);
            //黑幕渐起 
            backgroundSpriteRender.color = Color.Lerp(backgroundSpriteRender.color, targetColor,_terminalUpLerpTimer);
            yield return null; 
        }
        
        backgroundSpriteRender.color = targetColor;
        terminalSystem.transform.position = targetRectTransfomr_TerminalSystem.position;
        //Begin GamePlay
        codeGamePlay.BeginGameplay();
    }
}
