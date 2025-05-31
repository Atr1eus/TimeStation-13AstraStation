using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class NpcCardManager : SingletonMonoBehaviour<NpcCardManager>
{
    public GameObject cardPrefab;
    public Transform cardContainer;
    public List<NpcCardUI> activeCards = new List<NpcCardUI>();
    public List<Npc> remainingNpcs = new List<Npc>();

    public void InitializeCards(List<Npc> npcs)
    {
        ClearCardList();
        float screenWidth = Screen.width;
        float leftBorder = screenWidth / 5f;    // 左边界（1/3 处）
        float rightBorder = screenWidth * 4f / 5f; // 右边界（2/3 处）
        float totalSpace = rightBorder - leftBorder;
        float spacing = totalSpace / (npcs.Count + 1); // 卡牌间距
        for (int i = 0; i < npcs.Count; i++)
        {
            GameObject cardObj = Instantiate(cardPrefab, cardContainer);

            float xPos = leftBorder + (i + 1) * spacing;
            float yPos = Screen.height / 2f; // 默认居中

            RectTransform cardRect = cardObj.GetComponent<RectTransform>();
            Vector2 screenPos = new Vector2(xPos, yPos);

            Vector2 anchoredPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                (RectTransform)cardContainer,
                screenPos,
                null,
                out anchoredPos
            );
            cardRect.anchoredPosition = anchoredPos;

            NpcCardUI cardUI = cardObj.GetComponent<NpcCardUI>();
            NpcCard cardData = new NpcCard(npcs[i]);
            cardUI.SetUp(cardData);
            activeCards.Add(cardUI);
        }
    }

    public void SelectCard(NpcCard selectedCard) //选择卡牌事件
    {
        NpcCardUI selectedUI = activeCards.Find(c => c.npcCard == selectedCard);
        activeCards.Remove(selectedUI);
        foreach (var card in activeCards)
        {
            card.GetComponent<RectTransform>().DOAnchorPosY(-600f, 0.5f).OnComplete(() => Destroy(card.gameObject)); // 非选择Npc卡牌向下移动，动画完成后销毁
        }
        activeCards.Clear();
        remainingNpcs.Remove(selectedCard.npc);
        Destroy(selectedUI.gameObject);
        Debug.Log($"已选择 NPC: {selectedCard.npc.name}");
    }
    public void LoadRemainingCards()
    {
        InitializeCards(remainingNpcs);
    }
    public void ClearCardList()
    {
        foreach (var card in activeCards)
        {
            Destroy(card.gameObject);
        }
        activeCards.Clear();
    }

}