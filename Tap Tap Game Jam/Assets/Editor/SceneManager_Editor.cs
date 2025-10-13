using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static SceneLoadManager;


public class SceneManager_Editor : Editor
{
    //运行Play时导入,暂时没啥用，留后续数据存储到本地作拖拽
    [MenuItem("SceneLoadManager/scenePathToDictionary")]
    public static void ImportDataToSceneManager()
    {
        if (SceneLoadManager.Instance != null)
        {

        } 
        else
            Debug.Log("Not Found SceneLoadManager.Instance");
    }
     
}
