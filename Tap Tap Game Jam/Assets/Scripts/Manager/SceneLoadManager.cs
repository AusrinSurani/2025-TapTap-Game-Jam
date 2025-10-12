using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement; 

public class SceneLoadManager : Singleton<SceneLoadManager>
{
    //test
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
           // TryLoadToTargetSceneAsync(SceneDisplayID.Level_1);
        }
    }

    //endtest


    protected override void Awake()
    {
        base.Awake();
        //将List数据存入dictionary中
        ImportSceneDataFromListToDictionary();
    }

    #region ScenePath输入

    //与scensPath保持对应
    public List<SceneDisplayID> scenesDisplayID = new List<SceneDisplayID>();
    public List<string> scenesPath = new List<string>();
     
    public bool ImportSceneDataFromListToDictionary()
    {
        if (scenesDisplayID.Count != scenesPath.Count)
            return false;
        if (scenesDisplayID.Count == 0)
            return false;
        _sceneFile_DisplayIDAndPath.Clear();
        for(int i=0;i<scenesDisplayID.Count;i++)
        {
            if (!_sceneFile_DisplayIDAndPath.ContainsKey(scenesDisplayID[i]))
            {
                _sceneFile_DisplayIDAndPath.Add(scenesDisplayID[i], scenesPath[i]); 
            }
            else
            {
                Debug.LogError("Fail to Import ScenePath Data: Repeat Key [" + scenesDisplayID[i].ToString() + "] at index " + i + " .");
                return false;
            }
        }
        Debug.Log("dic count:" + _sceneFile_DisplayIDAndPath.Count);
        return true;
    }

    #endregion
    /// <summary>
    ///datatype: sceneName,scenePath
    /// </summary>
    private Dictionary<SceneDisplayID, string> _sceneFile_DisplayIDAndPath = new Dictionary<SceneDisplayID, string>();
    public enum SceneDisplayID
    {
        StartMenu,
        Level_1,
        EnterDreamLoad,
        DanceDream,

    }


    /// <summary>
    /// 同步加载，如找不到对应的场景,则return false 
    /// </summary>
    /// <param name="targetSceneID"></param>
    /// <returns></returns>
    public bool LoadToTargetScene(SceneDisplayID targetSceneID)
    {
        
        string scenePath = "";
        foreach (var i in _sceneFile_DisplayIDAndPath)
        {
            if (i.Key == targetSceneID )
            {
                scenePath = i.Value;
                break;
            }
        }
        if (scenePath != "")
        {
            //TODO: 加载过渡界面



            //调用GC
            System.GC.Collect();

            onSceneLoadBegin?.Invoke();
             
            SceneManager.LoadScene(SceneUtility.GetBuildIndexByScenePath(scenePath));
             
            onSceneLoadEnd?.Invoke();

            return true;
        }
        else
        { 
            Debug.Log("Load Scene Error: Not Found Target Scene: " + targetSceneID.ToString());
            return false;
        }
    }


    #region 异步加载
    public enum SceneLoadStatus
    {
        Wait,
        Running,
        Success,
        Failure
    }

    /// <summary>
    /// 记录当前异步加载状态,AsyncLoad Status
    /// </summary>
    public SceneLoadStatus curLoadStatus;

    /// <summary>
    /// 异步加载,协程状态不为Wait或者无法找到对应场景则return false,正常使用会开一个场景加载协程
    /// </summary>
    /// <param name="targetSceneID"></param>
    /// <returns></returns>
    public bool TryLoadToTargetSceneAsync(SceneDisplayID targetSceneID)
    {
        
        string scenePath = "";
        foreach (var i in _sceneFile_DisplayIDAndPath)
        {
            if (i.Key == targetSceneID)
            {
                scenePath = i.Value;
                break;
            }
        }
        //未找到场景
        if(scenePath=="")
        {
            Debug.Log("Load Scene Async Failure: Not Found Target Scene: " + targetSceneID.ToString());
            return false;
        }
        //协程空置等待中
        if (curLoadStatus==SceneLoadStatus.Wait)
        {
            if (_loadSceneAsync_ie != null)
                StopCoroutine(_loadSceneAsync_ie);
            _loadSceneAsync_ie = LoadToTargetSceneAsync(scenePath);
            StartCoroutine(_loadSceneAsync_ie); 
            return true;
        }
        else if (curLoadStatus == SceneLoadStatus.Running)
        { 
            //
            Debug.Log("One LoadProgress is Running.");
            return false;
        }
        else if(curLoadStatus==SceneLoadStatus.Success)
        { 
            Debug.Log("Last LoadProgress success,But not Reset LoadStatus To [Wait].");
            return false;
        }
        else
        {
            Debug.Log("Last LoadProgress fails,But not Reset LoadStatus To [Wait].");
            return false; 
        }    
    }

    private IEnumerator _loadSceneAsync_ie;
    private AsyncOperation _loadAO;
    private bool _bLoadingAORunning; 
    //异步加载协程处理
    private IEnumerator LoadToTargetSceneAsync(string sPath)
    {
        if (sPath != "")
        {
            //TODO: 加载过渡界面

            //黑屏过渡
            if (UIManager.Instance.sceneFader != null)
                UIManager.Instance.sceneFader.FadeIn(UIManager.Instance.sceneFader.currentFadeType);
            else
                Debug.Log("UIManager.Instance.sceneFader is NULL!");

            //调用GC
            System.GC.Collect();

            onSceneLoadBegin?.Invoke();

            curLoadStatus = SceneLoadStatus.Running;
            Debug.Log("sPath" + sPath);
            _loadAO = SceneManager.LoadSceneAsync(SceneUtility.GetBuildIndexByScenePath(sPath));
            _bLoadingAORunning = true;
            //等待异步加载完成
            yield return _loadAO;
            _bLoadingAORunning = false;
            _loadAO = null;
            curLoadStatus = SceneLoadStatus.Success;
             
            onSceneLoadEnd?.Invoke();

            Debug.Log("Load Scene Async Success");
        }
        else
        {
            curLoadStatus = SceneLoadStatus.Failure;
            Debug.Log("Load Scene Async Failure: Not Found Target Scene: ");
        }
    }

    public float GetLoadingProgressValue()
    {
        if (_bLoadingAORunning)
        {
            return _loadAO.progress;
        }
        else if (curLoadStatus == SceneLoadStatus.Success)
            return 1;
        else
            return 0;
    }

    /// <summary>
    /// 使用异步加载完成后，重置加载状态以下次使用
    /// When use LoadAsync,Do Reset For next Use,if one progress is running,it might go wrong
    /// </summary>
    public void ResetSceneLoadStatus()
    {
        if (curLoadStatus == SceneLoadStatus.Running)
            Debug.Log("One Progress is running,Reset might error."); 
        curLoadStatus = SceneLoadStatus.Wait; 
    }

    public UnityEvent onSceneLoadBegin; 
    public UnityEvent onSceneLoadEnd;
     

    #endregion
}
