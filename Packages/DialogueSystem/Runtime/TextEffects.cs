using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using static DialogueSystem.DialogueHandler;
using System.Linq;

namespace DialogueSystem
{
    public class TextEffects : MonoBehaviour
    {
        DialogueTheme theme;
        DialogueSettings settings;
        DialogueCallbackActions callbackActions;

        /*Public member variables */
        public bool IsAnimating { get; private set; }
        public bool speedUp { get; private set; }
        public bool Paused { get; private set; }

        /*Control struct */
        class CharacterInfo
        {
            public bool processed;

            public float elapsedTime;
            public float totalDuration;

            public CharacterInfo(float duration)
            {
                totalDuration = duration;
                elapsedTime = 0;
                processed = false;

            }
        }

        /* Private member variables */
        private TMP_Text _textComponent;
        private TMP_TextInfo _textInfo;
        private Dictionary<int, Color> colorIndices = new Dictionary<int, Color>();
        private Dictionary<int, CharacterInfo> characterMap = new Dictionary<int, CharacterInfo>();
        private List<TextEffectWrapper> _textEffects = new List<TextEffectWrapper>();
        private Dictionary<int, float> _waitIndices = new Dictionary<int, float>();

        public enum TextDisplayMode
        {
            INSTANT = 0,
            TYPEWRITER = 1
        }

        public enum Effect
        {
            WAVE = 0,
        }

        public void SetTheme(DialogueTheme theme)
        {
            this.theme = theme;
        }

        public void SetCallbackActions(DialogueCallbackActions callbackActions)
        {
            this.callbackActions = callbackActions;
        }

        public void Pause(bool toggle)
        {
            Paused = toggle;
        }

        public void SetColorIndices(int start, int end, string color)
        {

            if (theme.colors.ContainsKey(color.ToLower()))
            {
                for (int i = start; i < end; ++i)
                {
                    colorIndices.Add(i, theme.colors[color]);
                }
            }
        }

        public void SetEffectIndices(TextEffect fx, int start, int end)
        {
            List<int> indices = new List<int>();
            for (int i = start; i < end; ++i)
            {
                indices.Add(i);
            }
            int id = _textEffects.Count();
            TextEffectWrapper wrapper = new TextEffectWrapper(id, fx, indices);
            _textEffects.Add(wrapper);
        }

        public void SetWaitIndex(int index, float time)
        {
            _waitIndices.Add(index, time);
        }

        public void OffsetExistingIndices(int start, int offsetAmount)
        {
            foreach(TextEffectWrapper wrapper in _textEffects)
            {
                if(wrapper.indices[0] > start)
                {
                    for (int i = 0; i < wrapper.indices.Count; i++)
                    {
                        wrapper.indices[i] -= offsetAmount;
                    }
                }
            }

            Dictionary<int, Color> newDict = new Dictionary<int, Color>();
            foreach(int index in colorIndices.Keys)
            {
                if(index > start)
                {
                    newDict.Add(index - offsetAmount, colorIndices[index]);
                } else
                {
                    newDict.Add(index, colorIndices[index]);
                }
            }
            colorIndices = newDict;
        }

        public void ClearAllIndices()
        {
            colorIndices.Clear();
            characterMap.Clear();
            _textEffects.Clear();
            _waitIndices.Clear();
        }

        public void Init(DialogueTheme theme, DialogueSettings settings, DialogueCallbackActions callbackActions = null)
        {
            _textComponent = GetComponent<TMP_Text>();
            this.theme = theme;
            this.settings = settings;
            this.callbackActions = callbackActions;
        }

        public void DisplayWholeText()
        {
            if (IsAnimating)
            {
                _textComponent.maxVisibleCharacters = _textComponent.textInfo.characterCount;
                IsAnimating = false;
            }
        }

        public void SpeedUp(bool toggle)
        {
            if (IsAnimating)
            {
                speedUp = toggle;
            }
        }

