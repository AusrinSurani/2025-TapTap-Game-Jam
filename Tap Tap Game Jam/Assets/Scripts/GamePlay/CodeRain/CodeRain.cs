using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeRain : MonoBehaviour
{
    public CodeGamePlay ParentGamePlay;
    public List<CodePiece> codePieces = new List<CodePiece>();

    public int MaxRunningCodePiece;
    public int CurRunningCodePiece;

    public GameObject piecePrefab;
    private GameObject _tempObj;
    private CodePiece _tempCodePiece;
    private void Start()
    {
        _randomInstantiateDelay = Random.Range(0.1f, 1f);
        _randomInteractableInstantiateDelay = Random.Range(0.5f, 1f);
        _lastRandomValue = -1;
        //test
        if (codePieces.Count<100)
        {
            for(int i=0;i<100;i++)
            {
                _tempObj = Instantiate(piecePrefab, this.transform.GetChild(0));
                _tempCodePiece = _tempObj.GetComponent<CodePiece>();
                _tempCodePiece.parentRain = this;
                codePieces.Add(_tempCodePiece);
                _tempObj.gameObject.SetActive(false);
            }
        }

        //endtest

    }
    private void Update()
    {

        if(!BTimerPause)
            _delayTimer_interactable += Time.deltaTime;
        if (CurRunningCodePiece >= MaxRunningCodePiece)
            return;
        _delayTimer += Time.deltaTime;
        if (_delayTimer > _randomInstantiateDelay)
        {
            _delayTimer = 0;
            for (int i = 0; i < codePieces.Count; i++)
            {
                if (!codePieces[i].gameObject.activeSelf)
                {
                    RandomCodePieceInstantiate(codePieces[i]);
                    break; 
                }
            }
        }
    }

    public List<Transform> instantiateTransforms = new List<Transform>();

    private int _randomValue;
    private int _lastRandomValue;
    public void RandomCodePieceInstantiate(CodePiece piece)
    {
        _randomValue = Random.Range(0, instantiateTransforms.Count);
        //如果与上次位置重复则向右偏移
        if (_randomValue == _lastRandomValue)
            _randomValue = (_randomValue + 5) % instantiateTransforms.Count;
        //记录本次随机的位置索引
        _lastRandomValue = _randomValue;
        piece.gameObject.SetActive(true);
        piece.transform.position = instantiateTransforms[_randomValue].position;
        piece.transform.SetParent(this.transform);
        piece.Init();
        
        CurRunningCodePiece++;

        _randomInstantiateDelay = Random.Range(0.01f, 0.1f);
    }

    private float _delayTimer;
    private float _randomInstantiateDelay;//避免同时生成

    private float _randomInteractableInstantiateDelay;
    private float _delayTimer_interactable;
    public bool BTimerPause;

    public GameObject InteractableCodePiecePrefab;
    private GameObject _tempInteractObj;
    /// <summary>
    /// 在GamePlay中持续调用
    /// </summary>
    /// <param name="interactType"></param>
    /// <returns></returns>
    public bool TryInstantiateInteractableCodePiece(InteractableCodePiece.InteractableCodePieceType interactType)
    {
        if(_delayTimer_interactable<_randomInteractableInstantiateDelay)
        {
            return false;
        }
        //记录生成数
        if (interactType == InteractableCodePiece.InteractableCodePieceType.Error_Red)
        {
            if (CurInteractableCount_RedError > MaxInteractableCount_RedError)
                return false;
            CurInteractableCount_RedError++;
        }
        else
        {
            if (CurInteractableCount_BlueSpecial > MaxInteractableCount_BlueSpecial)
                return false;
            CurInteractableCount_BlueSpecial++;
        }
        //重置计时
        _delayTimer_interactable = 0;

        _randomValue = Random.Range(0, instantiateTransforms.Count);
        //如果与上次位置重复则向右偏移
        if (_randomValue == _lastRandomValue)
            _randomValue = (_randomValue + 5) % instantiateTransforms.Count;
        //记录本次随机的位置索引
        _lastRandomValue = _randomValue;

        //
        _tempInteractObj = Instantiate(InteractableCodePiecePrefab, this.gameObject.transform);
        _tempInteractObj.transform.position = instantiateTransforms[_randomValue].position; 
        _tempInteractObj.transform.SetParent(this.transform.GetChild(0));
        _tempInteractObj.GetComponent<InteractableCodePiece>().SetInteractType(interactType);
        _tempInteractObj.GetComponent<InteractableCodePiece>().parentGamePlay = ParentGamePlay;

        _randomInteractableInstantiateDelay = Random.Range(0.5f, 1.5f);
        return true;
    }

    public int CurInteractableCount_RedError;
    public int CurInteractableCount_BlueSpecial;
    public int MaxInteractableCount_RedError;
    public int MaxInteractableCount_BlueSpecial;
}
