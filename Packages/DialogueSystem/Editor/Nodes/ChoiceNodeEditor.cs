using DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using XNode;
using XNodeEditor;

[CustomNodeEditor(typeof(ChoiceNode))]
public class ChoiceNodeEditor: NodeEditor
{
    private ChoiceNode node;
    bool hasArrayData;

    public override int GetWidth()
    {
        return 400;
    }


    public override void OnBodyGUI()
    {
        if (node == null) node = target as ChoiceNode;
        // Update serialized object's representation
        serializedObject.Update();

        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("input"));

        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("nodeName"));

        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("textPrompt"));

        EditorGUILayout.Space(10);

        NodeEditorGUILayout.DynamicPortList("choices", typeof(DialogueChoice), serializedObject, NodePort.IO.Output, Node.ConnectionType.Override, Node.TypeConstraint.None, OnCreateReorderableList);

        serializedObject.ApplyModifiedProperties();
    }

    void OnCreateReorderableList(ReorderableList list)
    {
        SerializedProperty arrayData = serializedObject.FindProperty("choices");
        hasArrayData = arrayData != null && arrayData.isArray;

        list.drawHeaderCallback = (Rect rect) =>
        {
            string title = "Choices";
            EditorGUI.LabelField(rect, title);
        };

        list.drawElementCallback =
                (Rect rect, int index, bool isActive, bool isFocused) => {
                    XNode.NodePort port = node.GetPort("choices" + " " + index);
                    if (hasArrayData && arrayData.propertyType != SerializedPropertyType.String)
                    {
                        if (arrayData.arraySize <= index)
                        {
                            EditorGUI.LabelField(rect, "Array[" + index + "] data out of range");
                            return;
                        }
                        SerializedProperty itemData = arrayData.GetArrayElementAtIndex(index);
                        EditorGUI.PropertyField(rect, itemData, new GUIContent($"Choice {index}"), true);
                    }
                    else EditorGUI.LabelField(rect, port != null ? port.fieldName : "");
                    if (port != null)
                    {
                        Vector2 pos = rect.position + (port.IsOutput ? new Vector2(rect.width + 6, 0) : new Vector2(-36, 0));

                        NodeEditorGUILayout.PortField(pos, port);
                    }
                };
    }
}
