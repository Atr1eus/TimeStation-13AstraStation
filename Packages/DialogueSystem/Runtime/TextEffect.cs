using UnityEngine;

namespace DialogueSystem
{
    [System.Serializable]
    public class TextEffect
    {
        public string name;
        public bool Active = true;

        public AnimationCurve YPosAnimationCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 0));
        public AnimationCurve XPosAnimationCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 0));

        public AnimationCurve scaleAnimationCurve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));

        public AnimationCurve rotationAnimationCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 0));

        [Min(0f)]
        public float XPositionOffset = 0.1f;
        [Min(0f)]
        public float YPositionOffset = 0.1f;
        [Min(0f)]
        public float ScaleOffset = 0f;
        public float RotationOffset = 0f;


        public TextEffect(string name, AnimationCurve xPosCurve, AnimationCurve yPosCurve, AnimationCurve scaleCurve, AnimationCurve rotationCurve, bool loop = false)
        {
            this.name = name;
            for (int i = 0; i < xPosCurve.keys.Length; i++)
            {
                xPosCurve.SmoothTangents(i, 0);
            }

            for (int i = 0; i < yPosCurve.keys.Length; i++)
            {
                yPosCurve.SmoothTangents(i, 0);
            }

            for (int i = 0; i <scaleCurve.keys.Length; i++)
            {
                scaleCurve.SmoothTangents(i, 0);
            }

            for (int i = 0; i < rotationCurve.keys.Length; i++)
            {
                rotationCurve.SmoothTangents(i, 0);
            }


            XPosAnimationCurve = xPosCurve;
            YPosAnimationCurve = yPosCurve;
            scaleAnimationCurve = scaleCurve;
            rotationAnimationCurve = rotationCurve;
            if (loop)
            {
                XPosAnimationCurve.preWrapMode = WrapMode.Loop;
                XPosAnimationCurve.postWrapMode = WrapMode.Loop;

                YPosAnimationCurve.preWrapMode = WrapMode.Loop;
                YPosAnimationCurve.postWrapMode = WrapMode.Loop;

                scaleAnimationCurve.preWrapMode = WrapMode.Loop;
                scaleAnimationCurve.postWrapMode = WrapMode.Loop;

                rotationAnimationCurve.preWrapMode = WrapMode.Loop;
                rotationAnimationCurve.postWrapMode = WrapMode.Loop;
            }
        }
    }
}
