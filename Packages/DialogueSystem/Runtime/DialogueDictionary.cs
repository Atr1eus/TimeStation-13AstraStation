using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    [Serializable]
    public class DialogueDictionary
    {
        //List<DialogueDictionaryEntry> entries = new List<DialogueDictionaryEntry>();
        private List<DialogueDictionaryEntry> entries = new List<DialogueDictionaryEntry>();

        public bool AddEntry(string key, string value)
        {
            if (entries.Exists(x => x.Key == key))
            {
                Debug.LogError($"An entry with key {key} already exists.");
                return false;
            }
            DialogueDictionaryEntry newEntry = new DialogueDictionaryEntry(key, value);
            entries.Add(newEntry);
            return true;
        }

        public bool AddEntry(string key, Func<string> getValueFunction)
        {
            if (entries.Exists(x => x.Key == key))
            {
                Debug.LogError($"An entry with key {key} already exists.");
                return false;
            }
            DialogueDictionaryEntry newEntry = new DialogueDictionaryEntry(key, getValueFunction);
            entries.Add(newEntry);
            return true;
        }

        public string GetEntry(string key)
        {
            DialogueDictionaryEntry entry = TryGetEntry(key);
            if (entry != null)
            {
                return entry.Value;
            }
            return null;
        }

        public bool ModifyValye(string key, string newValue)
        {
            DialogueDictionaryEntry entry = TryGetEntry(key);
            if (entry != null)
            {
                entry.ChangeValue(newValue);
                return true;
            }
            return false;
        }

        public bool ModifyValye(string key, Func<string> getValueFunction)
        {
            DialogueDictionaryEntry entry = TryGetEntry(key);
            if (entry != null)
            {
                entry.ChangeValue(getValueFunction);
                return true;
            }
            return false;
        }

        public bool DeleteValue(string key)
        {
            DialogueDictionaryEntry entry = TryGetEntry(key);
            if (entry != null)
            {
                entries.Remove(entry);
                return true;
            }
            return false;
        }

        private DialogueDictionaryEntry TryGetEntry(string key)
        {
            DialogueDictionaryEntry entry = entries.Find(x => x.Key == key);
            if (entry == null)
            {
                Debug.LogError($"No dictionary entry found with key {key}.");
                return null;
            }

            return entry;
        }
    }

    [Serializable]
    public class DialogueDictionaryEntry
    {
        public string Key { get; private set; }
        private Func<string> getValueFunction = null;
        private string _value;
        public string Value
        {
            get
            {
                if (getValueFunction != null) return getValueFunction.Invoke();
                else return _value;
            }

            set
            {
                _value = value;
            }
        }

        public DialogueDictionaryEntry(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public DialogueDictionaryEntry(string key, Func<string> getValueFunction)
        {
            Key = key;
            this.getValueFunction = getValueFunction;
        }

        public void ChangeValue(string value)
        {
            Value = value;
            getValueFunction = null;
        }

        public void ChangeValue(Func<string> newGetValueFunction)
        {
            this.getValueFunction = newGetValueFunction;
        }

    }

}
