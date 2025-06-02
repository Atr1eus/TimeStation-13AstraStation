using Newtonsoft.Json;
using System.IO;
using UnityEngine;
using System;
using System.Collections.Generic;
[Serializable]
public class GameArchive
{
    // 用字典存储所有可序列化对象（Key=类型名, Value=对象实例）
    public Dictionary<string, object> serializedData = new Dictionary<string, object>();

    // 添加需要存档的实例
    public void AddData<T>(T data) where T : class
    {
        string key = typeof(T).FullName;
        serializedData[key] = data;
    }

    // 获取存档的实例
    public T GetData<T>() where T : class
    {
        string key = typeof(T).FullName;
        return serializedData.ContainsKey(key) ? (T)serializedData[key] : null;
    }
}