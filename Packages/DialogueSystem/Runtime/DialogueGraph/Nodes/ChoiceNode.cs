using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace DialogueSystem
{
    public class ChoiceNode : BaseNode
    {
        public override string NodeType => "ChoiceNode";

        public string textPrompt;

        [Output(dynamicPortList = true)] public List<DialogueChoice> choices = new List<DialogueChoice>();

        [Input] public float input;

        protected override void Init()
        {
            nodeName = "Choice Node";
            base.Init();

        }

        public override string[] GetString()
        {
            List<string> choiceTexts = new List<string>();
            foreach (DialogueChoice choice in choices)
            {
                choiceTexts.Add(choice.text);
            }
            return choiceTexts.ToArray();
        }

        public override void SetText(string[] text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                choices[i].text = text[i];
            }
        }

        public DialogueChoices GetChoices()
        {
            List<DialogueChoice> dialogueChoicesList = choices;
            for (int i = 0; i < dialogueChoicesList.Count; ++i)
            {
                dialogueChoicesList[i].choiceNumber = i;
            }
            DialogueChoices dialogueChoicesObject = new DialogueChoices(dialogueChoicesList, textPrompt != null && textPrompt != "" ? textPrompt : null, this);
            return dialogueChoicesObject;
        }
    }
}
