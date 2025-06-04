using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using XNode;

namespace DialogueSystem {
    public class NodeParser
    {
        public void GenerateScript(DialogueGraph graph)
        {
            string f = Application.persistentDataPath + "/dialogueScript.json";

            DialogueScript script = new DialogueScript();
            foreach (BaseNode node in graph.nodes)
            {
                if (node.GetString() != null)
                {
                    DialogueScriptLine line = new DialogueScriptLine(node.GetString(), node.GetID());
                    script.lines.Add(line);
                }
            }
            string json = JsonUtility.ToJson(script, true);
            File.WriteAllText(f, json);
        }

        public DialogueGraph GenerateGraphFromScript(DialogueGraph graph)
        {
            string f = Application.persistentDataPath + "/dialogueScript.json";
            StreamReader reader = new StreamReader(f);
            DialogueScript dialogueScript = JsonUtility.FromJson<DialogueScript>(reader.ReadToEnd());
            foreach (DialogueScriptLine line in dialogueScript.lines)
            {
                string id = line.id;
                BaseNode textNode = graph.dialogueNodes.Find(node => node.GetID() == id);
                if (textNode == null) Debug.LogError($"{graph.name}: Text node with ID " + line.id + " could not be found.");
                else
                {
                    textNode.SetText(line.text);
                }
            }
            return graph;
        }
    }

    public class TestWindow : EditorWindow
    {
        public NodeParser parser = new NodeParser();


        [MenuItem("Window/Test Window")]
        public static void ShowWindow()
        {
            GetWindow(typeof(TestWindow));
        }

        private void OnGUI()
        {

            if (GUILayout.Button("Try Serialize"))
            {
                parser.GenerateScript((DialogueGraph)AssetDatabase.LoadAssetAtPath("Assets/DialogueSystem/Samples/A.asset", typeof(DialogueGraph)));
            }

            if (GUILayout.Button("Try Deserialize"))
            {

                //parser.Deserialize(graph);

                parser.GenerateGraphFromScript((DialogueGraph)AssetDatabase.LoadAssetAtPath("Assets/DialogueSystem/Samples/A.asset", typeof(DialogueGraph)));
            }

            if (GUILayout.Button("try save graph"))
            {
                DialogueGraph graph = (DialogueGraph)AssetDatabase.LoadAssetAtPath("Assets/DialogueSystem/Samples/A.asset", typeof(DialogueGraph));
                EditorUtility.SetDirty(graph);

            }
        }


    }

    [Serializable]
    public class DialogueScript
    {
        public List<DialogueScriptLine> lines = new List<DialogueScriptLine>();
    }

    [Serializable]
    public class DialogueScriptLine
    {
        public DialogueScriptLine(string[] text, string id)
        {
            this.text = text;
            this.id = id;
        }
        public string[] text;
        public string id;
    }

}