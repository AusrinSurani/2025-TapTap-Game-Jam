using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.Text;

public class ClickableTextController : MonoBehaviour, IPointerClickHandler
{
    [Header("可收集字词表")] 
    public InventoryWordsData[] wordsData;
    public GameObject inventoryItemPrefab;
    public GameObject container;

    private float timer;
    private bool linksWerePresent = false;
    private bool linksWereTrace = false;
    
    [Header("浮动文字的预制体")]
    public GameObject floatingWordPrefab; 

    private TextMeshProUGUI textMeshPro;
    private Canvas mainCanvas;
    
    private Dictionary<InventoryWordsData, bool> wordsDict = new Dictionary<InventoryWordsData, bool>();

    void Awake()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
        // 获取UI所在的Canvas，用于坐标转换
        mainCanvas = GetComponentInParent<Canvas>();
    }

    private void Start()
    {
        foreach (InventoryWordsData word in wordsData)
        {
            wordsDict.Add(word, false);
        }
    }

    private void Update()
    {
        bool linksArePresent = textMeshPro.textInfo.linkCount > 0;

        //如果当前帧有链接，而上一帧没有
        if (linksArePresent && !linksWerePresent)
        {
            /*Debug.Log("检测到链接文字出现！");*/
            if(!GetTrace())
                StartCoroutine(GenerateTip(DestoryContainer.Instance.warning));
        }

        //如果当前帧没有链接，而上一帧有
        if (!linksArePresent && linksWerePresent)
        {
            /*Debug.Log("文本中的链接已消失。");*/
            DestoryContainer.Instance.warning.SetActive(false);
        }

        //更新状态
        linksWerePresent = linksArePresent;
        
        bool linksAreTrace = GetTrace();

        if (linksAreTrace && !linksWereTrace)
        {
            StartCoroutine(GenerateTip(DestoryContainer.Instance.trace));
        }

        if (!linksAreTrace && linksWereTrace)
        {
            DestoryContainer.Instance.trace.SetActive(false);
        }
        
        timer += Time.deltaTime;
    }

    private IEnumerator GenerateTip(GameObject prefab)
    {
        prefab.SetActive(true);
        timer = 0;
        yield return new WaitUntil(() => timer > 1);
        prefab.SetActive(false);
    }
    
    private bool GetTrace()
    {
        return textMeshPro.text.Contains("<link=痕迹");
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        //检测在鼠标点击位置是否有链接
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(textMeshPro, eventData.position, mainCanvas.worldCamera);

        if (linkIndex != -1)
        {
            TMP_LinkInfo linkInfo = textMeshPro.textInfo.linkInfo[linkIndex];

            //获取被点击的单词
            string clickedWord = linkInfo.GetLinkText();
            Debug.Log($"点击了单词: {clickedWord}, 链接ID为: {linkInfo.GetLinkID()}");
            
            //选择收集的文字信息
            foreach (InventoryWordsData word in wordsData)
            {
                if (clickedWord == word.word && wordsDict[word] == false)
                {
                    wordsDict[word] = true;
                    GenerateItem(word);
                }
            }

            //计算单词在屏幕上的位置
            Vector3 wordPosition = GetWordPosition(linkInfo);

            //在该位置实例化浮动文字预制体
            if (floatingWordPrefab != null)
            {
                GameObject wordInstance = Instantiate(floatingWordPrefab, transform.parent);
                wordInstance.GetComponent<TextMeshProUGUI>().text = clickedWord;
                
                // 设置初始位置
                RectTransform rt = wordInstance.GetComponent<RectTransform>();
                rt.position = wordPosition;
            }

            //将原文中的这个词替换成空格,看成消失
            ReplaceWordWithSpaces(linkInfo);

        }
    }

    /// <summary>
    /// 计算链接单词中心点的世界坐标
    /// </summary>
    private Vector3 GetWordPosition(TMP_LinkInfo linkInfo)
    {
        // 获取单词的第一个和最后一个字符的索引
        int firstCharIndex = linkInfo.linkTextfirstCharacterIndex;
        int lastCharIndex = firstCharIndex + linkInfo.linkTextLength - 1;

        // 获取第一个字符的左下角和最后一个字符的右上角
        Vector3 bottomLeft = textMeshPro.textInfo.characterInfo[firstCharIndex].bottomLeft;
        Vector3 topRight = textMeshPro.textInfo.characterInfo[lastCharIndex].topRight;

        // 计算中心点 (本地坐标)
        Vector3 center = (bottomLeft + topRight) / 2f;

        // 将本地坐标转换为世界坐标
        return textMeshPro.transform.TransformPoint(center);
    }
    
    /// <summary>
    /// 将被点击的链接单词替换成等长的空格
    /// </summary>
    private void ReplaceWordWithSpaces(TMP_LinkInfo linkInfo)
    {
        TMP_TextInfo textInfo = textMeshPro.textInfo;

        StringBuilder sb = new StringBuilder(textMeshPro.text);

        for (int i = 0; i < linkInfo.linkTextLength; i++)
        {
            int charInfoIndex = linkInfo.linkTextfirstCharacterIndex + i;
            
            if (charInfoIndex >= textInfo.characterInfo.Length) continue;

            TMP_CharacterInfo charInfo = textInfo.characterInfo[charInfoIndex];
            
            if (charInfo.isVisible)
            {
                sb[charInfo.index] = ' ';
            }
        }

        //用修改后的字符串更新文本内容
        textMeshPro.text = sb.ToString();

        textMeshPro.ForceMeshUpdate();
    }
    
    private void GenerateItem(InventoryWordsData itemData)
    {
        container = DestoryContainer.Instance.container;
        
        int numOfItems = container.transform.childCount;
        
        float yPosition = -1.5f * numOfItems + 4 + container.transform.position.y;

        GameObject newItem = Instantiate(inventoryItemPrefab, new Vector3(
            container.transform.position.x, yPosition, 9.938788f), Quaternion.identity );
        
        newItem.GetComponent<DraggableItem>().SetUpWords(itemData);
        newItem.transform.SetParent(container.transform);
    }
}