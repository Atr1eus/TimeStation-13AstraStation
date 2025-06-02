using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryBrowser : MonoBehaviour
{
    [Header("UI References")]
    public GameObject inventoryPanel;        // 主面板
    public GameObject itemPrefab;            // 单个物品项的预制体
    public Transform scrollContent;          // ScrollView的内容区域
    public ScrollRect scrollView;            // 滚动视图

    public InventorySystem m_inventory;

    // Start is called before the first frame update
    void Start()
    {
        if (inventoryPanel != null) inventoryPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 库存浏览按钮点击处理
    public void OnInventoryBrowseButtonClick()
    {
        // 如果面板未激活，则打开并填充内容
        if (!inventoryPanel.activeSelf)
        {
            OpenInventory();
        }
        else // 如果已经打开，则关闭
        {
            CloseInventory();
        }
    }

    // 打开库存浏览界面
    private void OpenInventory()
    {
        inventoryPanel.SetActive(true);

        // 清除现有内容
        ClearInventoryView();

        // 填充库存内容
        PopulateInventoryView();
    }

    // 关闭库存浏览界面
    private void CloseInventory()
    {
        inventoryPanel.SetActive(false);
    }

    // 清除库存视图内容
    private void ClearInventoryView()
    {
        foreach (Transform child in scrollContent)
        {
            Destroy(child.gameObject);
        }
    }

    // 填充库存视图内容
    private void PopulateInventoryView()
    {
        // 创建标题行
        CreateHeaderRow();

        // 遍历库存物品并创建行
        foreach (Item item in m_inventory.items)
        {
            CreateItemRow(item);
        }
    }

    // 创建标题行
    private void CreateHeaderRow()
    {
        GameObject header = Instantiate(itemPrefab, scrollContent);
        InventoryItemUI ui = header.GetComponent<InventoryItemUI>();

        // 设置标题文本（可以添加颜色或样式）
        ui.SetData(
            "<color=#FFD800>名称</color>",
            "<color=#FFD800>简介</color>",
            "<color=#FFD800>数量</color>");

        // 添加分隔线
        ui.SetAsHeader();
    }

    // 创建物品行
    private void CreateItemRow(Item item)
    {
        GameObject itemObject = Instantiate(itemPrefab, scrollContent);
        InventoryItemUI ui = itemObject.GetComponent<InventoryItemUI>();

        // 设置物品数据
        ui.SetData(
        item.data.itemName,
            item.data.itemDescription,
            item.amount.ToString());
    }
}
