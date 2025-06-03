using DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using XNodeEditor;

[CustomNodeEditor(typeof(EventNode))]
public class EventNodeEditor : NodeEditor
{

    ReorderableList list;
    bool hasArrayData;
    EventNode node;
    SerializedProperty arrayData;

    public override int GetWidth()
    {
        return 500;
    }

    public override void OnCreate()
    {
        if (node == null) node = target as EventNode;
        base.OnCreate();

        SerializedProperty arrayData = serializedObject.FindProperty("events");
        hasArrayData = arrayData != null && arrayData.isArray;

        list = EditorUtilities.CreateReorderableList("events", node.events, arrayData, typeof(int), serializedObject, null, (int)EditorGUIUtility.singleLineHeight * 8);
        list.list = node.events;
        //list = EditorUtilities.GetListWithFoldout(serializedObject,arrayData,true, true, true, true);

    }

    public override void OnBodyGUI()
    {
        

        serializedObject.Update();

        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("input"));

        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("nodeName"));

        list.DoLayoutList();
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("GoToNextNodeAutomatically"));
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("output"));

        serializedObject.ApplyModifiedProperties();
    }


}
