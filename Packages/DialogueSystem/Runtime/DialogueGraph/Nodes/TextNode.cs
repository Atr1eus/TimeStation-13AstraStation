using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using XNode;

namespace DialogueSystem
{
    public class TextNode : BaseNode
    {
        public override string NodeType => "TextNode";

        [Input] public float input;

        //Dictionary<string, string> metaData = new Dictionary<string, string>();
        [Serializable]
        public struct MetaData
        {
            public string key;
            public string value;
        }

        public List<MetaData> metaData = new List<MetaData>() { new MetaData(), new MetaData() };

        public AudioClip audioClip;
        public bool syncTypewriter = false;

        [TextArea]
        public string text;

        public bool WaitForInput = true;

        [Output] public float output;

        protected override void Init()
        {
            nodeName = "Text node";
            base.Init();

        }

        public override string[] GetString()
        {
            return new string[] { text };
        }

        public override void SetText(string[] text)
        {
            this.text = text[0];
        }

        public override string GetID()
        {
            return id;
        }

        public override object GetValue(NodePort port)
        {
            return null;
        }

        public string TryGetMetadataByKey(string key, DialogueDictionary dictionary = null)
        {
            TextNode.MetaData nullableMetaDataEntry = metaData.Find(x => x.key == key);
            if (dictionary != null && nullableMetaDataEntry.value != null)
            {
                string test = DialogueUtilities.ReplaceWords(nullableMetaDataEntry.value, dictionary);
                return test;
            }
            return nullableMetaDataEntry.value;
        }

    }
}
