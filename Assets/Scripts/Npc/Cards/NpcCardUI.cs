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

    public void SetUp(NpcCard card)
    {
        npcCard = card;
        description.text = card.description;
        chooseButton.onClick.AddListener(OnChooseButtonClick);
    }
    private void OnChooseButtonClick()
    {
        NpcCardManager.Instance.SelectCard(npcCard);
        NpcManager.Instance.InitializeCurrentNpc(npcCard);
    }
}