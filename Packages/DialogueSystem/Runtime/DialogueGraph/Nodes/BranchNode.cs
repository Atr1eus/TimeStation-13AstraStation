using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    public class BranchNode : BaseNode
    {
        public override string NodeType => "BranchNode";

        [Output] public float output;

        public List<DialogueConditional> branchCondition = new List<DialogueConditional>() { new DialogueConditional() };

        protected override void Init()
        {
            nodeName = "Branch";
            base.Init();
        }

        public override string GetID()
        {
            return id;
        }
    }
}