        public void Typewriter(string text, Action<DialogueEventType> callback, float? typewriterSpeedOverride = null)
        {
            _textComponent.maxVisibleCharacters = 0;
            _textComponent.text = text;
            StartCoroutine(TypewriterEffect(callback, typewriterSpeedOverride));
        }

        public IEnumerator TypewriterEffect(Action<DialogueEventType> callback, float? speedOverride)
        {
            _textComponent.ForceMeshUpdate();
            IsAnimating = true;
            float typeWriterWaitTime;
            for (int i = 0; i < _textComponent.textInfo.characterCount; ++i)
            {
                if (!IsAnimating) break;
                while (Paused) yield return new WaitForEndOfFrame();
                _textComponent.maxVisibleCharacters += 1;
                if (speedOverride != null)
                {
                    typeWriterWaitTime = speedOverride.Value;
                }
                else
                {
                    typeWriterWaitTime = speedUp ? DialogueUtilities.DecreasingFunction(settings.typewriterSpeed * settings.typewriterSpeedMultiplier) : DialogueUtilities.DecreasingFunction(settings.typewriterSpeed);
                }

                if (_waitIndices.TryGetValue(i, out float waitTime))
                {
                    typeWriterWaitTime = waitTime;
                }

                yield return new WaitForSeconds(typeWriterWaitTime);
            }

            callback(DialogueEventType.OnTextEnd);
            IsAnimating = false;
            yield return null;
        }

        private float GetOffset(int currentIndex, List<int> indices, float offsetAmount)
        {
            if(offsetAmount > 0)
            {
                return indices.IndexOf(currentIndex) * offsetAmount;
            }
            return 0;
        }


        private void ForEachCharacter(int charIndex, TMP_TextInfo textInfo)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[charIndex];
            if (!charInfo.isVisible)
            {
                return;
            }

            Vector3[] verts = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;
            Color32[] vertexColors = textInfo.meshInfo[charInfo.materialReferenceIndex].colors32;

            OnCharacterAppear(charIndex, charInfo, verts, vertexColors);

            TextEffectWrapper wrapper = _textEffects.Find(x => x.indices.Contains(charIndex));
            if(wrapper != null)
            {
                TextEffect fx = wrapper.fx;
                if (!fx.Active) return;
                for (int j = 0; j < 4; ++j)
                {
                    Vector3 orig = verts[charInfo.vertexIndex + j];
                    //float evaluatedXPosition = fx.XPosAnimationCurve.Evaluate(wrapper.deltaTime + (fx.OffsetXPosition ? orig.y : orig.z) * 0.01f) * 10f;
                    //float evaluatedYPosition = fx.YPosAnimationCurve.Evaluate(wrapper.deltaTime + (fx.OffsetYPosition ? orig.x : orig.z) * 0.01f) * 10f;
                    float evaluatedXPosition = fx.XPosAnimationCurve.Evaluate(wrapper.deltaTime - GetOffset(charIndex, wrapper.indices, fx.XPositionOffset)) * 10f;
                    float evaluatedYPosition = fx.YPosAnimationCurve.Evaluate(wrapper.deltaTime - GetOffset(charIndex, wrapper.indices, fx.YPositionOffset)) * 10f;
                    verts[charInfo.vertexIndex + j] = orig + new Vector3(evaluatedXPosition, evaluatedYPosition, 0);

                    //// Scale
                    Vector3[] vertices = new Vector3[] { verts[charInfo.vertexIndex], verts[charInfo.vertexIndex + 1], verts[charInfo.vertexIndex + 2], verts[charInfo.vertexIndex + 3] };
                    Vector3 preScaleCenterPoint = DialogueUtilities.CalculateCenter(vertices);
                    float evaluatedScale = fx.scaleAnimationCurve.Evaluate(wrapper.deltaTime - GetOffset(charIndex, wrapper.indices, fx.ScaleOffset));
                    verts[charInfo.vertexIndex + j] = (verts[charInfo.vertexIndex + j] - preScaleCenterPoint) * evaluatedScale + preScaleCenterPoint;

                }

                // Rotation
                Vector3[] v = new Vector3[] { verts[charInfo.vertexIndex], verts[charInfo.vertexIndex + 1], verts[charInfo.vertexIndex + 2], verts[charInfo.vertexIndex + 3] };
                DialogueUtilities.RotateVertices(fx.rotationAnimationCurve.Evaluate(wrapper.deltaTime - GetOffset(charIndex, wrapper.indices, fx.RotationOffset)), ref v);
                for (int i = 0; i < v.Length; i++)
                {
                    verts[charInfo.vertexIndex + i] = v[i];
                }
            }

