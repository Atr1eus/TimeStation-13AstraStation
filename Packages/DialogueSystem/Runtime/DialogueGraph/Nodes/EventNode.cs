using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    public class EventNode : BaseNode
    {
        public override string NodeType => "EventNode";

        [Input] public float input;
        [Output] public float output;

        public List<DialogueEvent> events = new List<DialogueEvent>();

        public bool GoToNextNodeAutomatically = true;

        protected override void Init()
        {
            nodeName = "Event node";
            base.Init();
        }

    }
}
