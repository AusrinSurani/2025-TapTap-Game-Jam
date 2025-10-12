using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("移动参数")]
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    private enum State { Idle, Move }
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
            }
        }
        else
        {
            if (currentState != State.Idle)
            {
                SwitchState(State.Idle);
            }
        }
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
            DialogManager.Instance.ShowMessage("Maybe I'd better not delve too far into other's dream");
        }
    }
}