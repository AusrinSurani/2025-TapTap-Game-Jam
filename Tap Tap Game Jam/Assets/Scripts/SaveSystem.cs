using System;
using UnityEngine;
using System.IO;

public class SaveSystem
{
    public static void SaveByJson(string saveFileName, object data)
    {
        var jsonData = JsonUtility.ToJson(data);
        var path = Path.Combine(Application.persistentDataPath,saveFileName);
        
        try
        {
            File.WriteAllText(path, jsonData);
        
            #if UNITY_EDITOR
            Debug.Log("Saved " + saveFileName + " To " + path);
            #endif
        }
        catch (SystemException e)
        {
            #if UNITY_EDITOR
            Debug.LogError($"保存失败，尝试保存到：{path}.\n{e}");
            #endif
        }
    }

    public static T LoadFromJson<T>(string saveFileName)
    {   
        var path = Path.Combine(Application.persistentDataPath,saveFileName);

        try
        {
            var jsonData =  File.ReadAllText(path);
            var data = JsonUtility.FromJson<T>(jsonData);
        
            return data;
        }
        catch (SystemException e)
        {
            #if UNITY_EDITOR
            Debug.LogError($"加载数据失败，尝试读取：{path}.\n{e}");
            #endif
            
            return default;
        }
    }

    public static void DeleteSavedFile(string saveFileName)
    {
        var path = Path.Combine(Application.persistentDataPath,saveFileName);

        try
        {
            File.Delete(path);
        }
        catch (SystemException e)
        {
            #if UNITY_EDITOR
            Debug.LogError($"删除存档失败，存档位置：{path}.\n{e}");
            #endif
        }
    }
}