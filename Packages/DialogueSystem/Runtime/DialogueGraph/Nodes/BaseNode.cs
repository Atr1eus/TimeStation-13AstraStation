using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace DialogueSystem
{
    public class BaseNode : Node
    {
        public string id = null;
        public string nodeName;
        public virtual string NodeType => "BaseNode";

        [HideInInspector]
        public bool processed = false;
        [HideInInspector]
        public bool entered = false;

        protected override void Init()
        {
#if UNITY_EDITOR
            id = UnityEditor.GUID.Generate().ToString();
#endif
            processed = false;
            entered = false;
            base.Init();
        }

        public virtual string[] GetString()
        {
            return null;
        }

        public virtual string GetID()
        {
            return id;
        }

        public virtual void SetText(string[] text)
        {
        }

    }
}
