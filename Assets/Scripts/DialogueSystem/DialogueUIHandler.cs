using DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUIHandler : MonoBehaviour
{
    public Transform ChoicePanel;
    public GameObject Pane;
    public GameObject Text;
    public Button choiceButtonPrefab;
    public Transform NamePanel;

    private List<Button> activeChoiceButtons = new List<Button>();

    void Start()
    {
        ChoicePanel.gameObject.SetActive(false);
        
    }

    public void RenderDialogueChoices(DialogueChoices dialogueChoices, Dialogue dialogueSystem)
    {
        //DialogueWindow.SetActive(false);
        ChoicePanel.gameObject.SetActive(true);
        foreach (DialogueChoice choice in dialogueChoices.Choices)
        {
            Button choiceButton = Instantiate(choiceButtonPrefab, ChoicePanel);
            TMP_Text btnText = choiceButton.transform.GetChild(0).GetComponent<TMP_Text>();
            btnText.text = choice.text;
            choiceButton.onClick.AddListener(() => {
                ClearAllOptions();
                dialogueSystem.SelectDialogueChoice(choice.choiceNumber);
                });
            activeChoiceButtons.Add(choiceButton);
        }
    }

    public void ClearAllOptions()
    {
        // 隐藏选择面板
        ChoicePanel.gameObject.SetActive(false);

        // 销毁所有按钮实例
        foreach (Button button in activeChoiceButtons)
        {
            if (button != null)
            {
                Destroy(button.gameObject);
            }
        }

        // 清空列表
        activeChoiceButtons.Clear();
    }

    public void SetSpeakerName(string name)
    {
        NamePanel.gameObject.SetActive(true);
        TMP_Text text = NamePanel.GetChild(0).GetComponent<TMP_Text>();
        text.text = name;
    }

    public void DisableNamePlate()
    {
        NamePanel.gameObject.SetActive(false);
    }
}
