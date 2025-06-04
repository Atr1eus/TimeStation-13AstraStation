using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemUI : MonoBehaviour
{
    public static InventoryItemUI Instance { get; set; }

    public GameObject panel;          // 面板 GameObject
    public Transform content;         // ScrollView 的内容区域
    public GameObject itemUIPrefab;   // ItemUI 预制体
    public Button closeButton;         // 打开面板的按钮

    [SerializeField] private TradingSceneManager tradingSceneManager;

    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
        panel.SetActive(false);
    }
    void Start()
    {
        //ClosePanel();
        closeButton.onClick.AddListener(ClosePanel);
    }

    public void OpenPanel()
    {
        if (panel != null)
        {
            Debug.LogWarning("无panel！");
        }
        panel.SetActive(true);
        RefreshUI();
    }

    public void ClosePanel()
    {
        panel.SetActive(false);
        tradingSceneManager.AgreeButtonState();
    }

    public void RefreshUI()
    {
        // 清空现有内容
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        // 动态生成物品 UI
        foreach (var item in InventorySystem.Instance.items)
        {
            GameObject itemUIObj = Instantiate(itemUIPrefab, content);
            ItemUI itemUI = itemUIObj.GetComponent<ItemUI>();
            itemUI.SetData(item);
        }

    }
}