            if (colorIndices.ContainsKey(charIndex))
            {

                for (int j = 0; j < 4; ++j)
                {
                    vertexColors[charInfo.vertexIndex + j] = colorIndices[charIndex];
                }
            }
        }

        private void OnCharacterAppear(int charIndex, TMP_CharacterInfo charInfo, Vector3[] verts, Color32[] vertexColors)
        {
            int numKeys = theme.OnLetterAppearXPos.length;
            float positionDuration = theme.OnLetterAppearXPos.keys[numKeys - 1].time;
            float colorDuration = theme.OnLetterAppearOpacity.keys[numKeys - 1].time;
            if (!characterMap.ContainsKey(charIndex))
            {
                CharacterInfo characterInfo = new CharacterInfo(Mathf.Max(positionDuration, colorDuration));
                characterMap.Add(charIndex, characterInfo);
                char character = _textComponent.text[charIndex];
                callbackActions.OnCharacterAppear?.Invoke(character);

                TextEffectWrapper wrapper = _textEffects.Find(x => x.indices.Contains(charIndex));
                if(wrapper != null)
                {
                    wrapper.animating = true;
                }
            }


            CharacterInfo currentCharacter;
            if (characterMap.TryGetValue(charIndex, out currentCharacter))
            {
                if (currentCharacter.processed) return;

                currentCharacter.elapsedTime += Time.deltaTime;

                if (currentCharacter.elapsedTime < currentCharacter.totalDuration)
                {
                    for (int j = 0; j < 4; ++j)
                    {
                        Vector3 orig = verts[charInfo.vertexIndex + j];
                        float yPos = theme.OnLetterAppearYPos.Evaluate(currentCharacter.elapsedTime);
                        float xPos = theme.OnLetterAppearXPos.Evaluate(currentCharacter.elapsedTime);
                        verts[charInfo.vertexIndex + j] = orig + new Vector3(xPos * 10, yPos * 10, 0);

                        Color32 charColor = vertexColors[charInfo.vertexIndex + j];
                        float opacity = DialogueUtilities.FloatToByte(theme.OnLetterAppearOpacity.Evaluate(currentCharacter.elapsedTime));
                        charColor.a = (byte)opacity;
                        vertexColors[charInfo.vertexIndex + j] = charColor;
                    }
                }
                else
                {
                    currentCharacter.processed = true;
                }
            }
            else
            {
                Debug.LogWarning("No character found in char map");
            }
        }


        private void UpdateDeltaTime()
        {

            foreach(TextEffectWrapper fx in _textEffects)
            {
                if(fx.animating)
                {
                    fx.deltaTime += Time.deltaTime;
                }
            }
        }

        void LateUpdate()
        {

            if (!Paused)
            {
                _textInfo = _textComponent.textInfo;
                if (_textInfo.characterCount > 0)
                {
                    _textComponent.ForceMeshUpdate();
                    UpdateDeltaTime();

                    for (int i = 0; i < _textInfo.characterCount; ++i)
                    {
                        ForEachCharacter(i, _textInfo);

                    }

                    for (int i = 0; i < _textInfo.meshInfo.Length; ++i)
                    {
                        var meshInfo = _textInfo.meshInfo[i];
                        meshInfo.mesh.vertices = meshInfo.vertices;
                        meshInfo.mesh.colors32 = meshInfo.colors32;

                        _textComponent.UpdateGeometry(meshInfo.mesh, i);
                    }
                }
            }
        }
    }
}
