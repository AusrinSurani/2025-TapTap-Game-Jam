using Cinemachine;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [Header("事件监听")]
    public VoidEventSO beginDialogEvent;
    public VoidEventSO endDialogEvent;
    
    [Header("酒店剧情")]
    public TextAsset initialDialog;
    public bool initialDialogHaveDone = true;
    
    [Header("移动参数")]
    public float moveSpeed = 5f;

    [Header("酒店脚步声")] 
    public float timeBetweenSteps;
    private float stepTimer;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    
    private enum State { Idle, Move}
    private State currentState;

    private float horizontalInput;

    private int countOfWrongAction = 0;
    private bool haveTip = false;
    
    public CinemachineVirtualCamera virtualCamera;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }


    #region 对话与可否走动

    private void OnEnable()
    {
        beginDialogEvent.OnEventRaise += OnDialogBegin;
        endDialogEvent.OnEventRaise += OnDialogEnd;
    }

    private void OnDisable()
    {
        beginDialogEvent.OnEventRaise -= OnDialogBegin;
        endDialogEvent.OnEventRaise -= OnDialogEnd;
    }

    private void OnDialogBegin()
    {
        BNoGetInput =  true;
        SetZeroVelocity();
    }

    private void OnDialogEnd()
    {
        BNoGetInput = false;
    }

    #endregion
    

    void Start()
    {
        SwitchState(State.Idle);
    }

    public bool BInvertInput;
    public bool BRemoveWrongActionTip;
    public bool BNoGetInput;
    void Update()
    {
        if (!BNoGetInput)
        {
            if (BInvertInput)
                horizontalInput = Input.GetAxisRaw("Horizontal") * -1f;
            else
                horizontalInput = Input.GetAxisRaw("Horizontal");
        }
        else
        {
            horizontalInput = 0;
        }

        #region 酒店特殊功能

        //酒店剧情
        if (!initialDialogHaveDone)
        {
            DialogManager.Instance.StartDialog(initialDialog);
            initialDialogHaveDone = true;
        }
        
        //酒店脚步
        if (SceneLoadManager.Instance.currentScene == SceneLoadManager.SceneDisplayID.WaiterDream)
        {
            if (rb.velocity.x != 0)
            {
                stepTimer -= Time.deltaTime;
                if (stepTimer <= 0)
                {
                    // 计时结束，播放声音
                    AudioManager.Instance.AudioOncePlay(AudioManager.Instance.hotelStep);
                
                    // 重置计时器
                    stepTimer = timeBetweenSteps;
                }
            }
            else
            {
                stepTimer = 0;
                AudioManager.Instance.PauseTargetAudioPiece(AudioManager.Instance.hotelStep);
            }
        }
        

        #endregion
        
        //检测状态是否要更换
        UpdateState();

        //每个状态的Update
        HandleStateActions();
        
        //处理转向
        HandleSpriteFlipping();

        if (!BRemoveWrongActionTip&&countOfWrongAction > 5 && !haveTip)
        {
            DialogManager.Instance.ShowMessage("左右颠倒的动作……这就是妮娜说的“想出左手出了右手，想出左脚出了右脚”");
            haveTip = true;
        }
    }
    
    private void UpdateState()
    {
        if (Mathf.Abs(horizontalInput) > 0.1f)
        {
            if (currentState != State.Move)
            {
                SwitchState(State.Move);
                countOfWrongAction++;
                return;
            }
        }
        
        if(Mathf.Abs(horizontalInput) == 0)
            //没有任何输入直接转成Idle
            SwitchState(State.Idle);
    }
    
    private void SwitchState(State newState)
    {
        currentState = newState;

        anim.SetBool("Idle", currentState == State.Idle);
        anim.SetBool("Move", currentState == State.Move);
    }
    
    private void HandleStateActions()
    {
        switch (currentState)
        {
            case State.Idle:
            case State.Move:
                MovePlayer();
                break;
        }
    }
    
    private void MovePlayer()
    {
        //操作与运动相反
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y) * (-1);
    }

    private void HandleSpriteFlipping()
    {
        //乘以-1是为了匹配相反的操作
        if (horizontalInput * -1 > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (horizontalInput * -1 < 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wall") && 
            SceneLoadManager.Instance.currentScene == SceneLoadManager.SceneDisplayID.DressingRoom)
        {
            DialogManager.Instance.ShowMessage("十分昏暗的房间，我最好不要在他人的梦境中走得太深");
        }
        
        if (other.CompareTag("Edge"))
        {
            DialogManager.Instance.ShowMessage("这是梦境的边缘，还是回去吧");
        }

        if (other.CompareTag("ChangeScene"))
        {
            SceneLoadManager.Instance.ResetSceneLoadStatus();
            SceneLoadManager.Instance.TryLoadToTargetSceneAsync
                (other.GetComponent<SceneLoadTrigger>().sceneToLoadID, "", false);
        }
    }

    #region PlayerAutoMove

    public void AutoMoveToTargetVector2(Vector2 v)
    {
        if (autoMove_IE != null)
            StopCoroutine(autoMove_IE);
        autoMove_IE = AutoMove(v);
        StartCoroutine(autoMove_IE);
    }
    //记录移动方向，避免走过
    private float _directionX;
    private IEnumerator autoMove_IE;
    private IEnumerator AutoMove(Vector2 v)
    {
        _directionX = 0;
        //禁止玩家输入
        BNoGetInput = true;
        //
        while (Vector2.Distance(this.transform.position, v) > 0.1f)
        {
            if (_directionX * (this.transform.position.x - v.x) < 0)
                break;
            else
                _directionX = this.transform.position.x - v.x;
            //
            if (this.transform.position.x - v.x > 0)
                horizontalInput = 1f;
            else
                horizontalInput = -1f;
            yield return null;
        }
        horizontalInput = 0f;
        BNoGetInput = true;
        this.transform.position = v; 
        OnPlayerAutoMoveFinished?.Invoke();
    }
     
    public UnityEvent OnPlayerAutoMoveFinished;
    #endregion

    public void SetZeroVelocity()
    {
        rb.velocity = Vector2.zero;
    }
}