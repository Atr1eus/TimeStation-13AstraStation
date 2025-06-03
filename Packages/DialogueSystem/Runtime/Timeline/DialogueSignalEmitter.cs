using DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[CreateAssetMenu]
public class DialogueSignalEmitter : SignalEmitter
{
    public PropertyName id => new PropertyName();

    [SerializeField]
    private DialogueGraph dialogueGraph;
    [SerializeField]
    private bool pauseTimeline = true;

    public DialogueGraph Graph => dialogueGraph;
    public bool PauseTimeline => pauseTimeline;

    
}
