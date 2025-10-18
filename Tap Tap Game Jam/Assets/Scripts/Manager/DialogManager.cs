using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogManager : Singleton<DialogManager>
{
    public GameObject dialogBox;

    public TextAsset textDialog;

    private Vector2 originalSize;
    
    /// <summary>
    /// 打字机效果，每秒显示多少字
    /// </summary>
    [SerializeField] private float charsPerSecond;
    
    /// <summary>
    /// 协程句柄，方便打断
    /// </summary>
    private Coroutine typingCoroutine;
    
    /// <summary>
    /// csv-UTH-8格式的文本
    /// </summary>
    public TextAsset dialogTextAsset;
    
    /// <summary>
    /// 左右的角色图片
    /// </summary>
    /*public Image leftCharacter;
    public Image rightCharacter;*/
    
    public Animator leftCharaAnim;//现在使用动画器/10/18
    
    /// <summary>
    /// 对话文本
    /// </summary>
    public TextMeshProUGUI dialogText;
    
    /// <summary>
    /// 角色名字
    /// </summary>
    public TextMeshProUGUI leftCharacterName;
    public TextMeshProUGUI rightCharacterName;
    
    /// <summary>
    /// 角色图片列表
    /// </summary>
    public List<Sprite> sprites = new List<Sprite>();
    
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
    
    protected override void Awake()
    {
        base.Awake();
        /*imageDic["尼安德·华莱士"] = sprites[0];
        imageDic["尼安德"] = sprites[0];
        
        imageDic["妮娜·奥蜜可"] = sprites[1];
        imageDic["来问诊的女人"] = sprites[1];
        imageDic["妮娜"] = sprites[1];
        
        imageDic["德尔塔·布莱梅"] = sprites[2];
        
        imageDic["桥田缪"] = sprites[3];*/
        
        //使用动画
        animDict["尼安德·华莱士"] = "Robot";
        animDict["尼安德"] = "Robot";
        
        animDict["妮娜·奥蜜可"] = "Dancer";
        animDict["来问诊的女人"] = "Dancer";
        animDict["妮娜"] = "Dancer";

        animDict["德尔塔·布莱梅"] = "Programmer";
        animDict["桥田缪"] = "Waiter";
    }

    private void Start()
    {
        dialogBox.SetActive(false);//先隐藏，等待调用
    }

    private void Update()
    {
        if(!dialogBox.activeSelf)
            return;
        
        if (Input.GetKeyDown(KeyCode.Space))
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
                    dialogBox.GetComponent<UI_Dialog>().MoveBack();
                    isShowingSimpleMessage = false;
                }
                else if(!hasChoices)
                {
                    Debug.Log("begin");
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
        foreach (char c in fullText)
        {
            dialogText.text += c;
            yield return new WaitForSeconds(interval);
        }
        typingCoroutine = null;               // 标记完成
    }

    /*private void UpdateImage(string _name, bool atLeft)
    {
        if (atLeft)
        {
            leftCharacter.sprite = imageDic[_name];
        }
        else
        {
            leftCharacter.sprite = imageDic[_name]; //左侧分布
            /*rightCharacter.sprite = imageDic[_name]; //左右分布#1#
        }
    }*/

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
            
            //Debug.Log(int.Parse(cells[1]));
            //Debug.Log(int.Parse(cells[1]) == dialogIndex);
            
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
                    dialogBox.GetComponent<UI_Dialog>().MoveBack();
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

    private void OptionEffect(string effect, int param, string target)
    {
        
    }
    
    //外部接口
    public void StartDialog(TextAsset dialogAsset)
    {
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
    
    public void ShowMessage(string message)
    {
        dialogBox.GetComponent<UI_Dialog>().StartMove();
        dialogBox.SetActive(true);

        isShowingSimpleMessage = true;
        hasChoices = false; 
        
        leftCharaAnim.gameObject.SetActive(true);
        leftCharaAnim.SetTrigger("Robot");
        leftCharacterName.text = "尼安德";
        
        //在启动协程前，保存完整文本
        currentFullText = message;
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeText(message));
    }

    [ContextMenu("Test")]
    public void TestStart()
    {
        dialogBox.SetActive(true);
        StartDialog(textDialog);
    }
}