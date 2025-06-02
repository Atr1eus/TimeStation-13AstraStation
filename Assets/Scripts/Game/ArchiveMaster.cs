using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using UnityEngine;

public static class ArchiveMaster
{
    private static string SavePath => Path.Combine(Application.persistentDataPath, "global_save.json");

    // 自动收集所有标记了[Serializable]的实例
    public static void SaveAll()
    {
        var archive = new GameArchive();

        // 通过反射找到所有需要存档的实例
        var savableTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => t.IsDefined(typeof(SerializableAttribute), false));

        foreach (var type in savableTypes)
        {
            // 获取单例实例（根据你的架构调整）
            var instance = FindInstance(type);
            if (instance != null)
            {
                archive.AddData(instance);
            }
        }

        string json = JsonConvert.SerializeObject(archive, Formatting.Indented,
            new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });

        File.WriteAllText(SavePath, json);
        Debug.Log("全局存档完成");
    }

    // 加载所有数据
    public static void LoadAll()
    {
        if (!File.Exists(SavePath)) return;

        string json = File.ReadAllText(SavePath);
        var archive = JsonConvert.DeserializeObject<GameArchive>(json,
            new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });

        foreach (var kvp in archive.serializedData)
        {
            Type type = Type.GetType(kvp.Key);
            var instance = FindInstance(type);
            if (instance != null)
            {
                JsonConvert.PopulateObject(JsonConvert.SerializeObject(kvp.Value), instance);
            }
        }
        Debug.Log("全局读档完成");
    }

    // 查找实例的辅助方法（需根据项目结构调整）
    private static object FindInstance(Type type)
    {
        // 如果是单例
        if (typeof(SingletonMonoBehaviour<>).IsAssignableFromGenericType(type))
        {
            var instanceProp = type.GetProperty("Instance");
            return instanceProp?.GetValue(null);
        }
        // 其他情况...
        return null;
    }
}