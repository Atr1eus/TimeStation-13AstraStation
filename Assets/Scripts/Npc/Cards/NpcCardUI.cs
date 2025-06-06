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
    //public GIFPlayer player;
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
        //player.StartGIFAndBlock();
        if (npcCard.npc.type == NpcType.Normal) NpcManager.Instance.InitializeCurrentNpc(npcCard);
        else NpcManager.Instance.InitializeCurrentStoryNpc(npcCard);
        npcCardManager.SelectCard(npcCard);
        
    }
}