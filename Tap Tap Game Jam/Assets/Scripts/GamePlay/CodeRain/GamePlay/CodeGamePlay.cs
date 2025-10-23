using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class CodeGamePlay : MonoBehaviour
{
    public CodeRain codeRain;

    public bool BInteracting;
     
    public BarDisplay codeRepairBar;

    [SerializeField]
    public float AddRepairValueOnePiece;
    private float _curRepairValue;
    public float MaxRepairValue;
    public float TotalRepairValue;

    private int _curSpecialPieceGetCount;
    public int MaxSpecialPieceGetCount;

    private void Start()
    {
        TotalRepairValue = 100f;
        _curRepairValue = 0f;
        StartCoroutine(GamePlayProgress());
    }

    private float _levelTimer;
    public float CurLevelGameTotalTime;
    private IEnumerator GamePlayProgress()
    {
        //LevelOne


        PauseGamePlay();
        yield return PauseGameWaitProgress();
        //出指引，指引结束后开始出现红色代码段
        SetLevelInit(0);
        while (CurGameLevel==0)
        {
            _levelTimer += Time.deltaTime;
            codeRain.TryInstantiateInteractableCodePiece(InteractableCodePiece.InteractableCodePieceType.Error_Red);
            //按需生成 红色代码片
            yield return null;
            //退出条件
            if (_levelTimer > CurLevelGameTotalTime)
            {
                CurGameLevel = 1;
                break;
            }
        }

        //关卡一演出
        //等待下一步
        PauseGamePlay();
        yield return PauseGameWaitProgress();
        //开始关卡二
        SetLevelInit(1);
        while(CurGameLevel==1)
        {

            yield return null;
            //退出条件
            if(_curSpecialPieceGetCount==MaxSpecialPieceGetCount)
            {
                
                CurGameLevel = 2;
                break;
            }
        }
        //关卡二演出
        //等待下一步
        PauseGamePlay();
        yield return PauseGameWaitProgress();

        //开始关卡三
        SetLevelInit(2);

        while (CurGameLevel == 2)
        {

            yield return null;
            //退出条件
            if (_curSpecialPieceGetCount == MaxSpecialPieceGetCount)
            { 
                CurGameLevel = 3;
                break;
            }
        }
        //关卡三演出
        //等待下一步
        PauseGamePlay();
        yield return PauseGameWaitProgress();

        //开始关卡4
        SetLevelInit(3);
        while (CurGameLevel == 3)
        {

            yield return null;
            //退出条件
            if (_curSpecialPieceGetCount == MaxSpecialPieceGetCount)
            {
                CurGameLevel = 4;
                break;
            }
        }


        //等待下一步
        PauseGamePlay();
        yield return PauseGameWaitProgress();

        //开始关卡5
        SetLevelInit(4);
        while (CurGameLevel == 4)
        {

            yield return null;
            //退出条件
            if (_curSpecialPieceGetCount == MaxSpecialPieceGetCount)
            {
                CurGameLevel = 5;
                break;
            }

        }

        //结束游戏
        yield return null;
    }

    private IEnumerator PauseGameWaitProgress()
    {
        while(_bPause)
        {
            yield return null;
        }
    }

    private bool _bPause;
    public void PauseGamePlay()
    {
        _bPause = true;
    }

    public void ResumeGameplay()
    {
        _bPause = false;
    }

    public int CurGameLevel;
    public void SetLevelInit(int level)
    {
        if(level==0)
        {
            codeRain.CurInteractableCount_BlueSpecial = 0;
            codeRain.CurInteractableCount_RedError = 0;
            codeRain.MaxInteractableCount_BlueSpecial = 0;
            codeRain.MaxInteractableCount_RedError = 10;
            AddRepairValueOnePiece = 3f;
            MaxRepairValue = 20f;
            CurLevelGameTotalTime = 10f;
        }
        else if(level ==1)
        {

        }
        else if (level == 2)
        {

        }
        else if (level == 3)
        {

        }
        else if (level == 4)
        {

        }
    }



    public void GetCodePiece(InteractableCodePiece.InteractableCodePieceType pieceType)
    {
        if(pieceType==InteractableCodePiece.InteractableCodePieceType.Error_Red)
        {
            //进度条
            _curRepairValue+= AddRepairValueOnePiece;
            if (_curRepairValue > MaxRepairValue)
                _curRepairValue = MaxRepairValue;
            codeRepairBar.SetBarValue((float)_curRepairValue / (float)TotalRepairValue);
        }
        else if(pieceType==InteractableCodePiece.InteractableCodePieceType.Special_Blue)
        {
            _curSpecialPieceGetCount++;
            if(_curRepairValue>=MaxSpecialPieceGetCount)
            {
                //当前轮次结束，触发相关演出
            }
        }
    }
}
