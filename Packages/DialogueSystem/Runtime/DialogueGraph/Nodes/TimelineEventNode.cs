using DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

namespace DialogueSystem
{
    public class TimelineEventNode : BaseNode
    {
        public override string NodeType => "TimelineEventNode";

        [Output] public float output;
        [Input] public float input;

        public enum TimelineEvent
        {
            Resume
        }

        public TimelineEvent timelineEvent;

        public bool HideDialoguePanelOnResume = true;
        public bool PauseDialogueOnResume = true;

        protected override void Init()
        {
            nodeName = "TimelineEventNode";
            base.Init();
        }

        public override string GetID()
        {
            return id;
        }
    }
}
