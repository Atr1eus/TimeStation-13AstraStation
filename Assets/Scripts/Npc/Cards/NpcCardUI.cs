using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NpcCardUI : MonoBehaviour
{
    public Text description;
    public Button chooseButton;
    public Image npcImage;
    public NpcCard npcCard;
    NpcCardManager npcCardManager;
    public void OnEnable()
    {
        npcCardManager = FindObjectOfType<NpcCardManager>();
    }
    public void SetUp(NpcCard card)
    {
        npcCard = card;
        description.text = card.description;
        chooseButton.onClick.AddListener(OnChooseButtonClick);
    }
    private void OnChooseButtonClick()
    {
        Debug.Log("按下选择角色！");
        NpcManager.Instance.InitializeCurrentNpc(npcCard);
        npcCardManager.SelectCard(npcCard);
        
    }
}