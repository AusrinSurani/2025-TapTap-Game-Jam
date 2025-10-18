using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class DanceGamePlay : MonoBehaviour
{
    private void Start()
    {
        if(DialogManager.Instance!=null)
        {
            DialogManager.Instance.OnDialogueClose.AddListener(OnReceiveDiaolgueEnd);
        }
    }

    private void OnDestroy()
    {
        if (DialogManager.Instance != null)
        {
            DialogManager.Instance.OnDialogueClose.RemoveListener(OnReceiveDiaolgueEnd);
        }
    }

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

    public bool BPause;

    public PlayerController playerCtr;
    public void StartDanceGamePlay()
    {
        _bGameEnd = false;
        BCanGetInput = true;
        BAllowInverseInput = true;//初始允许反向
        CurRound = 0;

        BHaveIntro_1 = false;
        BHaveIntro_2 = false;

        failCountUI.gameObject.SetActive(true);

        //关闭玩家移动交互
        playerCtr.enabled = false;


        if (_startDanceIE != null)
            StopCoroutine(_startDanceIE);
        _startDanceIE = StartDance();
        StartCoroutine(_startDanceIE);

    }

    private IEnumerator _startDanceIE;
    private IEnumerator StartDance()
    {
        //等待演出结束
        yield return null; 
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
        BCurRoundHaveSpecialInput = false;
        Debug.Log("Round Start");

        if (targetRound > 5)
        {
            EndGame();
            return; 
        }

        //轮次初始设置
        if (CurRound == 0)
        {
            if(BHaveIntro_1==false)
            {
                IntroCtr.DoIntroMask(1);
                BHaveIntro_1 = true;
                //暂停，等待引导确认
                PauseGamePlay();
            }
            //第一关激活左上教程1
            BMustInverseInput = true;
            BAllowInverseInput = true;
        }
        else if(CurRound==1)
        {
            if (BHaveIntro_2 == false)
            {
                IntroCtr.DoIntroMask(2);
                BHaveIntro_2 = true;
                //暂停，等待引导确认
                PauseGamePlay();
            }
            //第一关激活左上教程2
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
            //重置 自动跳舞状态
            DancerCtr.ResetAnimatorAllParams();
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
        //

        
        if (_danceGamePlayIE != null)
            StopCoroutine(_danceGamePlayIE);
        _danceGamePlayIE = DanceGamePlayProgress();
        StartCoroutine(_danceGamePlayIE);
    }

    public void EnterNextRound()
    {
        ResumeGamePlay();
        CurRound++;
        StartGameAtRound(CurRound);
    }
    public void RestartCurRound()
    { 
        ResumeGamePlay();
        StartGameAtRound(CurRound);
    }
    public void EndGame()
    {
        _bGameEnd = true;

        //关闭玩家移动交互
        playerCtr.enabled = true;
    }

    public void ResumeGamePlay()
    {
        BPause = false; 
    }
    public void PauseGamePlay()
    {
        BPause = true; 
    }

    public void FailCurrentRound()
    {
        //将 未结算的DanceOrderDisplay销毁
        for (int i = 0; i < AlreadyAppearedOrderDisplays.Count; i++)
        {
            if (AlreadyAppearedOrderDisplays != null && AlreadyAppearedOrderDisplays[i] != null)
            {
                AlreadyAppearedOrderDisplays[i].OnEndGamePlay();
            }
        }
    }

    public RoundEndUI RoundEndui;
    private IEnumerator _danceGamePlayIE;
    [SerializeField]
    private float _danceGameTimer; 
    private IEnumerator DanceGamePlayProgress()
    {
        while (!_bGameEnd)
        {
            if(BPause)
            {
                yield return null;
                continue;
            }

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

                    //处理不同轮次结束后的反应
                    if (CurRound == 0  )
                    {
                        if(BCurRoundHaveSpecialInput)
                        { 
                            ShowButtonDialogueEnd(0, true);
                        }
                        else
                        {
                            ShowButtonDialogueEnd(0, true); 
                        }
                        //教程关上半
                        //DancerCtr.SetDancerAnimatorStauts(DancerController.DancerStatus.Special_1);
                    }
                    else if (CurRound == 1)
                    {
                        if (BCurRoundHaveSpecialInput)
                        {
                            ShowButtonDialogueEnd(0, true);
                        }
                        else
                        {
                            ShowButtonDialogueEnd(0, true);
                        }

                    }
                    else if (CurRound == 2)
                    {
                        if (BCurRoundHaveSpecialInput)
                        {
                            //第一关
                            DancerCtr.SetDancerAnimatorStauts(DancerController.DancerStatus.Special_1);
                            //延时时长要大于人物舞蹈时间，TODO:监听人物舞蹈结束事件
                            _dialogueDisplayDelayTime = 0.5f;
                            SetDiaolgueTextAsset(dialogue_LevelOneSpecialActionEnd);
                            ShowButtonDialogueEnd(0, false); 
                        }
                        else
                        {
                            ShowButtonDialogueEnd(0, true);
                        }
                    }
                    else if (CurRound == 3  )
                    {
                        if (BCurRoundHaveSpecialInput)
                        {
                            //第二关
                            DancerCtr.SetDancerAnimatorStauts(DancerController.DancerStatus.Special_2);
                            //自由意志，失败演出 
                            _dialogueDisplayDelayTime = 2f;
                            SetDiaolgueTextAsset(dialogue_LevelTwoSpecialActionEnd);
                            ShowButtonDialogueEnd(1, false); 
                        }
                        else
                        {
                            ShowButtonDialogueEnd(0, true); 
                        }
                    }
                    else if (CurRound == 4  )
                    {
                        if (BCurRoundHaveSpecialInput)
                        {
                            ShowButtonDialogueEnd(0, true);
                        }
                        else
                        {
                            ShowButtonDialogueEnd(0, true);
                        }
                    }
                    else if (CurRound == 5)
                    {
                        //视为通关
                        if (BCurRoundHaveSpecialInput)
                        {
                            ShowButtonDialogueEnd(2, true);
                        }
                        else
                        {
                            //
                            SetDiaolgueTextAsset(dialogue_LevleFourEnd);
                            ShowButtonDialogueEnd(2, false);
                        }
                    }
                    else
                    {
                        //视为通关
                        if (BCurRoundHaveSpecialInput)
                        {
                            ShowButtonDialogueEnd(2, true);
                        }
                        else
                        { 
                            ShowButtonDialogueEnd(2, false);
                        }
                    }

                    //暂停，等待玩家交互轮次
                    PauseGamePlay(); 
                     

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
    public int FailCount { get => failCount; set
        { 
            failCount = value;
            if(failCountUI.gameObject.activeSelf)
            {
                failCountUI.SetInfo(failCount, BCurRoundHaveSpecialInput);
            }
            if (failCount > 2)
            {
                PauseGamePlay();
                FailCurrentRound();
                if (RoundEndui != null)
                {
                    ShowButtonDialogueEnd(1, true);
                }
            }
        }
    }

    public bool BCurRoundHaveSpecialInput { get => bCurRoundHaveSpecialInput;
        set 
        { 
            bCurRoundHaveSpecialInput = value; 
            if (failCountUI.gameObject.activeSelf)
            {
                failCountUI.SetInfo(FailCount, bCurRoundHaveSpecialInput);
            }
        }
    }

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
            //TODO:记录Order使用情况，有指令尚未处理则生成在位置2(原位置下 子物体0
            if (GameOrders[_curOrderIndex] ==DanceOrder.Up)
            {
                if(OrdersTransforms[0].childCount<=1)
                    _tempDanceOrder = Instantiate(DanceOrderPrefab, OrdersTransforms[0]).GetComponent<DanceOrderDisplay>();
                else
                    _tempDanceOrder = Instantiate(DanceOrderPrefab, OrdersTransforms[0].GetChild(0)).GetComponent<DanceOrderDisplay>();

            }
            else if (GameOrders[_curOrderIndex] == DanceOrder.Down)
            {
                if (OrdersTransforms[1].childCount <= 1)
                    _tempDanceOrder = Instantiate(DanceOrderPrefab, OrdersTransforms[1]).GetComponent<DanceOrderDisplay>();
                else
                    _tempDanceOrder = Instantiate(DanceOrderPrefab, OrdersTransforms[1].GetChild(0)).GetComponent<DanceOrderDisplay>();

            }
            else if (GameOrders[_curOrderIndex] == DanceOrder.Left)
            {
                if (OrdersTransforms[2].childCount <= 1)
                    _tempDanceOrder = Instantiate(DanceOrderPrefab, OrdersTransforms[2]).GetComponent<DanceOrderDisplay>();
                else
                    _tempDanceOrder = Instantiate(DanceOrderPrefab, OrdersTransforms[2].GetChild(0)).GetComponent<DanceOrderDisplay>();


            }
            else if (GameOrders[_curOrderIndex] == DanceOrder.Right)
            {
                if (OrdersTransforms[3].childCount <= 1)
                    _tempDanceOrder = Instantiate(DanceOrderPrefab, OrdersTransforms[3]).GetComponent<DanceOrderDisplay>();
                else
                    _tempDanceOrder = Instantiate(DanceOrderPrefab, OrdersTransforms[3].GetChild(0)).GetComponent<DanceOrderDisplay>();


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

    private float _dialogueDisplayDelayTime;
    public void SpecialActionEnd()
    {
        Invoke(nameof(BeginDialogue), _dialogueDisplayDelayTime);

    }
     
     

    private int failCount = 0;
    public void FailCurOrderInput()
    {
        //GetDanceOrderInput(0, DanceOrder.Left);
        _curInputIndex++;
        FailCount++;
        DancerCtr.SetDancerAnimatorStauts(DancerController.DancerStatus.Wrong);
        DancerCtr.SetDanceMaterialWrongOnce();
        //音效
         
    }
    private bool bCurRoundHaveSpecialInput;
    public void GetDanceOrderInput(int x,DanceOrder dOrder)
    {
        _curInputIndex++;
        if(x==0)
        { 
            FailCount++;
            //失败音效
            AudioManager.Instance.AudioOncePlay(AudioManager.Instance.wrongInteractAudioPiece);

            DancerCtr.SetDancerAnimatorStauts(DancerController.DancerStatus.Wrong);
            DancerCtr.SetDanceMaterialWrongOnce();
             
        }
        //正向
        else if(x==1)
        {
            //成功音效
            AudioManager.Instance.AudioOncePlay(AudioManager.Instance.correctInteractAudioPiece);

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
            BCurRoundHaveSpecialInput = true;
            //成功音效2
            AudioManager.Instance.AudioOncePlay(AudioManager.Instance.specialInteractAudioPiece);
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


    public JoystickController JoystickCtr;
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
                JoystickCtr.SetAnimatorStatus(JoystickController.JoystickStatus.Left);
                GetDanceOrderInput(AlreadyAppearedOrderDisplays[_curInputIndex].GetInteractOrder(DanceOrder.Left, BAllowInverseInput, BMustInverseInput), DanceOrder.Left);
            }
            else if (Input.GetKeyDown(rightOrderKey))
            {
                JoystickCtr.SetAnimatorStatus(JoystickController.JoystickStatus.Right);
                GetDanceOrderInput(AlreadyAppearedOrderDisplays[_curInputIndex].GetInteractOrder(DanceOrder.Right, BAllowInverseInput, BMustInverseInput), DanceOrder.Right);
            }
            else if (Input.GetKeyDown(upOrderKey))
            {
                JoystickCtr.SetAnimatorStatus(JoystickController.JoystickStatus.Up);
                GetDanceOrderInput(AlreadyAppearedOrderDisplays[_curInputIndex].GetInteractOrder(DanceOrder.Up, BAllowInverseInput, BMustInverseInput), DanceOrder.Up);
            }
            else if (Input.GetKeyDown(downOrderKey))
            {
                JoystickCtr.SetAnimatorStatus(JoystickController.JoystickStatus.Down);
                GetDanceOrderInput(AlreadyAppearedOrderDisplays[_curInputIndex].GetInteractOrder(DanceOrder.Down, BAllowInverseInput, BMustInverseInput), DanceOrder.Down);
            }
        }
    }


    public IntroductionCtr IntroCtr;
    public bool BHaveIntro_1;
    public bool BHaveIntro_2;

    public DanceGamePlayInfo failCountUI;

    //延迟一定时间播放
    private TextAsset _curTextAsset;

    public void SetDiaolgueTextAsset(TextAsset targetText)
    { 
        _curTextAsset = targetText;
    }

    public void BeginDialogue()
    { 
        //开始对话
        if (_curTextAsset != null && DialogManager.Instance != null)
            DialogManager.Instance.StartDialog(_curTextAsset);
    }

     

     
    private int _targetActiveButtonIndex;
    /// <summary>
    /// 第一个参数0对应下一轮，1对应重新，2对应结束游戏，第二个参数为true则无需对话立即显示,为false则等对话结束后显示
    /// </summary>
    /// <param name="butttonIndex"></param>
    private void ShowButtonDialogueEnd(int butttonIndex,bool bDisplayAtOnce)
    {
        if (RoundEndui == null)
        {
            Debug.Log("Not Found RoundEndui in DanceGamePlay");
            return;
        }

        if (bDisplayAtOnce)
        {
            RoundEndui.gameObject.SetActive(true);
            RoundEndui.SetButtonShow(butttonIndex);
        }
        else
        {
            _targetActiveButtonIndex = butttonIndex;
        }
    }

    public void OnReceiveDiaolgueEnd()
    {
        if (_targetActiveButtonIndex != -1)
        {
            RoundEndui.gameObject.SetActive(true);
            RoundEndui.SetButtonShow(_targetActiveButtonIndex);
            _targetActiveButtonIndex = -1;
        }

    }


    [Header("DialogueTextAsset")]
    //Round2
    public TextAsset dialogue_LevelOneSpecialActionEnd;
    //Round3
    public TextAsset dialogue_LevelTwoSpecialActionEnd;
    //Round5
    public TextAsset dialogue_LevleFourEnd;
      
}
