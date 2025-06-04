using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    [Serializable]
    public class DialogueEvent
    {
        public string eventName;

        public int intParameter;
        public float floatParameter;
        public string stringParameter;
        public bool boolParameter;


    }
}
