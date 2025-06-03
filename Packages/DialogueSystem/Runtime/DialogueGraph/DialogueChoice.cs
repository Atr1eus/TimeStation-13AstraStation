using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;


namespace DialogueSystem
{
    [Serializable]
    public class DialogueChoice
    {
        [SerializeField]
        public string choiceName;
        [SerializeField] public string text;

        [HideInInspector]
        public int choiceNumber;


        public DialogueChoice(string text, string name)
        {
            this.text = text;
            this.choiceName = name;
        }

    }

    public class DialogueChoices
    {
        public List<DialogueChoice> Choices { get; private set; }
        public string TextPrompt { get; private set; }
        public ChoiceNode _node;


        public DialogueChoices(List<DialogueChoice> choices, string textPrompt = null, ChoiceNode node = null)
        {
            Choices = choices;
            TextPrompt = textPrompt;
            _node = node;
        }

    }
}
