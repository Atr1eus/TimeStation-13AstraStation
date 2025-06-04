using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    public class DialogueGameState
    {
        public string name;
        public int IntValue { get; private set; }
        public float FloatValue { get; private set; }
        public string StringValue { get; private set; }
        public bool BoolValue { get; private set; }

        public Type ValueType { get; private set; }

        public DialogueGameState(int value, string name)
        {
            IntValue = value;
            ValueType = typeof(int);
            this.name = name;
        }

        public DialogueGameState(float value, string name)
        {
            FloatValue = value;
            ValueType = typeof(float);
            this.name = name;
        }

        public DialogueGameState(string value, string name)
        {
            StringValue = value;
            ValueType = typeof(string);
            this.name = name;
        }

        public DialogueGameState(bool value, string name)
        {
            BoolValue = value;
            ValueType = typeof(bool);
            this.name = name;
        }

        public void ChangeValue(int newValue)
        {
            IntValue = newValue;
        }

        public void ChangeValue(float newValue)
        {
            FloatValue = newValue;
        }

        public void ChangeValue(string newValue)
        {
            StringValue = newValue;
        }

        public void ChangeValue(bool newValue)
        {
            BoolValue = newValue;
        }

    }

}