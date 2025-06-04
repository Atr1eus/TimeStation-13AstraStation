using DialogueSystem;
using XNodeEditor;

[CustomNodeEditor(typeof(ConditionalNode))]
public class ConditionalNodeEditor : NodeEditor
{
    private ConditionalNode node;

    public override int GetWidth()
    {
        return 550;
    }


    public override void OnBodyGUI()
    {
        if (node == null) node = target as ConditionalNode;
        // Update serialized object's representation
        serializedObject.Update();

        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("input"));

        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("nodeName"));

        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("condition"));

        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("id"));

        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("trueOutput"));

        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("falseOutput"));

        // Apply property modifications
        serializedObject.ApplyModifiedProperties();
    }
}
