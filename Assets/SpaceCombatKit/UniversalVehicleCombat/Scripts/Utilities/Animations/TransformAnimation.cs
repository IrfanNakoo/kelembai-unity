using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VSX.Utilities;

namespace VSX.Utilities
{
    public class TransformAnimation : AnimationController
    {
        public Transform animatedTransform;

        protected Vector3 position;
        public Vector3 Position { get { return position; } }

        protected Quaternion rotation;
        public Quaternion Rotation { get { return rotation; } }

        public bool animatePosition = true;
        public Vector3 startPosition;
        public Vector3 endPosition;
        public AnimationCurve positionCurve;

        public bool animateRotation = true;
        public Vector3 startRotation;
        public Vector3 endRotation;
        public AnimationCurve rotationCurve;


        public override void SetAnimationPosition(float normalizedAnimationPosition)
        {
            base.SetAnimationPosition(normalizedAnimationPosition);

            if (animatePosition)
            {
                float val = positionCurve.Evaluate(normalizedAnimationPosition);
                animatedTransform.localPosition = val * endPosition + (1 - val) * startPosition;
            }
            if (animateRotation)
            {
                float val = rotationCurve.Evaluate(normalizedAnimationPosition);
                animatedTransform.localRotation = Quaternion.Slerp(Quaternion.Euler(startRotation), Quaternion.Euler(endRotation), val);
            }

        }
    }
}

