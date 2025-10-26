using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.Events;

public class DialogManager : Singleton<DialogManager>
{
    public bool isDialogEnded = true;
    
    public GameObject mask;
    
    public GameObject dialogBox;

    private Vector2 originalSize;

    [SerializeField] private float charsPerSecond;
    
    /// <summary>
    /// 协程句柄，方便打断
    /// </summary>
    private Coroutine typingCoroutine;
    
    /// <summary>
    /// csv-UTH-8格式的文本
    /// </summary>
    public TextAsset dialogTextAsset;
    
    public Animator leftCharaAnim;//现在使用动画器/10/18

    public TextMeshProUGUI dialogText;
    
    /// <summary>
    /// 角色名字
    /// </summary>
    public TextMeshProUGUI leftCharacterName;
    public TextMeshProUGUI rightCharacterName;
    
    
    /// <summary>
    /// 名字->图片字典; 名字->anim Trigger
    /// </summary>
    /*Dictionary<string, Sprite> imageDic = new Dictionary<string, Sprite>();*/
    Dictionary<string,string> animDict = new Dictionary<string,string>();//使用动画

    /// <summary>
    /// 当前对话索引
    /// </summary>
    public int dialogIndex = 0;

    /// <summary>
    /// 分割的对话文本
    /// </summary>
    public string[] dialogRows;
    
    /// <summary>
    /// 是否在选项页面
    /// </summary>
    public bool hasChoices = false;
    
    /// <summary>
    /// 选项按钮预制体
    /// </summary>
    public GameObject optionButtonPrefab;
    
    /// <summary>
    /// 选项按钮的父节点，用于自动排列
    /// </summary>
    public Transform optionButtonGroup;
    
    /// <summary>
    /// 标记当前是否正在显示一个简单的、无角色的消息
    /// </summary>
    private bool isShowingSimpleMessage = false;
    
    /// <summary>
    /// 当前正在显示或打字的完整文本
    /// </summary>
    private string currentFullText;
    
    /// <summary>
    /// 用于存储多句话消息
    /// </summary>
    private string[] currentMessages;
    /// <summary>
    /// 当前显示的多句话消息的索引
    /// </summary>
    private int currentMessageIndex;
    
    protected override void Awake()
    {
        base.Awake();
        //使用动画
        animDict["尼安德·华莱士"] = "Doctor";
        animDict["尼安德"] = "Doctor";

        animDict["机器人"] = "Robot";
        
        animDict["妮娜·奥蜜可"] = "Dancer";
        animDict["来问诊的女人"] = "Dancer";
        animDict["妮娜"] = "Dancer";

        animDict["桥田缪"] = "Programmer";
        
        animDict["德尔塔·布莱梅"] = "Waiter";
        animDict["布莱梅"] =  "Waiter";

        animDict["酒店客人"] = "NoOne";
        animDict["阿拉比"] =  "NoOne";
        animDict["男人的幻影"] = "NoOne";
        animDict["女人的幻影"] =  "NoOne";
    }

    private void Start()
    {
        dialogBox.SetActive(false);//先隐藏，等待调用
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            StartExample();
        }
        
