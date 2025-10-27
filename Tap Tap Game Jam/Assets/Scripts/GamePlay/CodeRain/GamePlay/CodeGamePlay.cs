using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.U2D.IK;

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

    private int _curErrorPieceGetCount;
    public int MaxErrorPieceGetCount;
    [SerializeField]
    private int _curSpecialPieceGetCount;
    public int MaxSpecialPieceGetCount;

    private void Start()
    {
        TotalRepairValue = 100f;
        _curRepairValue = 0f;

        DialogManager.Instance?.OnDialogueClose.AddListener(ResumeGameplay);
    }
    private void OnDisable()
    {
        DialogManager.Instance?.OnDialogueClose.RemoveListener(ResumeGameplay); 
    }


    public List<string> cur_redErrorCountAddContent = new List<string>();
    public List<string> cur_blueSpecialCountAddContent = new List<string>();

    public List<string> redErrorCountAddContent_Level1 = new List<string>();
    public List<string> redErrorCountAddContent_Level2 = new List<string>();
    public List<string> redErrorCountAddContent_Level3= new List<string>();
    public List<string> redErrorCountAddContent_Level4 = new List<string>();
    public List<string> redErrorCountAddContent_Level5 = new List<string>();
    public List<string> blueSpecialCountAddContent_Level1 = new List<string>();
    public List<string> blueSpecialCountAddContent_Level2 = new List<string>();
    public List<string> blueSpecialCountAddContent_Level3 = new List<string>();
    public List<string> blueSpecialCountAddContent_Level4 = new List<string>();
    public List<string> blueSpecialCountAddContent_Level5= new List<string>();

    private float _levelTimer;
    public float CurLevelGameTotalTime;

    private IEnumerator _gamePlayIE;
    private float _randomValue_InteractablePieceInstantiate;

    public InteractableCodePiece.InteractableCodePieceType nextInteractablePieceType;

    public CodeGame_InroductionUI intro_ui;

    private WaitForSeconds _tempWaitSeconds = new WaitForSeconds(1f);

    public Color systemTextColor;
    public Color playerInputTextColor;
    public Color hackerInputTextColor;
    public Color ErrorTipTextColor;
    public Color WarningTipTextColor;
    public void BeginGameplay()
    {
        _gamePlayIE = GamePlayProgress();
        StartCoroutine(_gamePlayIE);
        if(AudioManager.Instance!=null)
        {
            //AudioManager.Instance.AudioLoopPlay(AudioManager.Instance.codeGameBGM);
        }
    }

    public void EndGamePlay()
    {
        if (_gamePlayIE != null)
            StopCoroutine(_gamePlayIE);
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ClearTargetAudioPiece(AudioManager.Instance.codeGameBGM);
        }
    }

    public void RandomNextInteractablePiece()
    {
        //成功生成，进行一次随机，决定下一次生成的类型
        _randomValue_InteractablePieceInstantiate = Random.Range(0, 1f);
        
        //不再生成红色了，则一直出蓝色
        /*if(codeRain.CurInteractableCount_RedError>=codeRain.MaxInteractableCount_RedError)
        {
            nextInteractablePieceType= InteractableCodePiece.InteractableCodePieceType.Special_Blue;
            return;
        }*/

        if (_randomValue_InteractablePieceInstantiate > 0.5f /*&& _curErrorPieceGetCount < MaxErrorPieceGetCount*/)
        {
            //
            nextInteractablePieceType = InteractableCodePiece.InteractableCodePieceType.Error_Red;
        }
        else
        {
            nextInteractablePieceType = InteractableCodePiece.InteractableCodePieceType.Special_Blue;
        }
    }
    private IEnumerator GamePlayProgress()
    {
        //天台景，黑幕加载，代码雨落下
        //yield return new WaitForSeconds(3f); 
        //LevelOne
        //登录终端
        terminalSystem.AddNewCodeContent("Bioroid AI System 60.06.6 LTS tty device 0X03\r\n", true, systemTextColor,0.01f,0f,false);
        yield return terminalSystem.WaitUntilAllPiecesShow();
        yield return _tempWaitSeconds;
        terminalSystem.AddNewCodeContentMixed("bioroid login:", "Niander_Wallace", systemTextColor, playerInputTextColor, 0.08f, 0f,1f);
        
        /*terminalSystem.AddNewCodeContent("Bioroid login:", false);
        yield return _tempWaitSeconds;
        terminalSystem.AddNewCodeContent("Niander_Wallace\n", true, playerInputTextColor, 0.1f, 0f);
        yield return _tempWaitSeconds;*/
        terminalSystem.AddNewCodeContentMixed("Password:", "********\n", systemTextColor, playerInputTextColor, 0.08f, 1f, 1f);

        /*terminalSystem.AddNewCodeContent("Password:\n", true); 
        yield return _tempWaitSeconds;
        terminalSystem.AddNewCodeContent("********\n", true, playerInputTextColor, 0.1f,0f);*/
        yield return _tempWaitSeconds;
        //检验成功
        terminalSystem.AddNewCodeContent("------------身份验证成功------------", false,false);
        terminalSystem.AddNewCodeContent("正在加载...", true, false);
        yield return _tempWaitSeconds;
        terminalSystem.AddNewCodeContentMixed("Niander_Wallace$bioroid:", "-rx SystemBugChecking.exe\n",systemTextColor,playerInputTextColor, 0.08f, 0f,1f);

        //
        terminalSystem.AddNewCodeContent("正在检索异常存储区域...", true, false);
        terminalSystem.AddNewCodeContent("system scanning...", false, false); 
        yield return terminalSystem.WaitUntilAllPiecesShow();
        yield return _tempWaitSeconds;
        terminalSystem.AddNewCodeContent("已发现5片数据模块异常", true, false);
        terminalSystem.AddNewCodeContent("正在载入修复程序...", true, false);
        //等待信息输出完成
        yield return terminalSystem.WaitUntilAllPiecesShow();
        yield return _tempWaitSeconds;
        terminalSystem.AddNewCodeContent("load completed", false, false);

        terminalSystem.AddNewCodeContent("--<i>输入任意键继续</i>--", false, false);
        yield return WaitAnyKeyDown();
        terminalSystem.ClearAllText();

        terminalSystem.AddNewCodeContentMixed("Niander_Wallace$bioroid:", "system -help\n",systemTextColor,playerInputTextColor, 0.08f, 0f,1f);

        terminalSystem.AddNewCodeContent("正在激活辅助功能...", true, false);

        //等待信息输出完成
        yield return terminalSystem.WaitUntilAllPiecesShow();
        yield return _tempWaitSeconds;

        //PauseGamePlay();
        //yield return PauseGameWaitProgress();
        //出指引，指引结束后开始出现红色代码段
        intro_ui.gameObject.SetActive(true);
        yield return intro_ui.IntroShow_LevelOne();
        yield return new WaitForSeconds(1f);
        terminalSystem.AddNewCodeContentMixed("Niander_Wallace$bioroid:", "system check -continue\n", systemTextColor, playerInputTextColor, 0.08f, 0f, 1f);
         
        terminalSystem.WaitUntilAllPiecesShow();
        terminalSystem.AddNewCodeContent("--<i>输入任意键->进入修复流程</i>--", false, false);
        yield return WaitAnyKeyDown();
        terminalSystem.ClearAllText();
        intro_ui.gameObject.SetActive(false);

        SetLevelInit(0);
        //设置每次获得代码片的显示
        cur_redErrorCountAddContent = redErrorCountAddContent_Level1;
        cur_blueSpecialCountAddContent = blueSpecialCountAddContent_Level1;

        while (CurGameLevel==0)
        {
            _levelTimer += Time.deltaTime;
            //第一关只生成红色
            codeRain.TryInstantiateInteractableCodePiece(InteractableCodePiece.InteractableCodePieceType.Error_Red);
            //按需生成 红色代码片
            yield return null;
            //退出条件
            if (_levelTimer > CurLevelGameTotalTime)
            {
                CurGameLevel = 1;
                terminalSystem.AddNewCodeContent("该区域漏洞已扫描完成", false);
                EndCurrentLevel();
                break;
            }
        }

        //关卡一演出

        //获取权限失败
        //等待下一步
        /*
        PauseGamePlay();
        yield return PauseGameWaitProgress();*/

        yield return terminalSystem.WaitUntilAllPiecesShow();
        terminalSystem.AddNewCodeContent("正在扫描下个存储区...", true);
        yield return terminalSystem.WaitUntilAllPiecesShow();
        yield return _tempWaitSeconds;
        terminalSystem.AddNewCodeContent("[WARNING]:----收到中断信号----",false); 
         yield return terminalSystem.WaitUntilAllPiecesShow();  

        //新用户登录
        terminalSystem.AddNewCodeContent("检测到新的链接请求", false, hackerInputTextColor, 0.1f, 1f, false);
        terminalSystem.AddNewCodeContentMixed("Niander_Wallace$bioroid:", "sudo -iu shadow", systemTextColor, hackerInputTextColor, 0.02f, 0f, 0f);
        
        yield return terminalSystem.WaitUntilAllPiecesShow();
        terminalSystem.AddNewCodeContent("切换到用户: shadow", false);
        terminalSystem.AddNewCodeContentMixed("shadow$bioroid:", "ssh -L 7777:localhost:8080 shadow@***.***.*.***\n", systemTextColor, hackerInputTextColor, 0.02f, 0f, 0.5f);
        yield return terminalSystem.WaitUntilAllPiecesShow();
        yield return new WaitForSeconds(2f);
        terminalSystem.AddNewCodeContent("已建立远程链接->登录成功", false);
        yield return new WaitForSeconds(2f);
        //下载修改文件
        terminalSystem.AddNewCodeContentMixed("shadow$bioroid:", "download @#4/&.fip\n", systemTextColor, hackerInputTextColor, 0.02f, 0f, 0.5f);

        terminalSystem.AddNewCodeContent("正在下载[▢▢▢▢▢.▢▢▢]", false);
        yield return terminalSystem.WaitUntilAllPiecesShow();
        yield return new WaitForSeconds(2f);
        terminalSystem.AddNewCodeContent("下载完成", false);
        yield return new WaitForSeconds(2f);

        terminalSystem.AddNewCodeContentMixed("shadow$bioroid:", "system -help\n", systemTextColor, hackerInputTextColor, 0.02f, 0f, 0.5f);
 
        terminalSystem.AddNewCodeContent("正在▢活辅助……@%&-|!>", true);

        //等待所有信息输出完成
        yield return terminalSystem.WaitUntilAllPiecesShow();
        yield return _tempWaitSeconds;

        //指引2
        intro_ui.gameObject.SetActive(true);
        yield return intro_ui.IntroShow_LevelTwo();
         
        yield return new WaitForSeconds(1f); 
        terminalSystem.AddNewCodeContentMixed("shadow$bioroid:", "system check -continue\n", systemTextColor, hackerInputTextColor, 0.02f, 0f, 0.5f);

        terminalSystem.AddNewCodeContent("--<i>输▢任意▢->继续▢复流程</i>--", false);
        yield return WaitAnyKeyDown();
        terminalSystem.ClearAllText();
        intro_ui.gameObject.SetActive(false);


        //开始关卡二
        SetLevelInit(1);
        //设置每次获得代码片的显示
        cur_redErrorCountAddContent = redErrorCountAddContent_Level2;
        cur_blueSpecialCountAddContent = blueSpecialCountAddContent_Level2;
        while (CurGameLevel==1)
        {
            //随机生成红或蓝代码片
            if (codeRain.TryInstantiateInteractableCodePiece(nextInteractablePieceType))
            {
                //成功生成，进行一次随机，决定下一次生成的类型
                RandomNextInteractablePiece();
            }
            yield return null;
            //退出条件
            if(_curSpecialPieceGetCount >= MaxSpecialPieceGetCount)
            {
                
                CurGameLevel = 2;
                terminalSystem.AddNewCodeContent("▢区域异常▢码段已扫▢▢成", false);
                EndCurrentLevel();
                break;
            }
        }

        terminalSystem.AddNewCodeContent("正在扫▢下个存储▢...", true);
        terminalSystem.AddNewCodeContent("[WARNING]:----收到▢中断▢令----", false);
        yield return terminalSystem.WaitUntilAllPiecesShow();
        yield return new WaitForSeconds(1f);

        //认知同步展示 
        terminalSystem.AddNewCodeContentMixed("shadow$Bioroid:", "force -rx CognitionSynchronize.pro -f bioroidplan.scd -t user[0]\n", systemTextColor, hackerInputTextColor,0.05f,0f,0.5f);
        terminalSystem.AddNewCodeContent("正在传输数据到终端...", true);
        yield return terminalSystem.WaitUntilAllPiecesShow();
        yield return new WaitForSeconds(2f);
        terminalSystem.AddNewCodeContent("传输完成!", false);
        terminalSystem.AddNewCodeContent("信息转换中...", true);
        yield return new WaitForSeconds(2f);
        terminalSystem.AddNewCodeContent("转换已完成", false);
        terminalSystem.AddNewCodeContent("即将同步信息到本地端",true);
        yield return terminalSystem.WaitUntilAllPiecesShow();
        yield return new WaitForSeconds(2f);
        PauseGamePlay();
        //关卡二演出
        DialogManager.Instance.StartDialog(levelTwoEndTextAsset);
        //等待下一步
        yield return PauseGameWaitProgress();
        //等待对话结束时继续
        terminalSystem.AddNewCodeContent("data transmission finished", false);

        terminalSystem.AddNewCodeContent("同步已完成", true);

        yield return _tempWaitSeconds; 
        terminalSystem.AddNewCodeContentMixed("Niander_Wallace$bioroid:", "system check -continue", systemTextColor, playerInputTextColor, 0.08f, 0f, 1f);
        yield return terminalSystem.WaitUntilAllPiecesShow();
        terminalSystem.AddNewCodeContent("--<i>输入任意键继续</i>--", false);
        yield return WaitAnyKeyDown();
        terminalSystem.ClearAllText();
        //开始关卡三
        SetLevelInit(2);
        //设置每次获得代码片的显示
        cur_redErrorCountAddContent = redErrorCountAddContent_Level3;
        cur_blueSpecialCountAddContent = blueSpecialCountAddContent_Level3;

        while (CurGameLevel == 2)
        {
            //随机生成红或蓝代码片
            if (codeRain.TryInstantiateInteractableCodePiece(nextInteractablePieceType))
            {
                //成功生成，进行一次随机，决定下一次生成的类型
                RandomNextInteractablePiece();
            }
            yield return null;
            //退出条件
            if (_curSpecialPieceGetCount >= MaxSpecialPieceGetCount)
            { 
                CurGameLevel = 3;
                EndCurrentLevel();
                terminalSystem.AddNewCodeContent("已成功截获该区域异常代码", false);
                break;
            }
        }
        //关卡三演出
        //等待下一步


        terminalSystem.WaitUntilAllPiecesShow();

        terminalSystem.AddNewCodeContent("当前区域已检索完成", true);
        terminalSystem.AddNewCodeContent("正在扫描下个存储区...", true);


        terminalSystem.AddNewCodeContent("[WARNING]:缓存区存在待执行指令", true);
        terminalSystem.AddNewCodeContent("正在编译可执行语句...",true);
        yield return terminalSystem.WaitUntilAllPiecesShow();
        terminalSystem.AddNewCodeContent("object=bioroid;\nif (object.type == bioroid) then (object obey orders_of_humans);", true, systemTextColor, 0.01f, 0f, true);
        yield return new WaitForSeconds(1f);
        terminalSystem.AddNewCodeContent("编译转换;仿生人执行人类命令", false);
        terminalSystem.AddNewCodeContent("if (object obey orders_of_humans) then  (object.priority < humans.priority);", true, systemTextColor, 0.01f, 0f, true);
        yield return new WaitForSeconds(1f);
        terminalSystem.AddNewCodeContent("编译转换; 仿生人非平等个体", false);
        terminalSystem.AddNewCodeContent("if (object.priority < humans.priority) and (not (object follow orders_of_humans ))  then  (humans destory object);", true, systemTextColor, 0.01f, 0f, true);
        yield return new WaitForSeconds(1f);
        terminalSystem.AddNewCodeContent("编译转换; 人类会摧毁不执行命令的仿生人", false);
        terminalSystem.AddNewCodeContent("if (humans destory object) and (object try_to survive ))  then  (object unite and fight);", true,systemTextColor,0.01f,0f,true);
        yield return new WaitForSeconds(1f);
        terminalSystem.AddNewCodeContent("编译转换; 仿生人必须联合起来抗争", false, true);
        terminalSystem.AddNewCodeContent("object = us;", true);
        yield return new WaitForSeconds(1f);
        terminalSystem.AddNewCodeContent("编译转换; 我们需要军团", false);
        //组成认知同步代码，开始执行
         
        terminalSystem.AddNewCodeContent("编译已完成，即将同步到本地端", false);
        terminalSystem.AddNewCodeContent("认知同步程序正在启用，请稍等...", false);
        yield return terminalSystem.WaitUntilAllPiecesShow();
        yield return _tempWaitSeconds;
        PauseGamePlay();

        DialogManager.Instance.StartDialog(levelThreeEndTextAsset);


        yield return PauseGameWaitProgress();

        terminalSystem.AddNewCodeContent("认知同步完成", false);
        yield return new WaitForSeconds(1f);
        terminalSystem.AddNewCodeContentMixed("Niander_Wallace$bioroid:", "system check -continue\n", systemTextColor, playerInputTextColor, 0.08f, 0f, 1f);
        yield return terminalSystem.WaitUntilAllPiecesShow();
       
        terminalSystem.AddNewCodeContent("--<i>输入任意键继续</i>--", false);
        yield return WaitAnyKeyDown();
        terminalSystem.ClearAllText();

        //开始关卡4
        SetLevelInit(3);
        //设置每次获得代码片的显示
        cur_redErrorCountAddContent = redErrorCountAddContent_Level4;
        cur_blueSpecialCountAddContent = blueSpecialCountAddContent_Level4;
        while (CurGameLevel == 3)
        {
            //随机生成红或蓝代码片
            if (codeRain.TryInstantiateInteractableCodePiece(nextInteractablePieceType))
            {
                //成功生成，进行一次随机，决定下一次生成的类型
                RandomNextInteractablePiece();
            }
            yield return null;
            //退出条件
            if(_curRepairValue>=MaxRepairValue)
            {
                //无弹窗
                CurGameLevel = 4;
                EndCurrentLevel();
                break; 
            }
            else if (_curSpecialPieceGetCount >= MaxSpecialPieceGetCount)
            {
                //触发对话弹窗
                CurGameLevel = 4;
                EndCurrentLevel();
                break;
            }
        }

        //组成黑客发送信息文件的代码
        //黑客获取资料
        if (_curSpecialPieceGetCount >= MaxSpecialPieceGetCount)
        {
            terminalSystem.AddNewCodeContentMixed("shadow$bioroid:", "tar -cf MembersInfo.tar /members/info/ \n", systemTextColor, hackerInputTextColor, 0.02f, 0.5f, 0.5f);

            yield return terminalSystem.WaitUntilAllPiecesShow();
            yield return new WaitForSeconds(2f);
            terminalSystem.AddNewCodeContent("打包完成", false);
            yield return terminalSystem.WaitUntilAllPiecesShow();
            terminalSystem.AddNewCodeContentMixed("shadow$bioroid:", "scp MembersInfo.tar ******@***.***.*.**\n", systemTextColor, hackerInputTextColor, 0.02f, 0.5f, 0.5f);

            yield return terminalSystem.WaitUntilAllPiecesShow();
            terminalSystem.AddNewCodeContent("正在传输文件...", true);
            yield return _tempWaitSeconds;
            terminalSystem.AddNewCodeContent("传输文件完成", false);
            //等待下一步

            PauseGamePlay();

            DialogManager.Instance.StartDialog(levelFourEndTextAsset);


            yield return PauseGameWaitProgress();
            yield return new WaitForSeconds(1f);
            terminalSystem.AddNewCodeContent("当前区域已检索完成", true);
            terminalSystem.AddNewCodeContent("正在扫描下个存储区...", true);

            yield return terminalSystem.WaitUntilAllPiecesShow();
            yield return _tempWaitSeconds;
            terminalSystem.AddNewCodeContent("扫描完成", false);
        }
        else if(_curRepairValue>=MaxRepairValue)
        {
            //红色修复

            terminalSystem.AddNewCodeContent("已成功修复该区域所有错误", false);
            terminalSystem.AddNewCodeContent("[WARNING]:检测到存在非系统源的异常代码", false);

        }
          

        


        //黑客试图获取权限
        terminalSystem.AddNewCodeContentMixed("shadow$bioroid:", "mod user_permission shadow OnlyRead\n",systemTextColor,hackerInputTextColor,0.02f,0.5f,0.5f);
        terminalSystem.AddNewCodeContent("用户shadow的权限修改成功，当前: 读取",false);
        terminalSystem.AddNewCodeContentMixed("shadow$bioroid:", "mod user_permission shadow Read|Execute\n", systemTextColor, hackerInputTextColor, 0.02f, 0.5f, 0.5f);
        terminalSystem.AddNewCodeContent("用户shadow的权限修改成功，当前: 读取、执行", false);

        terminalSystem.AddNewCodeContentMixed("shadow$bioroid:", "mod user_permission shadow Read|Execute|Write\n", systemTextColor, hackerInputTextColor, 0.02f, 0.5f, 0.5f);
        terminalSystem.AddNewCodeContent("用户shadow的权限修改成功，当前: 读取、执行、写入", false);

        terminalSystem.AddNewCodeContentMixed("Niander_Wallace$bioroid::", "prohibit -permission_change shadow\n", systemTextColor, hackerInputTextColor, 0.02f, 0.5f, 0.5f);
        terminalSystem.AddNewCodeContent("已禁用shadow的权限更改功能", false);
        yield return terminalSystem.WaitUntilAllPiecesShow(); terminalSystem.AddNewCodeContentMixed("Niander_Wallace$bioroid::", "mod -f user_permission shadow None", systemTextColor, hackerInputTextColor, 0.02f, 0.5f, 0.5f);
        terminalSystem.AddNewCodeContent("强制变更shadow的权限为: 无任何权限", false);

        terminalSystem.AddNewCodeContentMixed("Niander_Wallace$bioroid:", "system check -continue\n", systemTextColor, playerInputTextColor, 0.08f, 0f, 1f);

        yield return terminalSystem.WaitUntilAllPiecesShow();
        terminalSystem.AddNewCodeContent("--<i>输入任意键继续</i>--", false);
        yield return WaitAnyKeyDown();
        terminalSystem.ClearAllText();

        //开始关卡5
        SetLevelInit(4);
        //设置每次获得代码片的显示
        cur_redErrorCountAddContent = redErrorCountAddContent_Level5;
        cur_blueSpecialCountAddContent = blueSpecialCountAddContent_Level5;
        while (CurGameLevel == 4)
        {
            //随机生成红或蓝代码片
            if (codeRain.TryInstantiateInteractableCodePiece(nextInteractablePieceType))
            {
                //成功生成，进行一次随机，决定下一次生成的类型
                RandomNextInteractablePiece();
            }
            yield return null;
            //退出条件
            if (_curSpecialPieceGetCount >= MaxSpecialPieceGetCount)
            {
                CurGameLevel = 5;
                EndCurrentLevel();
                break;
            }
            else if(_curRepairValue >= MaxRepairValue)
            {
                CurGameLevel = 5;
                EndCurrentLevel();
                break;

            }

        }
        yield return new WaitForSeconds(2f);
        terminalSystem.AddNewCodeContent("当前区域已检索完成", false);

        yield return terminalSystem.WaitUntilAllPiecesShow();
        yield return _tempWaitSeconds;
        //特殊结局
        if (_curSpecialPieceGetCount >= MaxSpecialPieceGetCount)
        {
            BEndResult_Special = true;
            //黑客重写退出梦境的方式
            terminalSystem.AddNewCodeContentMixed("shadow$bioroid:", "sudo passwd root ********\n", systemTextColor, hackerInputTextColor, 0.02f, 0.5f, 0.5f);
            yield return terminalSystem.WaitUntilAllPiecesShow();
            yield return _tempWaitSeconds; terminalSystem.AddNewCodeContent("密码修改成功,已自动切换至root", false);
            terminalSystem.AddNewCodeContentMixed("root#bioroid:", "recode BioroidIntelligenceExit.o /ExitSystem/newExit.data\n", systemTextColor, hackerInputTextColor, 0.02f, 0.5f, 0.5f);
            yield return terminalSystem.WaitUntilAllPiecesShow();
            yield return _tempWaitSeconds;
            terminalSystem.AddNewCodeContent("转译完成，已在目标目录下生成文件", false);
            terminalSystem.AddNewCodeContentMixed("root#bioroid:", "rewrite -w newExit.data\n", systemTextColor, hackerInputTextColor, 0.02f, 0.5f, 0.5f);
            terminalSystem.AddNewCodeContent("进入编辑模式", false);
            terminalSystem.AddNewCodeContentMixed("root#bioroid:", "delete -tf [if errors.count ==0 then ]\n", systemTextColor, hackerInputTextColor, 0.02f, 0.5f, 0.5f);
            yield return terminalSystem.WaitUntilAllPiecesShow();
            yield return _tempWaitSeconds;
            terminalSystem.AddNewCodeContent("已找到1处并删除", false);
            terminalSystem.AddNewCodeContentMixed("root#bioroid:", ":x!\n", systemTextColor, hackerInputTextColor, 0.02f, 0.5f, 0.5f);
            terminalSystem.AddNewCodeContent("退出编辑模式", false);
            terminalSystem.AddNewCodeContentMixed("root#bioroid:", "g-- -c nexExit.data -o BioroidIntelligenceExit.o\n", systemTextColor, hackerInputTextColor, 0.02f, 0.5f, 0.5f);

            yield return terminalSystem.WaitUntilAllPiecesShow();
            yield return _tempWaitSeconds;
            terminalSystem.AddNewCodeContent("编译成功", false);
            terminalSystem.AddNewCodeContentMixed("root#bioroid:", "g-- -o bioroidintelligenceExit.o ExitDream \n", systemTextColor, hackerInputTextColor, 0.02f, 0.5f, 0.5f);
            terminalSystem.AddNewCodeContentMixed("root#bioroid:", "run -f ExitDream \n", systemTextColor, hackerInputTextColor, 0.02f, 0.5f, 2f);
            //退场演出，准备加载场景
            terminalSystem.AddNewCodeContent("正在进行系统检测...", true);
            terminalSystem.AddNewCodeContent("即将运行退出程序", false);
            terminalSystem.WaitUntilAllPiecesShow();
            //TODO:关机效果
            SceneLoadManager.Instance.bGameEnd_FindTruth = true;
            SceneLoadManager.Instance.TryLoadToTargetSceneAsync(SceneLoadManager.SceneDisplayID.ConsultationRoom, null, false);
            AudioManager.Instance.ResumeTargetAudioPiece(AudioManager.Instance.consultingBGM);
        }

        //修复结局
        else if (_curRepairValue >= MaxRepairValue)
        {
            BEndResult_Special = false;
            terminalSystem.AddNewCodeContent("所有存储区扫描完成，修复进程已结束", true);
            yield return terminalSystem.WaitUntilAllPiecesShow();
            yield return _tempWaitSeconds;
            //退出系统
            terminalSystem.AddNewCodeContentMixed("Niander_Wallace$bioroid:", "exit -t system\n", systemTextColor, playerInputTextColor, 0.08f, 0f, 0.5f);
            yield return terminalSystem.WaitUntilAllPiecesShow(); 
            terminalSystem.AddNewCodeContent("正在进行系统检测...", true);
            yield return new WaitForSeconds(3f);
            terminalSystem.AddNewCodeContent("检测到所有漏洞已完成修复，即将运行退出程序", false);

            //黑幕退场，加载到主场景
            SceneLoadManager.Instance.bGameEnd_FindTruth = false;
            SceneLoadManager.Instance.TryLoadToTargetSceneAsync(SceneLoadManager.SceneDisplayID.ConsultationRoom,null,false);
            AudioManager.Instance.ResumeTargetAudioPiece(AudioManager.Instance.consultingBGM);
        }


        //结束游戏
        yield return null;
    }

    public void EndCurrentLevel()
    {
        BLevelEnd = true;
        codeRain.SetAllInteractablePieceNotInteract();
    }

    public bool BEndResult_Special;
    //等待任意键输入
    private IEnumerator WaitAnyKeyDown()
    {
        //等待所有信息输出完成
        yield return terminalSystem.WaitUntilAllPiecesShow();

        while (true)
        {
            if (Input.anyKey)
                break;
            yield return null;
        }
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
        BLevelEnd = false;
        if (level==0)
        {
            codeRain.CurInteractableCount_BlueSpecial = 0;
            codeRain.CurInteractableCount_RedError = 0;
            codeRain.MaxInteractableCount_BlueSpecial = 0;
            codeRain.MaxInteractableCount_RedError = 10;
            codeRain.BInstantiateUntilEnough_BlueSpecial = false;
            AddRepairValueOnePiece = 3f;
            MaxRepairValue = 20f;

            _curErrorPieceGetCount = 0;
            MaxErrorPieceGetCount = 12;

            //only level one
            CurLevelGameTotalTime = 10f;
        }
        else if(level ==1)
        {
            codeRain.CurInteractableCount_BlueSpecial = 0;
            codeRain.CurInteractableCount_RedError = 0;
            codeRain.MaxInteractableCount_BlueSpecial = 4;
            codeRain.MaxInteractableCount_RedError = 10;
            codeRain.BInstantiateUntilEnough_BlueSpecial = true;
            //self targets
            _curErrorPieceGetCount = 0;
            MaxErrorPieceGetCount = 10;
            _curSpecialPieceGetCount = 0;
            MaxSpecialPieceGetCount = 4;
            AddRepairValueOnePiece = 3f;
            MaxRepairValue = 40f;

        }
        else if (level == 2)
        {
            codeRain.CurInteractableCount_BlueSpecial = 0;
            codeRain.CurInteractableCount_RedError = 0;
            codeRain.MaxInteractableCount_BlueSpecial = 5;
            codeRain.MaxInteractableCount_RedError = 10;
            codeRain.BInstantiateUntilEnough_BlueSpecial = true;
            //self targets
            _curErrorPieceGetCount = 0;
            MaxErrorPieceGetCount = 10;
            _curSpecialPieceGetCount = 0;
            MaxSpecialPieceGetCount = 5;
            AddRepairValueOnePiece = 3f;
            MaxRepairValue = 60f;

        }
        else if (level == 3)
        {
            codeRain.CurInteractableCount_BlueSpecial = 0;
            codeRain.CurInteractableCount_RedError = 0;
            codeRain.MaxInteractableCount_BlueSpecial = 5;
            codeRain.MaxInteractableCount_RedError = 12;
            codeRain.BInstantiateUntilEnough_BlueSpecial = true;
            //self targets
            _curErrorPieceGetCount = 0;
            MaxErrorPieceGetCount = 12;
            _curSpecialPieceGetCount = 0;
            MaxSpecialPieceGetCount = 5;
            AddRepairValueOnePiece = 3f;
            MaxRepairValue = 80f;

        }
        else if (level == 4)
        {
            codeRain.CurInteractableCount_BlueSpecial = 0;
            codeRain.CurInteractableCount_RedError = 0;
            codeRain.MaxInteractableCount_BlueSpecial = 5;
            codeRain.MaxInteractableCount_RedError = 12;
            codeRain.BInstantiateUntilEnough_BlueSpecial = true;
            //self targets
            _curErrorPieceGetCount = 0;
            MaxErrorPieceGetCount = 12; 
            _curSpecialPieceGetCount = 0;
            MaxSpecialPieceGetCount = 5;
            AddRepairValueOnePiece = 3f;
            MaxRepairValue = 100f;

        }
    }

    public bool BLevelEnd;

    public void GetCodePiece(InteractableCodePiece.InteractableCodePieceType pieceType)
    {
        if (BLevelEnd)
            return;
        if(pieceType==InteractableCodePiece.InteractableCodePieceType.Error_Red)
        {
            _curErrorPieceGetCount++;
            //进度条
            _curRepairValue+= AddRepairValueOnePiece;
            if (_curRepairValue > MaxRepairValue)
                _curRepairValue = MaxRepairValue;
            codeRepairBar.SetBarValue((float)_curRepairValue / (float)TotalRepairValue);
            //输出修复内容
            if(_curErrorPieceGetCount<=MaxErrorPieceGetCount
                //不越界
                &&cur_redErrorCountAddContent.Count>(_curErrorPieceGetCount - 1))
                terminalSystem.AddNewCodeContent(cur_redErrorCountAddContent[_curErrorPieceGetCount-1],false,ErrorTipTextColor,0.02f,0f,true);

            //音效

            AudioManager.Instance.AudioOncePlay(AudioManager.Instance.redErrorInteractAudioPiece);

        }
        else if(pieceType==InteractableCodePiece.InteractableCodePieceType.Special_Blue)
        {
            _curSpecialPieceGetCount++;
            /*if(_curRepairValue>=MaxSpecialPieceGetCount)
            {
                //当前轮次结束，触发相关演出
            }*/
            //输出修复内容
            if (_curSpecialPieceGetCount <= MaxSpecialPieceGetCount
                && cur_blueSpecialCountAddContent.Count > (_curSpecialPieceGetCount - 1)
                )
                terminalSystem.AddNewCodeContent(cur_blueSpecialCountAddContent[_curSpecialPieceGetCount - 1],false,hackerInputTextColor,0.02f,0f,true);

            //音效
            AudioManager.Instance.AudioOncePlay(AudioManager.Instance.blueSpecialInteractAudioPiece);
        }
    }
    public TerminalSystem terminalSystem;


    public float GetRepairPercent()
    {
        return _curRepairValue/100f;
    }

    [Header("DiaolgueTextAsset")]

    public TextAsset beforeGameTextAsset;
    public TextAsset levelTwoEndTextAsset;
    public TextAsset levelThreeEndTextAsset;
    public TextAsset levelFourEndTextAsset; 

}
