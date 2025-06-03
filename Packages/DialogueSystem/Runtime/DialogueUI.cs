using System;
using TMPro;
using UnityEngine;
using static DialogueSystem.DialogueHandler;

namespace DialogueSystem
{
    public class DialogueUI
    {
        private GameObject _paneGameObject;
        private GameObject _textGameObject;
        private TMP_Text _text;

        public TextEffects textEffects { get; private set; }
        public DialogueTheme theme { get; private set; }
        public DialogueSettings settings { get; private set; }
        public DialogueCallbackActions callbackActions { get; private set; }

        public bool isAnimating { get { return textEffects.IsAnimating; } }


        public DialogueUI(GameObject dialogueText, GameObject dialoguePane = null, DialogueTheme defaultTheme = null, DialogueSettings settings = null, DialogueCallbackActions callbackActions = null)
        {
            if (dialoguePane != null) _paneGameObject = dialoguePane;
            if (dialogueText != null)
            {
                _textGameObject = dialogueText;
                _text = _textGameObject.GetComponent<TMP_Text>();
                if (_text == null)
                {
                    _text = _textGameObject.AddComponent<TMP_Text>();
                }
            }
            else
            {
                Debug.LogError("Dialogue UI is missing dialogue text gameobject");
            }

            this.settings = settings == null ? ScriptableObject.CreateInstance<DialogueSettings>() : settings;
            this.callbackActions = callbackActions;
            theme = defaultTheme == null ? ScriptableObject.CreateInstance<DialogueTheme>() : defaultTheme;

            textEffects = _text.gameObject.AddComponent<TextEffects>();
            textEffects.Init(theme, settings, callbackActions);
        }

        public void SetDialoguePane(GameObject gameObject)
        {
            _paneGameObject = gameObject;
        }

        public void SetDialogueTextGameObject(GameObject gameObject)
        {
            _textGameObject = gameObject;
            _text = _textGameObject.GetComponent<TMP_Text>();
            if (_text == null)
            {
                _text = _textGameObject.AddComponent<TMP_Text>();
            }
        }

        public void Reset()
        {
            textEffects.ClearAllIndices();
        }

        public void SetDialogueText(string text, TextEffects.TextDisplayMode mode, Action<DialogueEventType> callback, float? typewriterSpeedOverride = null)
        {
            ShowDialoguePane(true);
            callback(DialogueEventType.OnTextStart);
            switch (mode)
            {
                case TextEffects.TextDisplayMode.TYPEWRITER:
                    textEffects.Typewriter(text, callback, typewriterSpeedOverride);
                    break;
                case TextEffects.TextDisplayMode.INSTANT:
                    _text.text = text;
                    callback(DialogueEventType.OnTextEnd);
                    break;
                default:
                    _text.text = text;
                    break;
            }
        }

        public void SetTheme(DialogueTheme theme)
        {
            this.theme = theme;
            textEffects.SetTheme(theme);
        }

        public void SetCallbackActions(DialogueCallbackActions callbackActions)
        {
            this.callbackActions = callbackActions;
            textEffects.SetCallbackActions(callbackActions);
        }

        public void ShowDialoguePane(bool toggle)
        {
            if (_paneGameObject != null)
            {
                _paneGameObject.SetActive(toggle);
                SetTextGameobjectActive(toggle);
            }
        }

        public void SetTextGameobjectActive(bool toggle)
        {
            _textGameObject.SetActive(toggle);
        }

        public void RegisterTextEffectIndices(TextCommand command, int startIndex, int endIndex)
        {
            if (command.effect != null)
            {
                TextEffect fx = theme.effects.Find(x => x.name.ToLower() == command.effect.ToLower());
                if (fx != null)
                {
                    textEffects.SetEffectIndices(fx, startIndex, endIndex);
                }

                if(command.color != null)
                {
                    textEffects.SetColorIndices(startIndex, endIndex , command.color);
                }

            }
        }

        public void RegisterWaitIndex(int index, int replacedTextLength, float waitTime)
        {
            textEffects.SetWaitIndex(index, waitTime);
            textEffects.OffsetExistingIndices(index, replacedTextLength);
        }

        public void Pause(bool toggle)
        {
            textEffects.Pause(toggle);
        }
    }
}