        if(!dialogBox.activeSelf)
            return;
        
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (typingCoroutine != null)
            {
                // 如果在打字，立即停止协程
                StopCoroutine(typingCoroutine);
                typingCoroutine = null;
                // 直接显示完整文本
                dialogText.text = currentFullText;
            }
            else
            {
                if (isShowingSimpleMessage)
                {
                    // 检查是否还有下一句话
                    if (!IsOnLastMessage())
                    {
                        currentMessageIndex++;
                        ShowCurrentMessage(); // 显示下一句话
                    }
                    else
                    {
                        //已经是最后一句话，关闭对话框
                        dialogBox.GetComponent<UI_Dialog>().MoveBack();
                        isShowingSimpleMessage = false;
                        currentMessages = null; // 清理数据
                    }
                }
                else if(!hasChoices)
                {
                    ShowDialog();
                }
            }
        }

        if (!hasChoices && optionButtonGroup.childCount > 0)
        {
            for (int i = 0; i < optionButtonGroup.childCount; i++)
            {
                Destroy(optionButtonGroup.GetChild(i).gameObject);
            }
        }
    }

    private void UpdateText(string _name, string _text, bool isLeft)
    {
        if (isLeft)
            leftCharacterName.text = _name;
        else
        {
            leftCharacterName.text = _name; //左侧分布
            /*rightCharacterName.text = _name; //左右分布*/
        }
        
        //在启动协程前，保存完整文本
        currentFullText = _text;    
        
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(_text));
    }
    
    private IEnumerator TypeText(string fullText)
    {
        currentFullText = fullText;
        dialogText.text = "";                 // 先清空
        float interval = 1f / charsPerSecond; // 单字间隔

        //针对富文本优化 10/20
        for (int i = 0; i < fullText.Length; i++)
        {
            //检查当前字符是否是 '<'
            if (fullText[i] == '<')
            {
                //如果是，就从当前位置向后查找 '>'
                int tagEndIndex = fullText.IndexOf('>', i);
                if (tagEndIndex != -1)
                {
                    // 找到了完整的标签，一次性把它提取出来
                    string tag = fullText.Substring(i, tagEndIndex - i + 1);
                    dialogText.text += tag;
                    i = tagEndIndex;
                    continue;
                }
            }

            dialogText.text += fullText[i];
            yield return new WaitForSeconds(interval);
        }
    
        typingCoroutine = null; // 标记完成
    }

    private void UpdateAnimation(string _name)
    {
        leftCharaAnim.SetTrigger(_name);
    }

    private void ReadText(TextAsset _textAsset)
    {
        dialogRows = _textAsset.text.Split('\n');//分割文本
        
        Debug.Log("文本读取成功！");
    }

    private void ShowDialog()
    {
        isShowingSimpleMessage = false;
        
        for (int i = 1; i < dialogRows.Length; i++)
        {
            string[] cells = dialogRows[i].Split(',');
            
            if(int.Parse(cells[1]) == dialogIndex)
            {
                if (cells[0] == "#")
                {
                    UpdateText(cells[2], cells[4], cells[3] == "左" ? true : false);
                    UpdateAnimation(animDict[cells[2]]);
                    /*UpdateImage(cells[2], cells[3]=="左" ? true:false);*/

                    dialogIndex = int.Parse(cells[5]);
                    break;
                }
                
                if (cells[0] == "&")
                {
                    hasChoices = true;
                    GenerateOption(i);
                    break;
                }

                if (cells[0] == "END")
                {
                    Debug.Log("对话结束");
                    isDialogEnded = true;
                    dialogBox.GetComponent<UI_Dialog>().MoveBack();
                    OnDialogueClose?.Invoke();
                    break;
                }
            }
        }
    }

    private void GenerateOption(int index)
    {
        string[] cells = dialogRows[index].Split(',');
        
        if (cells[0] == "&")
        {
            GameObject button = Instantiate(optionButtonPrefab, optionButtonGroup);
            button.GetComponentInChildren<TextMeshProUGUI>().text = cells[4];
            button.GetComponent<Button>().onClick.AddListener(
                delegate
                {
                    OnOptionClick(int.Parse(cells[5]));
                });
            
            GenerateOption(index + 1);
        }
    }

    private void OnOptionClick(int nextIndex)
    {
        dialogIndex = nextIndex;
        hasChoices = false;
        ShowDialog();
    }
    
    //外部接口
    public void StartDialog(TextAsset dialogAsset)
    {
        isDialogEnded = false;
        
        dialogBox.SetActive(true);
        /*leftCharacter.gameObject.SetActive(true);*/
        leftCharaAnim.gameObject.SetActive(true);
        dialogBox.GetComponent<UI_Dialog>().StartMove();
        
        dialogIndex = 0; 
        isShowingSimpleMessage = false;
        hasChoices = false; // 清除可能存在的选项状态
        
        if (optionButtonGroup.childCount > 0)
        {
            for (int i = 0; i < optionButtonGroup.childCount; i++)
            {
                Destroy(optionButtonGroup.GetChild(i).gameObject);
            }
        }

        ReadText(dialogAsset);
        ShowDialog();
    }
    
    public void ShowMessage(params string[] messages)
    {
        if (messages == null || messages.Length == 0)
        {
            return;
        }
        
        //也可以在之前的基础上继续传递信息
        if (!dialogBox.activeSelf)
        {
            dialogBox.GetComponent<UI_Dialog>().StartMove();
            dialogBox.SetActive(true);
        }

        isShowingSimpleMessage = true;
        hasChoices = false; 
    
        // 存储消息并重置索引
        currentMessages = messages;
        currentMessageIndex = 0;
    
        leftCharaAnim.gameObject.SetActive(true);
    
        // 显示第一句话
        ShowCurrentMessage();
    }
    
    /// <summary>
    /// 显示 currentMessages 数组中当前索引的消息
    /// </summary>
    private void ShowCurrentMessage()
    {
        leftCharacterName.text = "尼安德";
        
        //现实与梦境的形象不一样
        leftCharaAnim.SetTrigger(
            SceneLoadManager.Instance.currentScene == SceneLoadManager.SceneDisplayID.ConsultationRoom ? "Doctor" : "Robot");

        string message = currentMessages[currentMessageIndex];
    
        //在启动协程前，保存完整文本
        currentFullText = message;
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeText(message));
    }
    
    public bool IsOnLastMessage()
    {
        if (!isShowingSimpleMessage)
            return true;
        
        // 必须是在显示简单消息模式下，且消息数组不为空
        if (!isShowingSimpleMessage || currentMessages == null || currentMessages.Length == 0)
        {
            return false;
        }

        // 判断当前索引是否为最后一个
        return currentMessageIndex >= currentMessages.Length - 1;
    }
    
    public bool IsDialogOpen()
    {
        return dialogBox.activeSelf;
    }

    public bool IsTyping()
    {
        return typingCoroutine != null;
    }

    public bool IsDialogEnded()
    {
        return dialogIndex ==  dialogRows.Length - 3;
    }
    
    [Header("Events")]
    public UnityEvent OnDialogueClose;
    
    private void StartExample()
    {
        string[] example = {"<i>一句话中的<link=咖啡豆><color=red>咖啡豆</color></link></i>",
            "第二句话里的<link=粉蔷><color=green>粉蔷</color></link>,第二句话里的<link=披萨><color=green>披萨</color></link>",
            "第三：<link=大衣><color=green>大衣</color></link>,第三句话里的<link=英格里><color=green>英格里</color></link>",
        };
        
        ShowMessage(example);
    }
}