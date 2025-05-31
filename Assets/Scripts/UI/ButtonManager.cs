using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        
    }

    // 在Inspector中绑定对应按钮
    public Button silenceButton;
    public Button satisfyButton;
    public Button extraButton;
    public Button nextDayButton;

    void Start()
    {
        // 预留接口
        silenceButton.onClick.AddListener(() => OnSilenceClicked());
        satisfyButton.onClick.AddListener(() => OnSatisfyClicked());
        extraButton.onClick.AddListener(() => OnExtraClicked());
        nextDayButton.onClick.AddListener(() => OnNextDayClicked());
    }

    // 后续可扩展的接口方法
    public void OnSilenceClicked() { /* 后续添加逻辑 */ }
    public void OnSatisfyClicked() { /* 后续添加逻辑 */ }
    public void OnExtraClicked() { /* 后续添加逻辑 */ }
    public void OnNextDayClicked() { /* 后续添加逻辑 */ }
}
