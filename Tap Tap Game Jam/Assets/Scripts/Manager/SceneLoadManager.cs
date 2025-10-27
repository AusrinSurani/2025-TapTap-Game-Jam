using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement; 

public class SceneLoadManager : Singleton<SceneLoadManager>
{
    [Header("事件广播")]
    public VoidEventSO changeChapterEvent;
    
    public SceneDisplayID currentScene = SceneDisplayID.StartMenu;
     


    protected override void Awake()
    {
        base.Awake();
        //将List数据存入dictionary中
        ImportSceneDataFromListToDictionary();
        
    }

    #region ScenePath输入

    //与scenesPath保持对应
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
#if UNITY_EDITOR
                Debug.LogError("Fail to Import ScenePath Data: Repeat Key [" + scenesDisplayID[i].ToString() + "] at index " + i + " .");
#endif
                return false;
            }
        } 
        return true;
    }

    #endregion

    [SerializeField]private Dictionary<SceneDisplayID, string> _sceneFile_DisplayIDAndPath = new Dictionary<SceneDisplayID, string>();
    public enum SceneDisplayID
    {
        StartMenu,
        ConsultationRoom,
        DressingRoom,
        DanceDream,
        WaiterDream,
        ComputerRoom,
        CoderDream
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
            
            if (changeChapterEvent != null)
            {
                changeChapterEvent.RaiseEvent();
            }

            return true;
        }
        else
        {

#if UNITY_EDITOR
            Debug.Log("Load Scene Error: Not Found Target Scene: " + targetSceneID.ToString());
#endif
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
    /// <param name="words"></param>
    /// <param name="needWords"></param>
    /// <returns></returns>
    public bool TryLoadToTargetSceneAsync(SceneDisplayID targetSceneID, string words, bool needWords)
    {
        //检测是否有对话框并关闭之
        if (DialogManager.Instance.IsDialogOpen())
        {
            DialogManager.Instance.dialogBox.GetComponent<UI_Dialog>().MoveBack();
        }
        
        currentScene =  targetSceneID;
        
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
#if UNITY_EDITOR
            Debug.Log("Load Scene Async Failure: Not Found Target Scene: " + targetSceneID.ToString());
#endif
            return false;
        }
        //协程空置等待中
        if (curLoadStatus==SceneLoadStatus.Wait)
        {
            if (UIManager.Instance.coverFader.BFading)
            {
#if UNITY_EDITOR
                Debug.Log("Cover Fading,one progress is running");
#endif
                return false;
            }

            if (_loadSceneAsync_ie != null)
                StopCoroutine(_loadSceneAsync_ie);
            _loadSceneAsync_ie = LoadToTargetSceneAsync(scenePath, words,  needWords);
            StartCoroutine(_loadSceneAsync_ie); 
            
            return true;
        }
        else if (curLoadStatus == SceneLoadStatus.Running)
        {

#if UNITY_EDITOR
            Debug.Log("One LoadProgress is Running.");
#endif
            return false;
        }
        else if(curLoadStatus==SceneLoadStatus.Success)
        {
            if (_loadSceneAsync_ie != null)
                StopCoroutine(_loadSceneAsync_ie);
            _loadSceneAsync_ie = LoadToTargetSceneAsync(scenePath, words, needWords);

#if UNITY_EDITOR
            Debug.Log("Last LoadProgress success,Not Reset but current order still Load.");
#endif
            StartCoroutine(_loadSceneAsync_ie);

            return true;
        }
        else
        {
#if UNITY_EDITOR
            Debug.Log("Last LoadProgress fails,But not Reset LoadStatus To [Wait].");
#endif
            return false; 
        }    
        
        
    }

    public bool TryLoadToTargetSceneAsync(SceneDisplayID targetSceneID, List<string> paras, bool needWords,bool bIsParas)
    {
        //检测是否有对话框并关闭之
        if (DialogManager.Instance.IsDialogOpen())
        {
            DialogManager.Instance.dialogBox.GetComponent<UI_Dialog>().MoveBack();
        }

        currentScene = targetSceneID;

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
        if (scenePath == "")
        {
#if UNITY_EDITOR
            Debug.Log("Load Scene Async Failure: Not Found Target Scene: " + targetSceneID.ToString());
#endif
            return false;
        }
        //协程空置等待中
        if (curLoadStatus == SceneLoadStatus.Wait)
        {
            if (UIManager.Instance.coverFader.BFading)
            {
#if UNITY_EDITOR
                Debug.Log("Cover Fading,one progress is running");
#endif
                return false;
            }

            if (_loadSceneAsync_ie != null)
                StopCoroutine(_loadSceneAsync_ie);
            _loadSceneAsync_ie = LoadToTargetSceneAsync(scenePath, paras, needWords);
            StartCoroutine(_loadSceneAsync_ie);

            return true;
        }
        else if (curLoadStatus == SceneLoadStatus.Running)
        {
            //

#if UNITY_EDITOR
            Debug.Log("One LoadProgress is Running.");
#endif
            return false;
        }
        else if (curLoadStatus == SceneLoadStatus.Success)
        {
            if (_loadSceneAsync_ie != null)
                StopCoroutine(_loadSceneAsync_ie);
            _loadSceneAsync_ie = LoadToTargetSceneAsync(scenePath, paras, needWords);
#if UNITY_EDITOR
            Debug.Log("Last LoadProgress success,Not Reset but current order still Load.");
#endif
            StartCoroutine(_loadSceneAsync_ie);

            return true;
        }
        else
        {
#if UNITY_EDITOR
            Debug.Log("Last LoadProgress fails,But not Reset LoadStatus To [Wait].");
#endif
            return false;
        }


    }


    private IEnumerator _loadSceneAsync_ie;
    private AsyncOperation _loadAO;
    private bool _bLoadingAORunning;
     
    //异步加载协程处理
    private IEnumerator LoadToTargetSceneAsync(string sPath, string words, bool needWords)
    {
        if (sPath != "")
        {

            //黑屏过渡
            if (UIManager.Instance.coverFader != null)
            {
                if (UIManager.Instance.coverFader.BFading)
                {
                    //已经在过渡中
                }
                else
                {
                    yield return UIManager.Instance.coverFader.FadeIn();
                    if (needWords)
                    {
                        yield return UIManager.Instance.coverFader.TextType(words);
                    }
                } 
                //UIManager.Instance.sceneFader.FadeIn(UIManager.Instance.sceneFader.currentFadeType);
            }
#if UNITY_EDITOR
            else
                Debug.Log("UIManager.Instance.sceneFader is NULL!");
#endif

            onSceneLoadBegin?.Invoke();
            //调用GC
            System.GC.Collect();


            curLoadStatus = SceneLoadStatus.Running;
            //Debug.Log("sPath:" + sPath);
            _loadAO = SceneManager.LoadSceneAsync(SceneUtility.GetBuildIndexByScenePath(sPath));
            _bLoadingAORunning = true;
            //等待异步加载完成
            yield return _loadAO;
            _bLoadingAORunning = false;
            _loadAO = null;
            curLoadStatus = SceneLoadStatus.Success;
            //黑屏过渡
            if (UIManager.Instance.coverFader != null)
            {
                yield return UIManager.Instance.coverFader.FadeOut();
                 
                //as:加载完把原来残留的Text删掉
                UIManager.Instance.coverFader.DisplayTextPro.text = String.Empty;
                //UIManager.Instance.sceneFader.FadeIn(UIManager.Instance.sceneFader.currentFadeType);
            }
#if UNITY_EDITOR
            else
                Debug.Log("UIManager.Instance.coverFader is NULL!");
#endif
            //恢复交互
            EventSystem.current.enabled = true;
            onSceneLoadEnd?.Invoke();
            
            if (changeChapterEvent != null)
            {
                changeChapterEvent.RaiseEvent();
            }
#if UNITY_EDITOR
            Debug.Log("Load Scene Async Success");
#endif
        }
        else
        {
            curLoadStatus = SceneLoadStatus.Failure;
 
#if UNITY_EDITOR
            Debug.Log("Load Scene Async Failure: Not Found Target Scene: ");
#endif
        }
    }

    private IEnumerator LoadToTargetSceneAsync(string sPath, List<string> allPages, bool needWords)
    {
        if (sPath != "")
        {

            //黑屏过渡
            if (UIManager.Instance.coverFader != null)
            {
                if (UIManager.Instance.coverFader.BFading)
                {
                    //已经在过渡中
                }
                else
                {
                    yield return UIManager.Instance.coverFader.FadeIn();
                    if (needWords)
                    {
                        yield return UIManager.Instance.coverFader.TextTypeByParagraph(allPages);
                    }
                } 
                //UIManager.Instance.sceneFader.FadeIn(UIManager.Instance.sceneFader.currentFadeType);
            }
#if UNITY_EDITOR
            else
                Debug.Log("UIManager.Instance.sceneFader is NULL!");
#endif

            onSceneLoadBegin?.Invoke();
            //调用GC
            System.GC.Collect();


            curLoadStatus = SceneLoadStatus.Running;
            //Debug.Log("sPath:" + sPath);
            _loadAO = SceneManager.LoadSceneAsync(SceneUtility.GetBuildIndexByScenePath(sPath));
            _bLoadingAORunning = true;
            //等待异步加载完成
            yield return _loadAO;
            _bLoadingAORunning = false;
            _loadAO = null;
            curLoadStatus = SceneLoadStatus.Success;
            //黑屏过渡
            if (UIManager.Instance.coverFader != null)
            {
                yield return UIManager.Instance.coverFader.FadeOut();

                //as:加载完把原来残留的Text删掉
                UIManager.Instance.coverFader.DisplayTextPro.text = String.Empty;
                //UIManager.Instance.sceneFader.FadeIn(UIManager.Instance.sceneFader.currentFadeType);
            }

#if UNITY_EDITOR
            else
                Debug.Log("UIManager.Instance.coverFader is NULL!");
#endif
            //恢复交互
            EventSystem.current.enabled = true;
            onSceneLoadEnd?.Invoke();

            if (changeChapterEvent != null)
            {
                changeChapterEvent.RaiseEvent();
            }

#if UNITY_EDITOR
            Debug.Log("Load Scene Async Success");
#endif
        }
        else
        {
            curLoadStatus = SceneLoadStatus.Failure;

#if UNITY_EDITOR
            Debug.Log("Load Scene Async Failure: Not Found Target Scene: ");
#endif
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
#if UNITY_EDITOR
        if (curLoadStatus == SceneLoadStatus.Running)
            Debug.Log("One Progress is running,Reset might error.");
#endif
        curLoadStatus = SceneLoadStatus.Wait; 
    }

    public UnityEvent onSceneLoadBegin; 
    public UnityEvent onSceneLoadEnd;

    #endregion

    public bool bGameEnd_FindTruth;
}
