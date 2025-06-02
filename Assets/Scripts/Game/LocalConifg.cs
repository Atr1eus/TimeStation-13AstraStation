using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
public class LocalConifg : MonoBehaviour
{
    public static void SaveUserData(UserData userData)
    {
        //创建users文件夹
        if(!File.Exists(Application.persistentDataPath + "/users"))
        {
            System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/users");
        }
        //转换用户数据为Json字符串
        string jsonData = JsonConvert.SerializeObject(userData);
        //将字符串写入文件
        File.WriteAllText(Application.persistentDataPath + string.Format("/users/{0}.json",userData.name),jsonData);
    }
    public static UserData LoadUserData(string userName)
    {
        string path = Application.persistentDataPath + string.Format("/users/{0}.json", userName);
        if(File.Exists(path))
        {
            //加载Json字符串
            string jsonData = File.ReadAllText(path);
            //将Json字符串转为玩家数据
            UserData userData = JsonConvert.DeserializeObject<UserData>(jsonData);
            return userData;
        }
        return null;
    }
}
public class UserData
{
    public string name;
    public int gold;    
}
