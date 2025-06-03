using DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNodeEditor;


[CustomPropertyDrawer(typeof(DialogueChoice))]
public class DialogueChoicePropertyDrawer : PropertyDrawer
{
    private ChoiceNode node;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        EditorGUI.indentLevel = 1;

        EditorGUILayout.BeginHorizontal();
        Rect choiceText = new Rect(position.x, position.y, 200, position.height);
        SerializedProperty text= property.FindPropertyRelative("text");
        EditorGUI.PropertyField(choiceText, text, GUIContent.none);

        

        EditorGUILayout.EndHorizontal();

        EditorGUI.EndProperty();
    }
}
