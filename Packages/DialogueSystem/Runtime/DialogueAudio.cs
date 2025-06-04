using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Globalization;

namespace DialogueSystem
{
    public class DialogueAudio : MonoBehaviour
    {

        public AudioSource audioSource;
        public Dialogue dialogueSystem;

        public AudioClip OnLetterAppearSound;

        public State CurrentState { get; private set; }

        public enum State
        {
            Idle,
            PlayingAudio
        }

        private void Start()
        {
            if (audioSource == null)
            {
                audioSource = GetComponent<AudioSource>();
                if (audioSource == null)
                {
                    Debug.LogError("Audio source missing on Dialogue Audio component!");
                }
            }

            if (dialogueSystem == null)
            {
                dialogueSystem = Dialogue.instance;
                if (dialogueSystem == null)
                {
                    Debug.LogError("Dialogue component missing on Dialogue Audio component!");
                }
            }

            dialogueSystem.dialogueCallbackActions.OnCharacterAppear += OnCharacterAppear;
            dialogueSystem.dialogueCallbackActions.OnTextNodeStart += OnTextStart;

        }

        private void OnCharacterAppear(char character)
        {
            if (OnLetterAppearSound != null) audioSource.PlayOneShot(OnLetterAppearSound);
        }

        private void OnTextStart(TextNode node)
        {
            if (CurrentState == State.PlayingAudio)
            {
                audioSource.Stop();
            }

            if (node.audioClip != null)
            {
                CurrentState = State.PlayingAudio;
                audioSource.PlayOneShot(node.audioClip);

            }
        }

        private void OnTextEnd(TextNode node)
        {
            if (CurrentState == State.PlayingAudio)
            {
                audioSource.Stop();
            }
        }

        private void Update()
        {
            if (audioSource != null)
            {
                if (CurrentState == State.PlayingAudio)
                {
                    if (!audioSource.isPlaying)
                    {
                        CurrentState = State.Idle;
                    }
                }
            }
        }

    }
}
