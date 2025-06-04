using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public Image icon;             // 图标
    public TMP_Text nameText;          // 名字
    public TMP_Text descriptionText;   // 描述
    public TMP_Text quantityText;      // 数量
    public Button useButton;       // 使用按钮

    private Item m_item;

    public void SetData(Item item)
    {
        m_item = item;

        icon.sprite = item.data.icon;               // 设置图标
        nameText.text = item.data.name;               // 设置名字
        descriptionText.text = item.data.itemDescription; // 设置描述
        quantityText.text = item.amount.ToString(); // 设置数量

        useButton.onClick.AddListener(OnUseButtonClicked); // 绑定使用按钮
    }

    private void OnUseButtonClicked()
    {
        InventorySystem.Instance.GiveNpcRequestItems(m_item.data, NpcManager.Instance.currentNpc.currentRequestAmount); // 给出物品
        InventoryItemUI inventoryUI = FindObjectOfType<InventoryItemUI>();
        inventoryUI.RefreshUI(); // 刷新 UI
    }
}

