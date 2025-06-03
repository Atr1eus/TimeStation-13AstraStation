using UnityEditor;
using UnityEngine;
using XNodeEditor;
using DialogueSystem;

[CustomNodeEditor(typeof(TextNode))]
public class TextNodeEditor : NodeEditor
{

    public override int GetWidth()
    {
        return 650;
    }

    //public override void OnBodyGUI()
    //{

    //    base.OnBodyGUI();
    //    serializedObject.Update();




    //    serializedObject.ApplyModifiedProperties();
    //}


}

[CustomPropertyDrawer(typeof(TextNode.MetaData))]
public class MetaDataPropertyDrawer: PropertyDrawer
{

    public float offset = 20;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {

        EditorGUI.BeginProperty(position, label, property);

        EditorGUILayout.BeginHorizontal();

        Rect keyRect = new Rect(position.x, position.y + offset, 150, EditorGUIUtility.singleLineHeight);
        EditorGUI.LabelField(new Rect(keyRect.x, keyRect.y - 20, keyRect.width, keyRect.height), new GUIContent("Key"));
        EditorGUI.PropertyField(keyRect, property.FindPropertyRelative("key"), GUIContent.none);

        Rect valueRect = new Rect(position.x + keyRect.width + 5, position.y + offset, 250, EditorGUIUtility.singleLineHeight);
        EditorGUI.LabelField(new Rect(valueRect.x, valueRect.y - 20, valueRect.width, valueRect.height), new GUIContent("Value"));
        EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("value"), GUIContent.none);
        EditorGUILayout.EndHorizontal();

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label) + 20;
    }
}