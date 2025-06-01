using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DialogueSystem;

public class DialogueEventList : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EventHandler(DialogueEvent myEvent)
    {
        if (myEvent == null) return;

        // 在此处添加您的各种事件处理逻辑
        switch (myEvent.eventName)
        {
            case "SSVGG":
                SSVGGEvent();
                break;

            case "YourCustomEvent": // 您的自定义事件
                
                break;

            default:
                Debug.LogWarning($"Unknown event: {myEvent.eventName}");
                break;
        }
    }

    public void SSVGGEvent()
    {
        Debug.Log("SSVGG");
    }
}
