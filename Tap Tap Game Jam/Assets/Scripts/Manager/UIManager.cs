using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [Header("PanelPool")]
    public int PanelPoolSizeCount = 5;
    //存储最近激活的UI，存满后将UI移除
    private Queue<BaseUIPanel> _panelPoolQueue = new Queue<BaseUIPanel>();

    [Header("UIGlobalSettings")]

    private bool bUIHiding;
    private bool bGamePause;

    #region UIPanelDicitionary 存储Panel来通过路径删除

    private Dictionary<string, BaseUIPanel> _panelPathDic = new Dictionary<string, BaseUIPanel>();
    //正在显示的Panel
    private Stack<BaseUIPanel> _panelOpenStack = new Stack<BaseUIPanel>();
    #endregion

    [Header("CanvasObj")]
    //附带的Canvas，Panel将在该层Canvas下 
    public Transform SelfPanelCanvasTransform;
    //最底层UI，当有任意UI激活时作为底层激活，用于遮住场景的UI，避免在有二级UI时还可以交互
    public Image BackgroundCoverImage;

    //[Header("PanelPoolClear")]
    //GC时立即清理PanelPool
    //TODO:切换场景进入加载动画时进行GC
    //public UnityEvent onGameGC;

    protected override void Awake()
    {
        base.Awake();
        //GC清理panel池子
        if (SceneLoadManager.Instance != null)
            SceneLoadManager.Instance.onSceneLoadBegin.AddListener(ClearPanelPool);
    }

    private void OnDisable()
    {
        if (SceneLoadManager.Instance != null)
            SceneLoadManager.Instance.onSceneLoadBegin.RemoveListener(ClearPanelPool);
    }

    /// <summary>
    /// GC前调用，清除PanelPool
    /// </summary>
    public void ClearPanelPool()
    {
        for (int i = 0; i < _panelPoolQueue.Count; i++)
        {
            Destroy(_panelPoolQueue.Peek().gameObject);
        }
        //Destroy(panelPoolList[i]);
    }

    /*public T ShowPanelByType<T>()where T:BaseUIPanel
    {
        return null;
    }*/

    /// <summary>
    /// 若路径找不到Prefab则返回null
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    public T ShowPanelByPath<T>(string path) where T : BaseUIPanel
    {
        string panelName = typeof(T).Name;

        /*// 已打开该UI,则直接返回该UI 
        if (_panelPathDic.ContainsKey(panelName))
        {
            //如果该UI不为当前最高层级则调用其OnResume从暂停状态恢复
            if (_panelOpenStack.Peek().GetType() != typeof(T))
                _panelPathDic[panelName].OnResume();
            return _panelPathDic[panelName] as T;
        }*/
        // 已打开该UI,则不响应，避免越级调动Panel,获取已打开UI限制通过GetPanel来获取
        if (_panelPathDic.ContainsKey(path))
        {
            return null;
        }

        //如果在最近使用池中，则直接激活而非生成
        foreach (var i in _panelPoolQueue)
        {
            if (i.GetType() == typeof(T))
            {

                if (_panelOpenStack.Count > 0)
                {
                    _panelOpenStack.Peek().OnPause();
                }
                _panelOpenStack.Push(i);
                _panelPathDic.Add(path, i);
                i.gameObject.SetActive(true);
                i.OnEnter();
                Debug.Log("Found old Panel in _panelPoolQueue");
                return i as T;
            }
        }


        //加载生成该UI
        GameObject panelObj = GameObject.Instantiate(Resources.Load<GameObject>(path));
        if (panelObj == null)
        {
            Debug.LogError("Error,not found UIPrefab [" + typeof(T).Name + "] at path:" + path);
            return null;
        }
        panelObj.transform.SetParent(SelfPanelCanvasTransform, false);
        T uiPanel = panelObj.GetComponent<T>();
        if (uiPanel == null)
        {
            Debug.LogError("Error,not found Component [" + typeof(T).Name + "]  at path:" + path);
            return null;
        }

        //存储panel字典，用路径作为key
        _panelPathDic.Add(path, uiPanel);
        //如果是第一层UI，则激活背景遮挡，避免与场景UI交互
        if (_panelOpenStack.Count == 0)
        {
            BackgroundCoverImage.gameObject.SetActive(true);
        }
        //处理上一层的UI交互， 调用OnPause
        if (_panelOpenStack.Count > 0)
        {
            _panelOpenStack.Peek().GetComponent<BaseUIPanel>().OnPause();
        }
        //压入当前UI到 已打开的面板 中
        _panelOpenStack.Push(uiPanel);
        //调用当前UI的进入函数
        uiPanel.OnEnter();

        

        return uiPanel;
    }

    /// <summary>
    /// 若目标不在已打开Panel中返回null
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetPanelByPath<T>(string path) where T : BaseUIPanel
    { 
        if (_panelPathDic.ContainsKey(path))
        {
            //如果该UI不为当前最高层级则调用其OnResume从暂停状态恢复
            if (_panelOpenStack.Peek().GetType() != typeof(T))
            {
                //暂停 当前面板
                _panelOpenStack.Peek().OnPause();
                //恢复 目标面板
                _panelPathDic[path].OnResume();
            }
            return _panelPathDic[path] as T;
        }

        return null;
    }

    private Stack<BaseUIPanel> _tempStackToRemove = new Stack<BaseUIPanel>();
    //private Queue<BaseUIPanel> _tempQueue = new Queue<BaseUIPanel>();
    /// <summary>
    /// 返回bool值，若目标Panel存在则将其关闭并返回true，若不存在返回false  
    /// 当移除目标为--非栈顶Panel时，会立即调用其OnResume和OnExit
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public bool HidePanelByPath<T>(string path) where T : BaseUIPanel
    {
        string panelName = typeof(T).Name;
        //未找到目标面板
        if (!_panelPathDic.ContainsKey(path))
        {
            return false;
        }
        //找到目标面板 
        //如果当前UI在openUI栈顶，则弹出
        if (_panelOpenStack.Peek() == _panelPathDic[path])
        {
            _panelOpenStack.Peek().OnExit(); 
            _panelOpenStack.Pop();
            if (_panelOpenStack.Count > 0)
                _panelOpenStack.Peek().OnResume();

        }
        //非栈顶则取出其上方Panel存入tempStack，移除后压回
        else
        {
            _tempStackToRemove.Clear();
            foreach(BaseUIPanel p in _panelOpenStack)
            {
                if (p == _panelPathDic[path])
                    break;
                _tempStackToRemove.Push(p);
            }
            for(int i=0;i<_tempStackToRemove.Count;i++)
            {
                _panelOpenStack.Pop();
            }
            //从Stack中移除目标Panel，立即调用其OnResume和OnExit
            _panelOpenStack.Peek().OnResume();
            _panelOpenStack.Pop().OnExit();
            //压回上方的Panel
            foreach(BaseUIPanel p in _tempStackToRemove)
            {
                _panelOpenStack.Push(p);
            }

        }
        //处理PanelPool相关
        //对象池中已存在则直接结束 TODO?:将已存在的Panel放到末尾
        foreach (BaseUIPanel p in _panelPoolQueue)
        {
            if (p.GetType() == typeof(T))
            {
                _panelPathDic[path].gameObject.SetActive(false);
                _panelPathDic.Remove(path);
                return true;
            }
        }
        //PanelPool中不存在，则存储TODO?:优化，记录UI使用的次数，将使用次数最少的Panel弹出，改用 List或者Dic存储，记录使用次数
        if (_panelPoolQueue.Count < PanelPoolSizeCount)
            _panelPoolQueue.Enqueue(_panelPathDic[path]);
        //面板池已满，则移除最旧的Panel
        else
        {
            GameObject.Destroy(_panelPoolQueue.Dequeue().gameObject);
            _panelPoolQueue.Enqueue(_panelPathDic[path]);
        }
        //隐藏该UI并从_panelPathDic中移除
        _panelPathDic[path].gameObject.SetActive(false);
        _panelPathDic.Remove(path);
        return true;




    }
}

