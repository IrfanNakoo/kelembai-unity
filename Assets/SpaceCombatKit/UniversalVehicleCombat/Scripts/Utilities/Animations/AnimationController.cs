using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace VSX.Utilities
{
    /// <summary>
    /// Base class for a custom animation controller.
    /// </summary>
    public class AnimationController : MonoBehaviour
    {
        [Tooltip("Whether to start the animation when the gameobject is enabled in the scene.")]
        [SerializeField]
        protected bool startOnEnable = true;

        [Tooltip("Whether to loop the animation.")]
        [SerializeField]
        protected bool loop = false;

        [SerializeField]
        protected bool limitLoopCount = false;

        [Tooltip("How many times to loop the animation once it is started.")]
        [SerializeField]
        protected int loopCount = 1;
        protected int currentLoopCount = 0;

        [Tooltip("The length of one cycle of the animation.")]
        [SerializeField]
        protected float animationLength = 3;
        public float AnimationLength
        {
            get { return animationLength; }
            set { animationLength = value; }
        }

        [Tooltip("The normalized starting position of the animation.")]
        [Range(0, 1)]
        [SerializeField]
        protected float startingAnimationPosition = 0;

        [SerializeField]
        protected bool resetToStartingPositionOnEnable = true;

        [SerializeField]
        protected bool resetToStartingPositionOnFinish = true;

        [Tooltip("Whether this animation is updates by itself (uncheck if animation is being controlled externally.)")]
        [SerializeField]
        protected bool selfUpdate = true;

        protected float currentAnimationDelay;

        protected float animationStartTime;

        protected bool animating;
        public virtual bool Animating { get { return animating; } }

        public UnityEvent onAnimationStarted;
        public UnityEvent onAnimationStopped;



        protected virtual void OnEnable()
        {
            if (resetToStartingPositionOnEnable)
            {
                ResetAnimation();
            }

            if (startOnEnable)
            {
                StartAnimation();
            }
        }



        /// <summary>
        /// Reset the animation to the starting position.
        /// </summary>
        public virtual void ResetAnimation()
        {
            SetAnimationPosition(startingAnimationPosition);
        }



        /// <summary>
        /// Start the animation.
        /// </summary>
        public virtual void StartAnimation()
        {
            if (animating) return;

            StartAnimationDelayed(0);
        }



        /// <summary>
        /// Start the animation with a delay.
        /// </summary>
        /// <param name="delay">The animation delay.</param>
        public virtual void StartAnimationDelayed(float delay)
        {
            if (animating) return;

            currentAnimationDelay = delay;
            animating = true;
            animationStartTime = Time.time;
            currentLoopCount = 0;
            onAnimationStarted.Invoke();
        }



        /// <summary>
        /// Stop animation.
        /// </summary>
        /// <param name="reset">Whether to reset the animation to the starting position.</param>
        public virtual void StopAnimation(bool reset = true)
        {
            if (!animating) return;

            animating = false;

            if (reset) ResetAnimation();

            onAnimationStopped.Invoke();
        }



        /// <summary>
        /// Set the normalized animation position (0 - 1).
        /// </summary>
        /// <param name="normalizedAnimationPosition">The normalized animation position (0 - 1).</param>
        public virtual void SetAnimationPosition(float normalizedAnimationPosition) { }



        /// <summary>
        /// Set the animation time (not normalized).
        /// </summary>
        /// <param name="animationTime">The animation time (may be any value).</param>
        public virtual void SetAnimationTime(float animationTime) 
        {
            float normalizedPosition = loop ? (animationTime % animationLength) / animationLength : Mathf.Clamp(animationTime / animationLength, 0, 1);
            
            SetAnimationPosition(normalizedPosition);

        }



        // Called every frame.
        protected virtual void Update()
        {
            if (animating && selfUpdate)
            {

                float animationTime = Mathf.Max(Time.time - animationStartTime - currentAnimationDelay, 0);

                if (loop)
                {
                    int numLoops = Mathf.FloorToInt(animationTime / animationLength);
                    if (limitLoopCount && currentLoopCount >= loopCount)
                    {
                        SetAnimationTime(animationLength);
                        StopAnimation();
                    }
                    else
                    {
                        SetAnimationTime(animationTime);
                    }
                }
                else
                {
                    if (animationTime > animationLength)
                    {
                        SetAnimationTime(animationLength);
                        StopAnimation(resetToStartingPositionOnFinish);
                    }
                    else
                    {
                        SetAnimationTime(animationTime);
                    }
                }
            }
        }
    }
}

