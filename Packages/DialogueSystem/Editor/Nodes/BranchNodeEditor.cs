using DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNodeEditor;

[CustomNodeEditor(typeof(BranchNode))]
public class BranchNodeEditor : NodeEditor
{
    private BranchNode node;

    public override int GetWidth()
    {
        return 500;
    }


    public override void OnBodyGUI()
    {
        if (node == null) node = target as BranchNode;
        // Update serialized object's representation
        serializedObject.Update();

        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("nodeName"));

        GUILayout.BeginHorizontal("box");
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("branchCondition"));
        GUILayout.EndHorizontal();

        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("output"));

        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("id"));

        // Apply property modifications
        serializedObject.ApplyModifiedProperties();
    }
}
