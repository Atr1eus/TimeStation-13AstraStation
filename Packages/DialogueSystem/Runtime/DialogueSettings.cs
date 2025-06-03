using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DialogueSystem
{
    [CreateAssetMenu(menuName = "Dialogue System/Settings")]
    [Serializable]
    public class DialogueSettings: ScriptableObject
    {
        [SerializeField]
        public MultipleValidBranchesSelectionMode multipleValidBranchesSelectionMode;

        /* Enums */
        [Serializable]
        public enum MultipleValidBranchesSelectionMode
        {
            FIRST,
            PRIORITY,
            RANDOM
        }

        public bool HideDialoguePaneOnStart = true;

        public TextEffects.TextDisplayMode textDisplayMode = TextEffects.TextDisplayMode.TYPEWRITER;

        public bool showWarnings = true;

        [Header("Typewriter")]
        public float typewriterSpeed = 15f;
        public float typewriterSpeedMultiplier = 2f;

        [Header("Timeline")]
        public bool autoResumeTimelineOnDialogueEnd = true;

    }
}
