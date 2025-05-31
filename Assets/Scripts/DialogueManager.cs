using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using System.IO;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text DialogueText;
    public float typingSpeed = 0.05f;

    private List<DialogueLine> currentLines = new List<DialogueLine>();
    private int currentIndex = 0;

    [System.Serializable]
    private class DialogueLine
    {
        public string characterName;
        public string content;
    }

    void Start()
    {
        SetDialogueText("欢迎来到游戏世界!");
    }

    public void SetDialogueText(string content)
    {
        if (DialogueText != null)
        {
            // 直接设置文本内容
            DialogueText.text = content;

            // 或者使用富文本
            // dialogueText.text = "<b>重要提示</b>: " + content;
        }
    }

    public void LoadCSVDialogues(string characterName)
    {
        TextAsset csvData = Resources.Load<TextAsset>("Dialogues");
        string[] lines = csvData.text.Split('\n');

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            string[] values = line.Split(',');
            if (values.Length >= 2 && values[0] == characterName)
            {
                currentLines.Add(new DialogueLine
                {
                    characterName = values[0],
                    content = values[1]
                });
            }
        }
    }

    public void ShowNextLine()
    {
        if (currentIndex >= currentLines.Count) return;

        StopAllCoroutines();
        StartCoroutine(TypeText(currentLines[currentIndex].content));
        currentIndex++;
    }

    private IEnumerator TypeText(string text)
    {
        DialogueText.text = "";
        foreach (char c in text.ToCharArray())
        {
            DialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
