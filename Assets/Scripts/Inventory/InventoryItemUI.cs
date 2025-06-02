using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Header("UI Components")]
    public Text nameText;
    public Text descriptionText;
    public Text amountText;
    public Image background;
    public Image divider;

    public void SetData(string name, string description, string amount)
    {
        nameText.text = name;
        descriptionText.text = description;
        amountText.text = amount;
    }

    public void SetAsHeader()
    {
        // 设置背景和分隔线的样式
        background.color = new Color(0.2f, 0.2f, 0.2f, 1f);
        divider.color = new Color(0.8f, 0.8f, 0.8f, 1f);
    }
}
