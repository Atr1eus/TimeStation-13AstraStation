using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    [CreateAssetMenu(menuName = "Dialogue System/Theme")]
    [Serializable]
    public class DialogueTheme : ScriptableObject
    {
        [Header("Colors")]
        public Color32 red = new Color32(230, 57, 70, 255);
        public Color32 yellow = new Color32(255,210,63, 255);
        public Color32 blue = new Color32(132,230,248, 255);
        public Color32 green = new Color32(119,173,120, 255);
        public Color32 purple = new Color32(203,179,191, 255);

        [Serializable]
        public struct ColorDictionary
        {
            public string name;
            public Color32 color;
        }

        public List<ColorDictionary> customColors = new List<ColorDictionary>();
        public Dictionary<string, Color> colors;

        [Header("Effects")]
        public AnimationCurve OnLetterAppearXPos = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 0));
        public AnimationCurve OnLetterAppearYPos = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 0));
        public AnimationCurve OnLetterAppearOpacity = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));
        //[Header("Custom text effects")]
        public List<TextEffect> effects;

        /*Private members*/
        private AnimationCurve _defaultXPos = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 0));
        private AnimationCurve _defaultYPos = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 0));
        private AnimationCurve _defaultScale = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));
        private AnimationCurve _defaultRotation = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 0));

        public DialogueTheme()
        {
            colors = new Dictionary<string, Color>()
        {
            {"red", red },
            {"yellow", yellow },
            {"blue", blue },
            {"green", green},
            {"purple", purple}
        };

            foreach (ColorDictionary color in customColors)
            {
                colors.Add(color.name.ToLower(), color.color);
            }

            effects = new List<TextEffect>() {
                new TextEffect("Wave",
                    new AnimationCurve(_defaultXPos.keys),
                    new AnimationCurve(new Keyframe(0, -.5f), new Keyframe(.5f, 1),new Keyframe(1, -.5f)), //yPos
                    new AnimationCurve(_defaultScale.keys),
                    new AnimationCurve(_defaultRotation.keys),
                    true),

                new TextEffect("Shake",
                    new AnimationCurve(new Keyframe(0, 0), new Keyframe(0.05f, 0.25f), new Keyframe(0.155f, -0.25f), new Keyframe(0.2f, 0f)), //xPos
                    new AnimationCurve(_defaultYPos.keys),
                    new AnimationCurve(_defaultScale.keys),
                    new AnimationCurve(_defaultRotation.keys),

                    true),

                new TextEffect("Bounce",
                    new AnimationCurve(_defaultXPos.keys),
                    new AnimationCurve(new Keyframe(0, -.5f), new Keyframe(.5f, 1),new Keyframe(1, -.5f)), //yPos
                    new AnimationCurve(_defaultScale.keys),
                    new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 360)), //Rotation
                    true)
            };

        }

        public void AddColor(string name, Color color)
        {
            colors.Add(name, color);
        }

        public void ChangeColor(string name, Color newColor)
        {
            string key = name.ToLower();
            colors[key] = newColor;
        }
    }

}