using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace DialogueSystem
{
    public class EditorUtilities
    {
        private static int reorderableListIndex = -1;

        public static ReorderableList CreateReorderableList(string fieldName, List<DialogueEvent> ilist, SerializedProperty arrayData, Type type, SerializedObject serializedObject, Action<ReorderableList> onCreation, int contentHeight)
        {
            bool hasArrayData = arrayData != null && arrayData.isArray;
            XNode.Node node = serializedObject.targetObject as XNode.Node;
            //ReorderableList list = new ReorderableList(dynamicPorts, null, true, true, true, true);
            //ReorderableList list = new ReorderableList(ilist, null, true, true, true, true);
            ReorderableList list = new ReorderableList(serializedObject, arrayData, true, true, true, true);
            string label = arrayData != null ? arrayData.displayName : ObjectNames.NicifyVariableName(fieldName);

            list.drawElementCallback +=
                (Rect rect, int index, bool isActive, bool isFocused) => {
                //XNode.NodePort port = node.GetPort(fieldName + " " + index);
                if (hasArrayData && arrayData.propertyType != SerializedPropertyType.String)
                    {
                        if (arrayData.arraySize <= index)
                        {
                            EditorGUI.LabelField(rect, "Array[" + index + "] data out of range");
                            return;
                        }
                    //EditorGUI.Foldout
                    SerializedProperty itemData = arrayData.GetArrayElementAtIndex(index);
                        EditorGUI.PropertyField(rect, itemData, true);
                    }
                    else EditorGUI.LabelField(rect, "asd");

                };
            list.elementHeightCallback =
                (int index) =>
                {
                    if (hasArrayData)
                    {
                        if (arrayData.arraySize <= index) return EditorGUIUtility.singleLineHeight;
                        SerializedProperty itemData = arrayData.GetArrayElementAtIndex(index);
                        if (!itemData.isExpanded) return 23;
                        return EditorGUI.GetPropertyHeight(itemData) == 23 ? 138 : EditorGUI.GetPropertyHeight(itemData);
                    }
                    else return EditorGUIUtility.singleLineHeight;
                };
            list.drawHeaderCallback =
                (Rect rect) =>
                {
                    EditorGUI.LabelField(rect, label);
                };
            list.onSelectCallback =
                (ReorderableList rl) => {
                    reorderableListIndex = rl.index;
                };
            list.onReorderCallback =
                (ReorderableList rl) => {
                    Rect rect = Rect.zero;
                    Rect newRect = Rect.zero;

                // Apply changes
                serializedObject.ApplyModifiedProperties();
                    serializedObject.Update();

                // Move array data if there is any
                if (hasArrayData)
                    {
                        arrayData.MoveArrayElement(reorderableListIndex, rl.index);
                    }

                // Apply changes
                serializedObject.ApplyModifiedProperties();
                    serializedObject.Update();
                    XNodeEditor.NodeEditorWindow.current.Repaint();
                    EditorApplication.delayCall += XNodeEditor.NodeEditorWindow.current.Repaint;
                };
            list.onAddCallback =
                (ReorderableList rl) => {

                    serializedObject.Update();
                    EditorUtility.SetDirty(node);
                    if (hasArrayData)
                    {
                        arrayData.InsertArrayElementAtIndex(arrayData.arraySize);
                    }
                    serializedObject.ApplyModifiedProperties();
                };
            list.onRemoveCallback =
                (ReorderableList rl) => {

                    serializedObject.Update();
                    EditorUtility.SetDirty(node);
                    int index = rl.index;

                    if (hasArrayData && arrayData.propertyType != SerializedPropertyType.String)
                    {
                        if (arrayData.arraySize <= index)
                        {
                            Debug.LogWarning("Attempted to remove array index " + index + " where only " + arrayData.arraySize + " exist - Skipped");
                            Debug.Log(rl.list[0]);
                            return;
                        }
                        arrayData.DeleteArrayElementAtIndex(index);
                    if (ilist.Count <= arrayData.arraySize)
                        {
                            while (ilist.Count <= arrayData.arraySize)
                            {
                                arrayData.DeleteArrayElementAtIndex(arrayData.arraySize - 1);
                            }
                        }
                        serializedObject.ApplyModifiedProperties();
                        serializedObject.Update();
                    }
                };

            if (hasArrayData)
            {
                int dynamicPortCount = ilist.Count;
                while (dynamicPortCount < arrayData.arraySize)
                {
                    // Add dynamic port postfixed with an index number
                    string newName = arrayData.name + " 0";
                    EditorUtility.SetDirty(node);
                    dynamicPortCount++;
                }
                while (arrayData.arraySize < dynamicPortCount)
                {
                    arrayData.InsertArrayElementAtIndex(arrayData.arraySize);
                }
                serializedObject.ApplyModifiedProperties();
                serializedObject.Update();
            }
            if (onCreation != null) onCreation(list);
            return list;
        }

        public static ReorderableList GetListWithFoldout(SerializedObject serializedObject, SerializedProperty property, bool draggable, bool displayHeader, bool displayAddButton, bool displayRemoveButton)
        {
            var list = new ReorderableList(serializedObject, property, draggable, displayHeader, displayAddButton, displayRemoveButton);

            list.drawHeaderCallback = (Rect rect) => {
                var newRect = new Rect(rect.x + 10, rect.y, rect.width - 10, rect.height);
                property.isExpanded = EditorGUI.Foldout(newRect, property.isExpanded, property.displayName);
            };
            list.drawElementCallback =
                (Rect rect, int index, bool isActive, bool isFocused) => {
                    if (!property.isExpanded)
                    {
                        GUI.enabled = index == list.count;
                        return;
                    }

                    var element = list.serializedProperty.GetArrayElementAtIndex(index);
                    rect.y += 2;
                    EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element, GUIContent.none);
                };
            list.elementHeightCallback = (int indexer) => {
                if (!property.isExpanded)
                    return 0;
                else
                    return list.elementHeight;
            };

            return list;
        }
    }
}



