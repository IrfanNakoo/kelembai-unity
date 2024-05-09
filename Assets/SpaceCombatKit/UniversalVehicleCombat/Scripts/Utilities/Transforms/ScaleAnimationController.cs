using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VSX.Utilities
{
    /// <summary>
    /// Scale an object using an animation curve.
    /// </summary>
    public class ScaleAnimationController : AnimationController
    {

        [Tooltip("The transform being animated.")]
        [SerializeField]
        protected Transform animatedTransform;

        [Tooltip("The curve describing the scale over the normalized animation time (0 - 1).")]
        [SerializeField]
        protected AnimationCurve scaleCurve = AnimationCurve.Linear(0, 2, 1, 1);


        protected virtual void Reset()
        {
            animatedTransform = transform;
        }


        public override void SetAnimationPosition(float normalizedAnimationPosition)
        {
            base.SetAnimationPosition(normalizedAnimationPosition);
            float scale = scaleCurve.Evaluate(normalizedAnimationPosition);
            animatedTransform.localScale = new Vector3(scale, scale, scale);
        }
    }
}

