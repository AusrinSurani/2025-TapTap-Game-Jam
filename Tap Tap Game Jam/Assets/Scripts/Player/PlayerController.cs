using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("移动参数")]
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    public KeyCode torchKey = KeyCode.I;
    public KeyCode sleepKey = KeyCode.O;
    
    private enum State { Idle, Move, Touch, Sleep }
    private State currentState;

    private float horizontalInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Start()
    {
        SwitchState(State.Idle);
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        
        //检测状态是否要更换
        UpdateState();

        //每个状态的Update
        HandleStateActions();
        
        //处理转向
        HandleSpriteFlipping();
    }
    
    private void UpdateState()
    {
        if (Mathf.Abs(horizontalInput) > 0.1f)
        {
            if (currentState != State.Move)
            {
                SwitchState(State.Move);
                return;
            }
        }

        if (Input.GetKeyDown(torchKey))
        {
            SwitchState(currentState == State.Touch ? State.Idle : State.Touch);
            return;
        }
        
        if (Input.GetKeyDown(sleepKey))
        {
            SwitchState(currentState == State.Sleep ? State.Idle : State.Sleep);
            return;
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
        anim.SetBool("Touch", currentState == State.Touch);
        anim.SetBool("Sleep", currentState == State.Sleep);
    }
    
    private void HandleStateActions()
    {
        switch (currentState)
        {
            case State.Idle:
            case State.Touch:
            case State.Sleep:
                rb.velocity = new Vector2(0, rb.velocity.y);
                break;
            case State.Move:
                MovePlayer();
                break;
        }
    }
    
    private void MovePlayer()
    {
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
    }

    private void HandleSpriteFlipping()
    {
        if (horizontalInput > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (horizontalInput < 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wall"))
        {
            DialogManager.Instance.ShowMessage("十分昏暗的房间，我最好不要在他人的梦境中走得太深");
        }
    }
}