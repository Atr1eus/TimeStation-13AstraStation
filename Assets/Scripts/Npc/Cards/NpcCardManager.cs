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
    public GIFPlayer player;
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
            cardObj.transform.localScale = Vector3.zero; // ��ʼ��СΪ0

            NpcCardUI cardUI = cardObj.GetComponent<NpcCardUI>();
            NpcCard cardData = new NpcCard(npcs[i]);
            cardUI.SetUp(cardData);
            activeCards.Add(cardUI);

            // �������Ŷ���Э��
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

        cardTransform.localScale = targetScale; // ȷ�����մ�С׼ȷ
    }
    public void InitializeRemainingNpcs(List<NpcData> npcs)
    {
        remainingNpcs = npcs;
    }
    public void SelectCard(NpcCard selectedCard) //ѡ�����¼�
    {
        foreach (var card in activeCards)
        {
            card.GetComponent<RectTransform>().DOAnchorPosY(-600f, 0.5f).OnComplete(() => Destroy(card.gameObject)); // ��ѡ��Npc���������ƶ���������ɺ�����
        }
        //yield return new WaitForSeconds(0.5f); // �ȴ����ƶ�����ɣ�0.5�룩

        // 2. ���� GIF �������ȴ����
        player.StartGIFAndBlock();

        StartCoroutine(ExecuteAfterGIF(selectedCard));
    }

    private IEnumerator ExecuteAfterGIF(NpcCard selectedCard)
    {
        // �ȴ� GIF �������
        while (player.isPlaying)
        {
            yield return null;
        }

        // GIF ������ɺ�ִ�к����߼�
        Debug.Log("SelectCard");
        NpcCardUI selectedUI = activeCards.Find(c => c.npcCard == selectedCard);
        if (selectedUI != null)
        {
            activeCards.Remove(selectedUI);
            remainingNpcs.Remove(selectedCard.npc);
            RoundManager.Instance.currentCanSelectNpcNum--;
            RoundManager.Instance.AddCurrentRoundSelectedNpc(selectedUI.npcCard.npc);
            Destroy(selectedUI.gameObject);
        }
        activeCards.Clear();
        DialogueManager.Instance.StartDialogue();
        DialogueManager.Instance.HandleSpaceKeyPress();
        scene.DecidedNpcButtonState();

        Debug.Log($"��ѡ�� NPC: {selectedCard.npc.npcName}");
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