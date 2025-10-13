using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class DanceGamePlay : MonoBehaviour
{
    public DancerController DancerCtr;

    public enum DanceOrder
    {
        Up=3,
        Down=6,
        Left=4,
        Right=8
    }
     
    //舞蹈指令
    public List<DanceOrder> GameOrders = new List<DanceOrder>();
    public List<float> OrdersAppearTime = new List<float>();
    public int CurRound;

    public List<DanceOrder> GameRound_0_firstHalf = new List<DanceOrder>();  
    public List<float> Round0_OrdersAppearTime = new List<float>();
    public List<DanceOrder> GameRound_1_secondHalf = new List<DanceOrder>();
    public List<float> Round1_OrdersAppearTime = new List<float>();
    public List<DanceOrder> GameRound_2 = new List<DanceOrder>();
    public List<float> Round2_OrdersAppearTime = new List<float>();
    public List<DanceOrder> GameRound_3 = new List<DanceOrder>();
    public List<float> Round3_OrdersAppearTime = new List<float>();
    public List<DanceOrder> GameRound_4 = new List<DanceOrder>();
    public List<float> Round4_OrdersAppearTime = new List<float>();
    public List<DanceOrder> GameRound_5 = new List<DanceOrder>();
    public List<float> Round5_OrdersAppearTime = new List<float>();

    [SerializeField]
    private int _curOrderIndex;
    //玩家当前输入结算的DanceOrder,避免一次交互结算两个DanceOrder
    [SerializeField]
    private int _curInputIndex;
    //记录已生成的DanceOrder
    public List<DanceOrderDisplay> AlreadyAppearedOrderDisplays = new List<DanceOrderDisplay>();
    private bool _bGameEnd;
    public void StartDanceGamePlay()
    {
        _bGameEnd = false;
        BCanGetInput = true;
        BAllowInverseInput = true;//初始允许反向
        CurRound = 0;
        //进入轮次流程
        StartGameAtRound(CurRound);
    }

    public void StartGameAtRound(int targetRound)
    {
        //重置指令表的索引
        _curOrderIndex = 0;
        _curInputIndex = 0;
        FailCount = 0;
        _danceGameTimer = 0;
        bCurRoundHaveSpecialInput = false;
        Debug.Log("Round Start");

        if (targetRound > 5)
            return;

        //轮次初始设置
        if (CurRound == 0)
        {
            BMustInverseInput = true;
            BAllowInverseInput = true;
        }
        else if(CurRound==1)
        { 
            BMustInverseInput = false;
            BAllowInverseInput = false;
        }
        else if(CurRound==2||CurRound==3)
        { 
            BMustInverseInput = false;
            BAllowInverseInput = true;
        }
        else if(CurRound == 4 || CurRound == 5||CurRound==6)
        { 
            BMustInverseInput = false;
            BAllowInverseInput = false;
        }
        else
        { 
            BMustInverseInput = false;
            BAllowInverseInput = false;
        }


            GameOrders.Clear();
        OrdersAppearTime.Clear();
        AlreadyAppearedOrderDisplays.Clear();
        if (targetRound==0)
        {
            GameOrders.AddRange(GameRound_0_firstHalf);
            OrdersAppearTime.AddRange(Round0_OrdersAppearTime);
        }
        else if(targetRound==1)
        { 
            GameOrders.AddRange(GameRound_1_secondHalf);
            OrdersAppearTime.AddRange(Round1_OrdersAppearTime);
        }
        else if (targetRound == 2)
        {
            GameOrders.AddRange(GameRound_2);
            OrdersAppearTime.AddRange(Round2_OrdersAppearTime);
        }
        else if (targetRound == 3)
        {
            GameOrders.AddRange(GameRound_3);
            OrdersAppearTime.AddRange(Round3_OrdersAppearTime);
        }
        else if (targetRound == 4)
        {
            GameOrders.AddRange(GameRound_4);
            OrdersAppearTime.AddRange(Round4_OrdersAppearTime);
        }
        else if (targetRound == 5)
        {
            GameOrders.AddRange(GameRound_5);
            OrdersAppearTime.AddRange(Round5_OrdersAppearTime);
        }
        if (_danceGamePlayIE != null)
            StopCoroutine(_danceGamePlayIE);
        _danceGamePlayIE = DanceGamePlayProgress();
        StartCoroutine(_danceGamePlayIE);
    }
    private IEnumerator _danceGamePlayIE;
    [SerializeField]
    private float _danceGameTimer; 
    private IEnumerator DanceGamePlayProgress()
    {
        while (!_bGameEnd)
        {
            HandlePlayerInput();
            _danceGameTimer += Time.deltaTime;

            /*//最后一个指令结算，没有错两个重新开始则视为本轮次结束
            if ( _curOrderIndex >= OrdersAppearTime.Count
                &&_curInputIndex == _curOrderIndex 
                &&FailCount<=2)
            {
                //本轮结束
                //完成当前轮次
                OnGameRoundEnd?.Invoke(CurRound);
                CurRound++;
                if (CurRound > 5)
                {
                    //视为通关
                    _bGameEnd = true;
                    //关闭GamePlay
                    this.gameObject.SetActive(false);
                }
                else
                {
                    //开启下一轮
                    StartGameAtRound(CurRound);
                } 
            } */

            if (OrdersAppearTime.Count==0||_danceGameTimer > OrdersAppearTime[OrdersAppearTime.Count-1]+5f/*远超出指令自行结束时间+滞留时间*/)
            {
                if (FailCount <= 2)
                {
                    OnGameRoundSuccessEnd?.Invoke(CurRound);
                    CurRound++;

                    //处理不同轮次结束后的反应
                    if (CurRound == 1 && bCurRoundHaveSpecialInput)
                    {
                        //教程关下半，也就是第2(CurRound==1)轮
                        DancerCtr.SetDancerAnimatorStauts(DancerController.DancerStatus.Special_1);
                    }
                    else if (CurRound == 2 && bCurRoundHaveSpecialInput)
                    {
                        //第一关
                        DancerCtr.SetDancerAnimatorStauts(DancerController.DancerStatus.Special_1);
                    }
                    else if (CurRound == 3 && bCurRoundHaveSpecialInput)
                    {
                        //第二关
                        DancerCtr.SetDancerAnimatorStauts(DancerController.DancerStatus.Special_1);
                        //自由意志，失败演出  
                    }
                    else if (CurRound > 5)
                    {
                        //视为通关
                        _bGameEnd = true;
                        //关闭GamePlay
                        this.gameObject.SetActive(false);
                    }

                    //开启下一轮
                    //TODO:WaitFor NextRoundStart
                    //StartGameAtRound(CurRound);

                }
                else
                {
                    //游戏失败
                }
            }
            //待出现指令没有溢界
            if (_curOrderIndex<OrdersAppearTime.Count&&_danceGameTimer > OrdersAppearTime[_curOrderIndex])
            {
                //索引小于已生成Order说明还未生成
                if(_curOrderIndex<=AlreadyAppearedOrderDisplays.Count)
                    ShowOrder(_curOrderIndex);
            } 

            yield return null;
              
        }
    }

    private bool bCanGetInput;

    public bool BCanGetInput { get => bCanGetInput; set => bCanGetInput = value; }

    public KeyCode leftOrderKey;
    public KeyCode rightOrderKey;
    public KeyCode upOrderKey;
    public KeyCode downOrderKey;
    //输入反转
    public bool bInputReverse;

    public List<Transform> orderShowTransform=new List<Transform>();


    public GameObject DanceOrderPrefab;
    public List<Transform> OrdersTransforms=new List<Transform>();//上下左右

    public UnityEvent<int> OnGameRoundSuccessEnd;
    private DanceOrderDisplay _tempDanceOrder; 
    public void ShowOrder(int targetIndex)
    {
        if(_curOrderIndex < GameOrders.Count)
        { 
            //Debug.Log("Instiante");
            if (GameOrders[_curOrderIndex] ==DanceOrder.Up)
            {
                _tempDanceOrder = Instantiate(DanceOrderPrefab, OrdersTransforms[0]).GetComponent<DanceOrderDisplay>();

            }
            else if (GameOrders[_curOrderIndex] == DanceOrder.Down)
            {
                _tempDanceOrder = Instantiate(DanceOrderPrefab, OrdersTransforms[1]).GetComponent<DanceOrderDisplay>();

            }
            else if (GameOrders[_curOrderIndex] == DanceOrder.Left)
            {
                _tempDanceOrder = Instantiate(DanceOrderPrefab, OrdersTransforms[2]).GetComponent<DanceOrderDisplay>();

            }
            else if (GameOrders[_curOrderIndex] == DanceOrder.Right)
            {
                _tempDanceOrder = Instantiate(DanceOrderPrefab, OrdersTransforms[3]).GetComponent<DanceOrderDisplay>();

            }
            AlreadyAppearedOrderDisplays.Add(_tempDanceOrder);
            _tempDanceOrder.SetOrderDisplay(GameOrders[_curOrderIndex]);
            _tempDanceOrder.ParentGamePlay = this;
            _curOrderIndex++;
        }
        else
        { 
        }  
    }

    public int FailCount=0;
    public void FailCurOrderInput()
    {
        //GetDanceOrderInput(0, DanceOrder.Left);
        _curInputIndex++;
        FailCount++;
        DancerCtr.SetDancerAnimatorStauts(DancerController.DancerStatus.Wrong);
        DancerCtr.SetDanceMaterialWrongOnce();
        //音效
        if(FailCount>2)
        {

            Debug.Log("Fail");

            //TODO:弹UI,重新挑战，点击后再重开本轮
            //重新开始当前轮次  
            //StartGameAtRound(CurRound);
        }
    }
    public bool bCurRoundHaveSpecialInput;
    public void GetDanceOrderInput(int x,DanceOrder dOrder)
    {
        _curInputIndex++;
        if(x==0)
        { 
            FailCount++;
            //失败音效
            DancerCtr.SetDancerAnimatorStauts(DancerController.DancerStatus.Wrong);
            DancerCtr.SetDanceMaterialWrongOnce();
        }
        //正向
        else if(x==1)
        {
            //成功音效
            switch (dOrder)
            {
                case DanceOrder.Up: 
                    DancerCtr.SetDancerAnimatorStauts(DancerController.DancerStatus.Up);
                    break;
                case DanceOrder.Down:
                    DancerCtr.SetDancerAnimatorStauts(DancerController.DancerStatus.Down);
                    break;
                case DanceOrder.Left:
                    DancerCtr.SetDancerAnimatorStauts(DancerController.DancerStatus.Left);
                    break;
                case DanceOrder.Right:
                    DancerCtr.SetDancerAnimatorStauts(DancerController.DancerStatus.Right);
                    break;
                default: 
                    DancerCtr.SetDancerAnimatorStauts(DancerController.DancerStatus.Idle);
                    break;
            }

        }
        //反向
        else if(x==2)
        {
            bCurRoundHaveSpecialInput = true;
            //成功音效2
            switch (dOrder)
            {
                case DanceOrder.Up:
                    DancerCtr.SetDancerAnimatorStauts(DancerController.DancerStatus.Down);
                    break;
                case DanceOrder.Down:
                    DancerCtr.SetDancerAnimatorStauts(DancerController.DancerStatus.Up);
                    break;
                case DanceOrder.Left:
                    DancerCtr.SetDancerAnimatorStauts(DancerController.DancerStatus.Right);
                    break;
                case DanceOrder.Right:
                    DancerCtr.SetDancerAnimatorStauts(DancerController.DancerStatus.Left);
                    break;
                default:
                    DancerCtr.SetDancerAnimatorStauts(DancerController.DancerStatus.Idle);
                    break;
            }
        }
    }

    public bool BMustInverseInput;
    public bool BAllowInverseInput;

    private void HandlePlayerInput()
    {
        if (BCanGetInput)
        {
            if (_curInputIndex > AlreadyAppearedOrderDisplays.Count - 1||AlreadyAppearedOrderDisplays.Count==0)
                return;
            if (AlreadyAppearedOrderDisplays[_curInputIndex] == null)
                return;
            //识别玩家输入
            if (Input.GetKeyDown(leftOrderKey))
            {
                GetDanceOrderInput(AlreadyAppearedOrderDisplays[_curInputIndex].GetInteractOrder(DanceOrder.Left, BAllowInverseInput, BMustInverseInput), DanceOrder.Left);
            }
            else if (Input.GetKeyDown(rightOrderKey))
            {
                GetDanceOrderInput(AlreadyAppearedOrderDisplays[_curInputIndex].GetInteractOrder(DanceOrder.Right, BAllowInverseInput, BMustInverseInput), DanceOrder.Right);
            }
            else if (Input.GetKeyDown(upOrderKey))
            {
                GetDanceOrderInput(AlreadyAppearedOrderDisplays[_curInputIndex].GetInteractOrder(DanceOrder.Up, BAllowInverseInput, BMustInverseInput), DanceOrder.Up);
            }
            else if (Input.GetKeyDown(downOrderKey))
            {
                GetDanceOrderInput(AlreadyAppearedOrderDisplays[_curInputIndex].GetInteractOrder(DanceOrder.Down, BAllowInverseInput, BMustInverseInput), DanceOrder.Down);
            }
        }
    }


    //test --late to delete
    private void Update()
    {
        if (Input.GetKey(KeyCode.P))
            StartDanceGamePlay();
    }

    //testend
}
