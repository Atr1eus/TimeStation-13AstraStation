using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueSystem;

namespace DialogueSystem
{
    //Serves as a wrapper for TextEffect objects.
    public class TextEffectWrapper
    {
        public TextEffect fx { get; private set; }
        public float deltaTime = 0f;
        public List<int> indices;

        public int id { get; private set; }

        public bool animating = false;

        public TextEffectWrapper(int id, TextEffect fx, List<int> indices)
        {
            this.fx = fx;
            this.id = id;
            this.indices = indices;
        }

    }

}