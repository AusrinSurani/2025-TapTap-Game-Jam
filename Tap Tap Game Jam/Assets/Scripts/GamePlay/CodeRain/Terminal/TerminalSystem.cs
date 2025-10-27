using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerminalSystem : MonoBehaviour
{

    public ScrollRect terminalSR;

    public GameObject codeContentPrefab;

    private ShowTextPiece _curShowTextPiece;

    private struct WaitTypePiece 
    {
        public string showContent;
        public bool bShowByType;
        public float charTypeCostTime;
        public float endTypeWaitTime;
        public Color pieceTextColor;

        public bool bIsMixed;
        public float waitTimeBeforeType; 
        public string typeContent;
        public Color typeTextColor;
        public bool bLongText;
    }

    public void ClearAllText()
    {
        for(int i=terminalSR.content.childCount-1;i>=0;i--)
        {
            Destroy(terminalSR.content.GetChild(i).gameObject);
        }
    }

    private List<WaitTypePiece> _waitingPiecesInfos = new List<WaitTypePiece>(); 
    public bool BHaveTextWait;
    public void AddNewCodeContent(string code, bool bTypeShow)
    {
        if (_curShowTextPiece != null)
        {
            if (_curShowTextPiece.GetIsTextShowTyping())
            {
                //当前代码段仍在输出
                WaitTypePiece nTypePiece = new WaitTypePiece();
                nTypePiece.showContent = code;
                nTypePiece.bShowByType = bTypeShow;
                nTypePiece.charTypeCostTime = 0.1f;
                nTypePiece.endTypeWaitTime = 0f;
                nTypePiece.pieceTextColor = Color.white;
                nTypePiece.bLongText = false;
                _waitingPiecesInfos.Add(nTypePiece);
                BHaveTextWait = true;
            }
            else
            {
                //取消选中状态
                _curShowTextPiece.ResetThisInputStatus();
                //生成新代码片段
                _curShowTextPiece = Instantiate(codeContentPrefab, terminalSR.content).GetComponent<ShowTextPiece>();

                if (bTypeShow)
                    _curShowTextPiece.SetShowTextForType(code, false);
                else
                    _curShowTextPiece.SetShowTextNoType(code, false);
                _curShowTextPiece.SetThisCurrentInput();

            }
        }
        else
        {
            //生成新代码片段
            _curShowTextPiece = Instantiate(codeContentPrefab, terminalSR.content).GetComponent<ShowTextPiece>();
            if (bTypeShow)
                _curShowTextPiece.SetShowTextForType(code, false);
            else
                _curShowTextPiece.SetShowTextNoType(code, false);
            _curShowTextPiece.SetThisCurrentInput();

        }
    }

    public void AddNewCodeContent(string code,bool bTypeShow,bool bLongText)
    {
        //bAddNewContent = true;
        if (_curShowTextPiece!=null)
        {
            if (_curShowTextPiece.GetIsTextShowTyping())
            {
                //当前代码段仍在输出
                WaitTypePiece nTypePiece = new WaitTypePiece();
                nTypePiece.showContent = code;
                nTypePiece.bShowByType = bTypeShow;
                nTypePiece.charTypeCostTime = 0.1f;
                nTypePiece.endTypeWaitTime = 0f;
                nTypePiece.pieceTextColor = Color.white;
                nTypePiece.bLongText = bLongText;
                _waitingPiecesInfos.Add(nTypePiece);
                BHaveTextWait = true;
            }
            else
            {
                //取消选中状态
                _curShowTextPiece.ResetThisInputStatus();
                //生成新代码片段
                _curShowTextPiece = Instantiate(codeContentPrefab, terminalSR.content).GetComponent<ShowTextPiece>();

                if (bTypeShow)
                    _curShowTextPiece.SetShowTextForType(code, bLongText);
                else
                    _curShowTextPiece.SetShowTextNoType(code, bLongText);
                _curShowTextPiece.SetThisCurrentInput();

            }
        }
        else
        {
            //生成新代码片段
            _curShowTextPiece = Instantiate(codeContentPrefab, terminalSR.content).GetComponent<ShowTextPiece>();
            if (bTypeShow) 
                _curShowTextPiece.SetShowTextForType(code, bLongText);
            else
                _curShowTextPiece.SetShowTextNoType(code, bLongText);
            _curShowTextPiece.SetThisCurrentInput();

        } 
    }

    public void AddNewCodeContent(string code, bool bTypeShow,Color textColor,float typeCharTime,float endTypeWaitTime,bool bLongText)
    {
        //bAddNewContent = true; 
        if (_curShowTextPiece != null)
        {
            if (_curShowTextPiece.GetIsTextShowTyping())
            {
                //当前代码段仍在输出
                WaitTypePiece nTypePiece = new WaitTypePiece();
                nTypePiece.showContent = code;
                nTypePiece.bShowByType = bTypeShow;
                nTypePiece.charTypeCostTime = typeCharTime;
                nTypePiece.endTypeWaitTime = endTypeWaitTime;
                nTypePiece.pieceTextColor = textColor;
                nTypePiece.bLongText = bLongText;
                _waitingPiecesInfos.Add(nTypePiece);
                BHaveTextWait = true;
            }
            else
            {
                //取消选中状态
                _curShowTextPiece.ResetThisInputStatus();
                //生成新代码片段
                _curShowTextPiece = Instantiate(codeContentPrefab, terminalSR.content).GetComponent<ShowTextPiece>();
                _curShowTextPiece.SetWaitTime(typeCharTime, endTypeWaitTime);
                if (bTypeShow)
                    _curShowTextPiece.SetShowTextForType(code, textColor, bLongText);
                else
                    _curShowTextPiece.SetShowTextNoType(code, textColor,bLongText);
                _curShowTextPiece.SetThisCurrentInput();

            }
        }
        else
        {
            //生成新代码片段
            _curShowTextPiece = Instantiate(codeContentPrefab, terminalSR.content).GetComponent<ShowTextPiece>();
            if (bTypeShow)
                _curShowTextPiece.SetShowTextForType(code, textColor, bLongText);
            else
                _curShowTextPiece.SetShowTextNoType(code, textColor, bLongText);
            _curShowTextPiece.SetThisCurrentInput();

        } 
    }

    public void AddNewCodeContentMixed(string noTypecode,string typeCode,Color notypeColor, Color typeColor, float typeCharTime, float endTypeWaitTime,float wTimeBeforeType)
    {
        //bAddNewContent = true; 
        if (_curShowTextPiece != null)
        {
            if (_curShowTextPiece.GetIsTextShowTyping())
            {
                //当前代码段仍在输出
                WaitTypePiece nTypePiece = new WaitTypePiece();
                nTypePiece.showContent = noTypecode;
                nTypePiece.bShowByType = false;
                nTypePiece.charTypeCostTime = typeCharTime;
                nTypePiece.endTypeWaitTime = endTypeWaitTime;
                nTypePiece.pieceTextColor = notypeColor;
                nTypePiece.bIsMixed = true;
                nTypePiece.waitTimeBeforeType = wTimeBeforeType;
                nTypePiece.typeContent = typeCode;
                nTypePiece.typeTextColor = typeColor;
                _waitingPiecesInfos.Add(nTypePiece);
                BHaveTextWait = true;
            }
            else
            {
                //取消选中状态
                _curShowTextPiece.ResetThisInputStatus();
                //生成新代码片段
                _curShowTextPiece = Instantiate(codeContentPrefab, terminalSR.content).GetComponent<ShowTextPiece>();
                _curShowTextPiece.SetWaitTime(typeCharTime, endTypeWaitTime); 
                _curShowTextPiece.SetShowTextMixedMode(noTypecode,typeCode,notypeColor, typeColor,wTimeBeforeType);
                _curShowTextPiece.SetThisCurrentInput();

            }
        }
        else
        {
            //生成新代码片段
            _curShowTextPiece = Instantiate(codeContentPrefab, terminalSR.content).GetComponent<ShowTextPiece>(); 
            _curShowTextPiece.SetShowTextMixedMode(noTypecode, typeCode,notypeColor, typeColor, wTimeBeforeType);
            _curShowTextPiece.SetThisCurrentInput();

        }
    }

    public bool GetIsNoWaitingContentShow()
    {
        return _waitingPiecesInfos.Count == 0 ? true : false;
    }

    private void Update()
    {
        if(BHaveTextWait)
        {
            if (!_curShowTextPiece.GetIsTextShowTyping() && _waitingPiecesInfos.Count > 0)
            {

                //当前代码片输出完成，开始下一个输出 
                //取消选中状态
                _curShowTextPiece.ResetThisInputStatus();
                //生成新代码片段
                _curShowTextPiece = Instantiate(codeContentPrefab, terminalSR.content).GetComponent<ShowTextPiece>();
                  
                if (_waitingPiecesInfos[0].bIsMixed)
                {
                    _curShowTextPiece.SetWaitTime(_waitingPiecesInfos[0].charTypeCostTime, _waitingPiecesInfos[0].endTypeWaitTime);
                    _curShowTextPiece.SetShowTextMixedMode(
                        _waitingPiecesInfos[0].showContent,
                        _waitingPiecesInfos[0].typeContent,
                       _waitingPiecesInfos[0].pieceTextColor,
                       _waitingPiecesInfos[0].typeTextColor,
                       _waitingPiecesInfos[0].waitTimeBeforeType);

                }
                else
                {
                    if (_waitingPiecesInfos[0].bShowByType)
                    {
                        _curShowTextPiece.SetShowTextForType(_waitingPiecesInfos[0].showContent, _waitingPiecesInfos[0].pieceTextColor, _waitingPiecesInfos[0].bLongText);
                        _curShowTextPiece.SetWaitTime(_waitingPiecesInfos[0].charTypeCostTime, _waitingPiecesInfos[0].endTypeWaitTime);
                    }
                    else
                    {
                        LayoutRebuilder.ForceRebuildLayoutImmediate(terminalSR.content);
                        _curShowTextPiece.SetShowTextNoType(_waitingPiecesInfos[0].showContent, _waitingPiecesInfos[0].pieceTextColor, _waitingPiecesInfos[0].bLongText);
                    }
                }
                _curShowTextPiece.SetThisCurrentInput();
                _waitingPiecesInfos.RemoveAt(0);

                //bAddNewContent = true;
                if (_waitingPiecesInfos.Count == 0)
                    BHaveTextWait = false;
            }
        }
         
    }
    private void LateUpdate()
    {
            terminalSR.verticalNormalizedPosition = 0f; 
       /* //固定到底部
        if (bAddNewContent)
        {
            bAddNewContent = false;
        }*/
    }

    public IEnumerator WaitUntilAllPiecesShow()
    {
        while(_waitingPiecesInfos.Count>0||_curShowTextPiece.GetIsTextShowTyping())
        {
            //Debug.Log("waiting typing");
            yield return null;
        }
    }
}
