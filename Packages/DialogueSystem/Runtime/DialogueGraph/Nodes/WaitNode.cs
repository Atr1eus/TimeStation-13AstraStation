using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace DialogueSystem
{
    public class WaitNode : BaseNode
    {
        public override string NodeType => "WaitNode";

        [Input] public float input;
        [Output] public float output;


        public float time = 2;
        public bool hideWindow = true;

        protected override void Init()
        {
            nodeName = $"Wait {time}s";
            base.Init();

        }


        public override string GetID()
        {
            return id;
        }

        public override object GetValue(NodePort port)
        {
            return null;
        }
    }
}
