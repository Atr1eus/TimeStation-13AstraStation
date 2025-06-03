using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueSystem;

public class SimpleDialogueInteraction : MonoBehaviour
{

    public Dialogue dialogueManager;

    // Start is called before the first frame update
    void Start()
    {
        dialogueManager.dialogueCallbackActions.OnDialogueGraphEnd += (DialogueGraph graph) => { Debug.Log("Hi"); };
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            if (dialogueManager.IsRunning)
            {
                if (dialogueManager.isAnimating)
                {
                    dialogueManager.EndLine();
                }
                else
                {
                    if (dialogueManager.CurrentState != DialogueState.AwaitingEventResponse) dialogueManager.AdvanceDialogue();
                }

            }
        }
    }
}
