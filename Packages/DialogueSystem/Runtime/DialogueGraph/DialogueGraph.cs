using System;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using System.Linq;
using UnityEditor;

namespace DialogueSystem
{
    [CreateAssetMenu(menuName = "Dialogue System/Dialogue Graph")]
    [Serializable]
    public class DialogueGraph : NodeGraph
    {

        [SerializeField]
        public List<BaseNode> dialogueNodes = new List<BaseNode>();

        public List<BranchNode> _branchNodes = new List<BranchNode>();


        public override Node AddNode(Type type)
        {
            Node originalNode = base.AddNode(type);
            dialogueNodes.Add(originalNode as BaseNode);
            if (type == typeof(BranchNode)) _branchNodes.Add(originalNode as BranchNode);
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            return originalNode;
        }

        public override void RemoveNode(Node node)
        {
            BaseNode baseNode = node as BaseNode;
            if (baseNode.NodeType == "BranchNode") _branchNodes.Remove(node as BranchNode);
            dialogueNodes.Remove(baseNode);
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            base.RemoveNode(node);
        }


        public List<BranchNode> GetAllBranchNodes()
        {
            if (_branchNodes.Count > 0) return _branchNodes;
            else
            {
                return dialogueNodes.Select(x => x.NodeType == "BranchNode").Cast<BranchNode>().ToList();
            }

        }

    }
}