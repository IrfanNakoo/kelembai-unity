﻿//Still testing

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace VSX.UniversalVehicleCombat
{
    /// <summary>
    /// Input script for controlling the steering and movement of a space fighter vehicle.
    /// </summary>
    public class PlayerInput_InputSystem_SpaceshipControls : VehicleInput
    {
        protected SCKInputAsset spaceshipInput;
        protected GeneralInputAsset generalInput;

        protected float acceleration, roll;
        protected Vector2 steering, strafing;

        [Header("Control Scheme")]
        [Tooltip("Whether the vehicle should yaw when rolling.")]
        [SerializeField]
        protected bool linkYawAndRoll = false;

        [Tooltip("How much the vehicle should yaw when rolling.")]
        [SerializeField]
        protected float yawRollRatio = 1;

        [Header("Auto Roll")]
        [SerializeField]
        protected bool autoRollEnabled = true;

        [SerializeField]
        protected float autoRollStrength = 0.04f;

        [SerializeField]
        protected float maxAutoRoll = 0.2f;

        protected float lastRollTime;

        [Header("Mouse Steering")]
        [SerializeField]
        protected bool mouseEnabled = true;
        public bool MouseEnabled
        {
            get { return mouseEnabled; }
            set { mouseEnabled = value; }
        }

        [SerializeField]
        protected MouseSteeringType mouseSteeringType;
        public MouseSteeringType MouseSteeringType
        {
            get { return mouseSteeringType; }
            set { mouseSteeringType = value; }
        }

        [Tooltip("Invert the vertical mouse input.")]
        [SerializeField]
        protected bool mouseVerticalInverted = false;

        [Tooltip("Invert the horizontal mouse input.")]
        [SerializeField]
        protected bool mouseHorizontalInverted = false;

        [Header("Mouse Screen Position Settings")]
        [Tooltip("The fraction of the viewport (based on the screen width) around the screen center inside which the mouse position does not affect the ship steering.")]
        [SerializeField]
        protected float mouseDeadRadius = 0.1f;

        [Tooltip("How far the mouse reticule is allowed to get from the screen center.")]
        [SerializeField]
        protected float maxReticleDistanceFromCenter = 0.475f;

        [SerializeField]
        protected float reticleMovementSpeed = 1;

        [Tooltip("How much the ship pitches (local X axis rotation) based on the mouse distance from screen center.")]
        [SerializeField]
        protected AnimationCurve mousePositionInputCurve = AnimationCurve.Linear(0, 0, 1, 1);

        [SerializeField]
        protected bool centerCursorOnInputEnabled = true;

        [Header("Mouse Delta Position Settings")]
        [SerializeField]
        protected float mouseDeltaPositionSensitivity = 0.75f;

        [SerializeField]
        protected AnimationCurve mouseDeltaPositionInputCurve = AnimationCurve.Linear(0, 0, 1, 1);

        [Header("Keyboard Steering")]
        [Tooltip("Invert the pitch (local X rotation) input.")]
        [SerializeField]
        protected bool nonMouseVerticalInverted = false;

        [Tooltip("Invert the pitch (local X rotation) input.")]
        [SerializeField]
        protected bool nonMouseHorizontalInverted = false;

        [Header("Throttle")]
        [SerializeField]
        protected bool setThrottle = false;

        [SerializeField]
        protected float throttleSensitivity = 1;

        protected Vector3 mouseSteeringInputs = Vector3.zero;
        protected Vector3 steeringInputs = Vector3.zero;

        protected Vector3 movementInputs = Vector3.zero;
        protected Vector3 boostInputs = Vector3.zero;

        protected bool steeringEnabled = true;
        protected bool movementEnabled = true;

        [Header("Boost")]
        [SerializeField]
        protected float boostChangeSpeed = 3;
        protected Vector3 boostTarget = Vector3.zero;

        protected VehicleEngines3D spaceVehicleEngines;

        protected HUDCursor hudCursor;
        protected Vector3 reticuleViewportPosition = new Vector3(0.5f, 0.5f, 0);

        protected override void Awake()
        {
            base.Awake();

            generalInput = new GeneralInputAsset();
            spaceshipInput = new SCKInputAsset();

            spaceshipInput.SpacefighterControls.Steer.performed += ctx => steering = ctx.ReadValue<Vector2>();
            spaceshipInput.SpacefighterControls.Strafe.performed += ctx => strafing = ctx.ReadValue<Vector2>();
            spaceshipInput.SpacefighterControls.Roll.performed += ctx => GetRollInput(ctx.ReadValue<float>());
            spaceshipInput.SpacefighterControls.Throttle.performed += ctx => acceleration = ctx.ReadValue<float>();
            spaceshipInput.SpacefighterControls.Boost.performed += ctx => boostTarget.z = ctx.ReadValue<float>();
        }

        protected void GetRollInput(float rollAmount)
        {
            roll = rollAmount;
        }

        private void OnEnable()
        {
            generalInput.Enable();
            spaceshipInput.Enable();
        }

        private void OnDisable()
        {
            generalInput.Disable();
            spaceshipInput.Disable();
        }

        protected override bool Initialize(Vehicle vehicle)
        {
            if (!base.Initialize(vehicle)) return false;

            spaceVehicleEngines = vehicle.GetComponent<VehicleEngines3D>();
            hudCursor = vehicle.GetComponentInChildren<HUDCursor>();

            if (spaceVehicleEngines == null)
            {
                if (debugInitialization)
                {
                    Debug.LogWarning(GetType().Name + " failed to initialize - the required " + spaceVehicleEngines.GetType().Name + " component was not found on the vehicle.");
                }
                return false;
            }

            if (debugInitialization)
            {
                Debug.Log(GetType().Name + " component successfully initialized.");
            }

            return true;
        }

        public override void EnableInput()
        {
            base.EnableInput();

            if (centerCursorOnInputEnabled && hudCursor != null)
            {
                hudCursor.CenterCursor();
            }
        }

        public override void DisableInput()
        {
            base.DisableInput();
            reticuleViewportPosition = new Vector3(0.5f, 0.5f, 0);
        }

        public virtual void EnableSteering()
        {
            steeringEnabled = true;
        }

        public virtual void DisableSteering(bool clearCurrentValues)
        {
            steeringEnabled = false;

            if (clearCurrentValues)
            {
                steeringInputs = Vector3.zero;
                spaceVehicleEngines.SetSteeringInputs(steeringInputs);
            }
        }

        public virtual void EnableMovement()
        {
            movementEnabled = true;
        }

        public virtual void DisableMovement(bool clearCurrentValues)
        {
            movementEnabled = false;

            if (clearCurrentValues)
            {
                movementInputs = Vector3.zero;
                spaceVehicleEngines.SetMovementInputs(movementInputs);

                boostTarget = Vector3.zero;
                boostInputs = Vector3.zero;
                spaceVehicleEngines.SetBoostInputs(boostInputs);
            }
        }

        protected void UpdateReticulePosition(Vector3 mouseDelta)
        {
            if (mouseSteeringType == MouseSteeringType.ScreenPosition)
            {
                reticuleViewportPosition += new Vector3(mouseDelta.x / Screen.width, mouseDelta.y / Screen.height, 0) * reticleMovementSpeed;

                Vector3 centeredReticuleViewportPosition = reticuleViewportPosition - new Vector3(0.5f, 0.5f, 0);
                centeredReticuleViewportPosition.x *= (float)Screen.width / Screen.height;

                centeredReticuleViewportPosition = Vector3.ClampMagnitude(centeredReticuleViewportPosition, maxReticleDistanceFromCenter);
                centeredReticuleViewportPosition.x /= (float)Screen.width / Screen.height;

                reticuleViewportPosition = centeredReticuleViewportPosition + new Vector3(0.5f, 0.5f, 0);
            }
            else if (mouseSteeringType == MouseSteeringType.DeltaPosition)
            {
                reticuleViewportPosition = new Vector3(0.5f, 0.5f, 0);
            }
        }

        protected virtual void MouseSteeringUpdate()
        {
            mouseSteeringInputs = Vector3.zero;

            if (!mouseEnabled) return;

            Vector3 screenInputs = Vector3.zero;
            if (mouseSteeringType == MouseSteeringType.ScreenPosition)
            {
                Vector3 centeredViewportPos = reticuleViewportPosition - new Vector3(0.5f, 0.5f, 0);

                centeredViewportPos.x *= (float)Screen.width / Screen.height;

                float amount = Mathf.Max(centeredViewportPos.magnitude - mouseDeadRadius, 0) / (maxReticleDistanceFromCenter - mouseDeadRadius);

                centeredViewportPos.x /= (float)Screen.width / Screen.height;

                screenInputs = mousePositionInputCurve.Evaluate(amount) * centeredViewportPos.normalized;
            }
            else if (mouseSteeringType == MouseSteeringType.DeltaPosition)
            {
                screenInputs = mouseDeltaPositionSensitivity * generalInput.GeneralControls.MouseDelta.ReadValue<Vector2>();
                screenInputs = Mathf.Clamp(mouseDeltaPositionInputCurve.Evaluate(screenInputs.magnitude), 0, 1) * screenInputs.normalized;
            }

            mouseSteeringInputs.x = -screenInputs.y;
            mouseSteeringInputs.y = screenInputs.x;

            mouseSteeringInputs.x *= (mouseVerticalInverted ? -1 : 1);
            mouseSteeringInputs.y *= (mouseHorizontalInverted ? -1 : 1);

            if (hudCursor != null)
            {
                hudCursor.SetViewportPosition(reticuleViewportPosition);
            }
        }

        protected virtual void MovementUpdate()
        {
            if (InputMechanic.isInputActive) return;

            Vector3 movementInputs = spaceVehicleEngines.MovementInputs;

            if (setThrottle)
            {
                movementInputs.z = acceleration;
            }
            else
            {
                movementInputs.z += acceleration * throttleSensitivity * Time.deltaTime;
            }

            movementInputs.x = strafing.x;
            movementInputs.y = strafing.y;

            boostInputs = Vector3.Lerp(boostInputs, boostTarget, boostChangeSpeed * Time.deltaTime);
            if (boostInputs.magnitude < 0.0001f) boostInputs = Vector3.zero;

            if (boostTarget.z > 0.5f) movementInputs.z = 1;

            if (movementEnabled) spaceVehicleEngines.SetMovementInputs(movementInputs);
            spaceVehicleEngines.SetBoostInputs(boostInputs);
        }

        protected virtual void OnRoll(float rollAmount)
        {
            if (Mathf.Abs(rollAmount) > 0.0001f)
            {
                lastRollTime = Time.time;
            }
        }

        public void SetBoost(float boostAmount)
        {
            boostTarget = new Vector3(0f, 0f, boostAmount);
        }

        protected void AutoRoll()
        {
            if (Time.time - lastRollTime < 0.5f) return;

            Vector3 flattenedFwd = spaceVehicleEngines.transform.forward;
            flattenedFwd.y = 0;
            flattenedFwd.Normalize();

            Vector3 right = Vector3.Cross(Vector3.up, flattenedFwd);

            float angle = Vector3.Angle(right, spaceVehicleEngines.transform.right);

            if (Vector3.Dot(spaceVehicleEngines.transform.up, right) > 0)
            {
                angle *= -1;
            }

            steeringInputs.z = Mathf.Clamp(angle * -1 * autoRollStrength, -1, 1);
            steeringInputs.z *= maxAutoRoll;
            steeringInputs.z *= 1 - Mathf.Abs(Vector3.Dot(spaceVehicleEngines.transform.forward, Vector3.up));
        }

        protected override void InputUpdate()
        {
            if (InputMechanic.isInputActive) return;

            steeringInputs.x = Mathf.Clamp((nonMouseVerticalInverted ? -1 : 1) * -steering.y, -1, 1);
            steeringInputs.z = Mathf.Clamp(roll, -1, 1);
            steeringInputs.y = Mathf.Clamp((nonMouseHorizontalInverted ? -1 : 1) * steering.x, -1f, 1f);

            UpdateReticulePosition(generalInput.GeneralControls.MouseDelta.ReadValue<Vector2>());
            MouseSteeringUpdate();

            steeringInputs = new Vector3(Mathf.Abs(steeringInputs.x) > Mathf.Abs(mouseSteeringInputs.x) ? steeringInputs.x : mouseSteeringInputs.x,
                                         Mathf.Abs(steeringInputs.y) > Mathf.Abs(mouseSteeringInputs.y) ? steeringInputs.y : mouseSteeringInputs.y,
                                         Mathf.Abs(steeringInputs.z) > Mathf.Abs(mouseSteeringInputs.z) ? steeringInputs.z : mouseSteeringInputs.z);

            if (Mouse.current == null || !mouseEnabled)
            {
                hudCursor.CenterCursor();
                reticuleViewportPosition = new Vector3(0.5f, 0.5f, 0);
            }

            if (linkYawAndRoll)
            {
                steeringInputs.z = Mathf.Clamp(-steeringInputs.y * yawRollRatio, -1f, 1f);
            }

            OnRoll(steeringInputs.z);

            MovementUpdate();

            if (autoRollEnabled) AutoRoll();

            spaceVehicleEngines.SetSteeringInputs(steeringInputs);
        }
    }
}

