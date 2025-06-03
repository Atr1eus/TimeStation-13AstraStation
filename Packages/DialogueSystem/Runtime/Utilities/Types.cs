using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DialogueSystem
{
    public enum DialogueState
    {
        NotRunning,
        Idle,
        Running,
        Animating,
        AwaitingChoice,
        Waiting,
        Paused,
        AwaitingEventResponse,
        PausedByTimeline,
    }
}
