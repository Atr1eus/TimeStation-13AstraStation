using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DialogueSystem
{
    [Serializable]
    public class DialogueCallbackActions
    {
        /* Actions */
        public Action<char> OnCharacterAppear;

        public Action<TextNode> OnTextNodeEnter;
        public Action<TextNode> OnTextNodeStart;
        public Action<TextNode> OnTextNodeEnd;
        public Action<TextNode> OnTextNodeLeave;

        public Action<EventNode> OnEventNodeEnter;
        public Action<EventNode> OnEventNodeLeave;

        public Action<BranchNode> OnBranchNodeEnter;
        public Action<BranchNode> OnBranchNodeLeave;

        public Action<DialogueGraph> OnDialogueGraphEnter;
        public Action<DialogueGraph> OnDialogueGraphEnd;

        public Action<BaseNode> OnNodeEnter;
        public Action<BaseNode> OnNodeLeave;


        /*Events*/
        public UnityEvent<TextNode> OnTextNodeEndEvents;
        public UnityEvent<BranchNode> OnBranchNodeEndEvents;
        public UnityEvent<DialogueGraph> OnDialogueGraphEndEvents;
        public UnityEvent<EventNode> OnEventNodeEndEvents;

        /* Event Handler*/
        public Action<DialogueEvent> EventHandler;
    }
}
