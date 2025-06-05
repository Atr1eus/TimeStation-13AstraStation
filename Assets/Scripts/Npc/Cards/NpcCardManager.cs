using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class NpcCardManager : MonoBehaviour
{
    public GameObject cardPrefab;
    public Transform cardContainer;
    public List<NpcCardUI> activeCards = new List<NpcCardUI>();
    public List<NpcData> remainingNpcs = new List<NpcData>();
    [SerializeField] private TradingSceneManager scene;

    public void InitializeCards(List<NpcData> npcs)
    {
        ClearCardList();

        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        float leftBorder = screenWidth / 10f;
        float rightBorder = screenWidth * 9f / 10f;
        float yPos = screenHeight / 2f;

        float totalSpace = rightBorder - leftBorder;
        float spacing = totalSpace / (npcs.Count + 1);

        for (int i = 0; i < npcs.Count; i++)
        {
            float xPos = leftBorder + (i + 1) * spacing;
            Vector3 screenPos = new Vector3(xPos, yPos, 10f);
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);

            GameObject cardObj = Instantiate(cardPrefab, worldPos, Quaternion.identity, cardContainer);
            cardObj.transform.localScale = Vector3.zero; // 初始大小为0

            NpcCardUI cardUI = cardObj.GetComponent<NpcCardUI>();
            NpcCard cardData = new NpcCard(npcs[i]);
            cardUI.SetUp(cardData);
            activeCards.Add(cardUI);

            // 启动缩放动画协程
            StartCoroutine(ScaleCardAnimation(cardObj.transform, 0.2f, new Vector3(1, 1, 1)));
        }
    }
    private IEnumerator ScaleCardAnimation(Transform cardTransform, float duration, Vector3 targetScale)
    {
        float elapsedTime = 0f;
        Vector3 initialScale = Vector3.zero;

        while (elapsedTime < duration)
        {
            cardTransform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cardTransform.localScale = targetScale; // 确保最终大小准确
    }
    public void InitializeRemainingNpcs(List<NpcData> npcs)
    {
        remainingNpcs = npcs;
    }
    public void SelectCard(NpcCard selectedCard) //选择卡牌事件
    {
        Debug.Log("SelectCard");
        NpcCardUI selectedUI = activeCards.Find(c => c.npcCard == selectedCard);
        activeCards.Remove(selectedUI);
        foreach (var card in activeCards)
        {
            card.GetComponent<RectTransform>().DOAnchorPosY(-600f, 0.5f).OnComplete(() => Destroy(card.gameObject)); // 非选择Npc卡牌向下移动，动画完成后销毁
        }
        activeCards.Clear();
        remainingNpcs.Remove(selectedCard.npc);
        RoundManager.Instance.currentCanSelectNpcNum--;
        RoundManager.Instance.AddCurrentRoundSelectedNpc(selectedUI.npcCard.npc);
        Destroy(selectedUI.gameObject);
        scene.OnOpenApplicationButtonClick();
        scene.OnOpenTicketButtonClick();
        DialogueManager.Instance.StartDialogue();
        DialogueManager.Instance.HandleSpaceKeyPress();
        scene.DecidedNpcButtonState();
        Debug.Log($"已选择 NPC: {selectedCard.npc.npcName}");
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