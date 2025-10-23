using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CodePiece : MonoBehaviour
{
    public ImageFadeEffect fadeEffect;
    public Rigidbody2D SelfRb;
    public Vector3 StartVelocity;
    private void Start()
    { 
    }


    public void Init()
    {
        SelfRb.velocity = StartVelocity;
        _moveTimer = 0; 
        _bWaitReset = false;
    }

    public List<string> randomCodeContent = new List<string>();



    public int displaySortOrderIndex;
    public float displaySize;

    public Text  codeText;
    public Text headText;

    public void SetCodeSizeValue(float v,int sortOrder)
    {
        displaySize = v;
        displaySortOrderIndex = sortOrder;
        this.transform.localScale = Vector3.one * displaySize;
    }
    public void SetCodeContent(string content)
    {
        codeText.text = content;
    }

    public void SetCodeColor(string vcolor)
    {
        codeText.text = vcolor;
    } 
    public void MoveToStartPos(Vector3 startPos)
    {
        this.transform.position = startPos;
    }

    protected Vector3 _curPos;
    public float DownMoveSpeed;

    protected float _moveTimer;
    public float MaxMoveTime;

    public Color codeColor;

    protected IEnumerator CodeMoveDown()
    {
        while (_moveTimer<MaxMoveTime)
        {
            _moveTimer += Time.deltaTime;
            //下移
            _curPos = this.transform.position;
            _curPos.y -= Time.deltaTime * DownMoveSpeed * 100f;
            this.transform.position = _curPos;
            yield return null;
        }
        if(parentRain!=null)
            parentRain.CurRunningCodePiece--;
        this.gameObject.SetActive(false);
    }

    protected int _randomValue;
    public void RandomSetCodeContent()
    {
        _randomValue = Random.Range(0, randomCodeContent.Count);
        codeText.text = randomCodeContent[_randomValue];
        _randomValue = Random.Range(0, 2);
        headText.text= _randomValue.ToString();
    }

    public float CodeChangeInterval;
    protected float _codeChangeTimer;

    protected bool _bWaitReset;
    public bool BNotChangeCodeContent;
    protected void Update()
    {
        
        _codeChangeTimer += Time.deltaTime;
        if(_codeChangeTimer>CodeChangeInterval&&!BNotChangeCodeContent)
        {
            _codeChangeTimer = 0;
            RandomSetCodeContent();
        }

        _moveTimer += Time.deltaTime;
        if (_moveTimer > MaxMoveTime)
        {
            if (!_bWaitReset)
            {
                _bWaitReset = true;
                if(parentRain!=null)
                    parentRain.CurRunningCodePiece--;
                this.gameObject.SetActive(false);

            }
        }
    }

    public CodeRain parentRain;


}
