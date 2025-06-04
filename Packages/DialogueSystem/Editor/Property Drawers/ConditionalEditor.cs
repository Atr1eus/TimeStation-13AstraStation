using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

[CustomPropertyDrawer(typeof(DialogueConditional))]
public class ConditionalEditor : PropertyDrawer
{

    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        //VisualElement container = new VisualElement();

        //PropertyField variableType = new PropertyField(property.FindPropertyRelative("type"));

        //container.Add(variableType);

        //return container;
        return base.CreatePropertyGUI(property);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //base.OnGUI(position, property, label);
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        Rect typeRect = new Rect(position.x, position.y, 80, position.height);
        Rect variableRect = new Rect(typeRect.x + typeRect.width + 5, position.y, 100, position.height);
        Rect restrictionRect = new Rect(variableRect.x + variableRect.width + 5, position.y, 80, position.height);
        Rect conditionRect = new Rect(restrictionRect.x + restrictionRect.width + 5, position.y, 150, position.height);

        SerializedProperty type = property.FindPropertyRelative("type");

        EditorGUI.PropertyField(typeRect, type, GUIContent.none);
        EditorGUI.PropertyField(variableRect, property.FindPropertyRelative("variable"), GUIContent.none);

        SerializedProperty condition = null;
        SerializedProperty target = null;
        switch (type.enumValueIndex) {
            case 0:
                condition = property.FindPropertyRelative("numberCondition");
                target = property.FindPropertyRelative("intTarget");
                break;
            case 1:
                condition = property.FindPropertyRelative("stringCondition");
                target = property.FindPropertyRelative("stringTarget");
                break;
            case 2:
                condition = property.FindPropertyRelative("boolCondition");
                target = property.FindPropertyRelative("boolTarget");
                break;
            case 3:
                condition = property.FindPropertyRelative("numberCondition");
                target = property.FindPropertyRelative("floatTarget");
                break;
        }

        EditorGUI.PropertyField(restrictionRect, condition, GUIContent.none);

        if(type.enumValueIndex != 2)
        {
            EditorGUI.PropertyField(conditionRect, target, GUIContent.none);
        } 


        EditorGUI.EndProperty();
    }

}
