using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DialogueSystem;

public class SampleUIHandler : MonoBehaviour
{

    public Transform ChoicePanel;
    public GameObject DialogueWindow;
    public Button choiceButtonPrefab;
    public Transform NamePanel;

    void Start()
    {
        ChoicePanel.gameObject.SetActive(false);

    }

    public void RenderDialogueChoices(DialogueChoices dialogueChoices, Dialogue dialogueSystem)
    {
        DialogueWindow.SetActive(false);
        ChoicePanel.gameObject.SetActive(true);
        foreach (DialogueChoice choice in dialogueChoices.Choices)
        {
            Button choiceButton = Instantiate(choiceButtonPrefab, ChoicePanel);
            TMP_Text btnText = choiceButton.transform.GetChild(0).GetComponent<TMP_Text>();
            btnText.text = choice.text;
            choiceButton.onClick.AddListener(() => dialogueSystem.SelectDialogueChoice(choice.choiceNumber));
            
        }
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
