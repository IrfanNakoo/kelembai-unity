using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace VSX.UniversalVehicleCombat
{
    /// <summary>
    /// Unity event for running functions when a raycast hit is detected
    /// </summary>
    [System.Serializable]
    public class OnCollisionScannerHitDetectedEventHandler : UnityEvent<RaycastHit> { }

    /// <summary>
    /// This class uses a raycast from the transform's previous position to its current one to detect a hit on a collider regardless of speed.
    /// </summary>
    public class CollisionScanner : MonoBehaviour, IRootTransformUser
    {

        [Header("Settings")]

        [SerializeField]
        protected LayerMask hitMask = Physics.DefaultRaycastLayers;
        public LayerMask HitMask { get { return hitMask; } }

        [SerializeField]
        protected HitScanIntervalType hitScanIntervalType = HitScanIntervalType.FrameInterval;

        // Frame interval

        [SerializeField]
        protected int hitScanFrameInterval = 1;
        protected int frameCountSinceLastScan = 1;

        // Time interval

        [SerializeField]
        protected float hitScanTimeInterval;
        protected float lastHitScanTime;

        protected Vector3 lastPosition;

        [Tooltip("Whether to ignore trigger colliders when scanning for collisions.")]
        [SerializeField]
        protected bool ignoreTriggerColliders = false;

        [Tooltip("Whether to ignore collision with the object or vehicle that this object came from.")]
        [SerializeField]
        protected bool ignoreHierarchyCollision = true;

        [SerializeField]
        protected Transform rootTransform;    // To check for collisions with firer
        public Transform RootTransform { set { rootTransform = value; } }

        [SerializeField]
        protected Rigidbody m_Rigidbody;
        public Rigidbody Rigidbody
        {
            get { return m_Rigidbody; }
            set { m_Rigidbody = value; }
        }

        [Header("Events")]

        // Hit detected event
        public OnCollisionScannerHitDetectedEventHandler onHitDetected;

        protected bool disabled = false;


        protected virtual void Reset()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
        }

        // Reset when enabled
        private void OnEnable()
        {
            disabled = false;
            lastPosition = transform.position;

            VSX.FloatingOriginSystem.FloatingOriginObject obj = GetComponent<VSX.FloatingOriginSystem.FloatingOriginObject>();
            if (obj != null)
            {
                obj.onPostOriginShift.AddListener(UpdateLastPos);
            }
        }

        void UpdateLastPos()
        {
            lastPosition = transform.position;
        }

        /* Do a single hit scan
        protected void DoHitScan()
        {

            if (disabled) return;

            RaycastHit[] hits;

            // Scan from previous position to current position
            float scanDistance = Vector3.Distance(lastPosition, transform.position);

            // Raycast
            hits = Physics.RaycastAll(lastPosition, transform.forward, scanDistance, hitMask, ignoreTriggerColliders ? QueryTriggerInteraction.Ignore : QueryTriggerInteraction.Collide);
            System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));    // Sort by distance

            for (int i = 0; i < hits.Length; ++i)
            {
                if (ignoreHierarchyCollision && hits[i].transform.IsChildOf(rootTransform))
                {
                    continue;
                }

                transform.position = hits[i].point;

                disabled = true;
                onHitDetected.Invoke(hits[i]);

                break;

            }

            // Update the last position
            lastPosition = transform.position;

        }*/

        /// <summary>
        /// Disable this collision scanner.
        /// </summary>
        public void SetHitScanDisabled()
        {
            disabled = true;
        }

        /// <summary>
        /// Enable this collision scanner
        /// </summary>
        public void SetHitScanEnabled()
        {
            disabled = false;
        }

        /* Called every frame
        private void Update()
        {

            switch (hitScanIntervalType)
            {
                case HitScanIntervalType.FrameInterval:

                    // Check if enough frames have passed for a new scan
                    if (frameCountSinceLastScan >= hitScanFrameInterval)
                    {
                        DoHitScan();
                        frameCountSinceLastScan = 0;
                    }

                    break;

                case HitScanIntervalType.TimeInterval:

                    // Check if enough time has passed for a new scan
                    if ((Time.time - lastHitScanTime) > hitScanTimeInterval)
                    {
                        DoHitScan();
                        lastHitScanTime = Time.time;
                    }

                    break;

            }

            frameCountSinceLastScan += 1;

        }*/



        //
        //Tambahan
        //

        // New variables
        private int shotsFired = 0;    // Tracks how many shots have been fired
        private int shotsHit = 0;      // Tracks successful hits
        private float accuracy = 0f;   // Accuracy percentage

        void Update()
        {
            // Original update for hit scan logic
            frameCountSinceLastScan += 1;

            switch (hitScanIntervalType)
            {
                case HitScanIntervalType.FrameInterval:

                    if (frameCountSinceLastScan >= hitScanFrameInterval)
                    {
                        DoHitScan();
                        frameCountSinceLastScan = 0;
                    }
                    break;

                case HitScanIntervalType.TimeInterval:

                    if ((Time.time - lastHitScanTime) > hitScanTimeInterval)
                    {
                        DoHitScan();
                        lastHitScanTime = Time.time;
                    }
                    break;
            }

            // Detect when the player presses the fire button (e.g., "Fire1")
            if (Input.GetButtonDown("Fire1"))
            {
                ShotFired();
                Debug.Log("Fire button pressed. Shot fired.");
            }
        }

        // Method to increment shots fired
        private void ShotFired()
        {
            shotsFired++;
            Debug.Log("Shots Fired: " + shotsFired);
        }

        // Do a single hit scan
        protected void DoHitScan()
        {
            if (disabled) return;

            RaycastHit[] hits;

            float scanDistance = Vector3.Distance(lastPosition, transform.position);

            hits = Physics.RaycastAll(lastPosition, transform.forward, scanDistance, hitMask, ignoreTriggerColliders ? QueryTriggerInteraction.Ignore : QueryTriggerInteraction.Collide);
            System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));    // Sort by distance

            for (int i = 0; i < hits.Length; ++i)
            {
                if (ignoreHierarchyCollision && hits[i].transform.IsChildOf(rootTransform))
                {
                    continue;
                }

                // Check if the hit object has the tag "Enemy"
                if (hits[i].collider.CompareTag("EnemyCapitalShip"))
                {
                    ShotHit();  // Increment shots hit if an enemy was hit
                    Debug.Log("Enemy hit!");
                }

                transform.position = hits[i].point;

                disabled = true;
                onHitDetected.Invoke(hits[i]);
                break;
            }

            lastPosition = transform.position;
        }

        // Method to increment shots hit
        private void ShotHit()
        {
            shotsHit++;
            Debug.Log("Shots Hit: " + shotsHit);
            UpdateAccuracy();  // Update accuracy after each hit
        }

        // Updates the accuracy based on shots fired and successful hits
        private void UpdateAccuracy()
        {
            if (shotsFired > 0)
            {
                accuracy = ((float)shotsHit / shotsFired) * 100f;
                Debug.Log("Accuracy: " + accuracy.ToString("F2") + "%");
            }
            else
            {
                Debug.Log("Accuracy: N/A");
            }
        }

        // Save accuracy to PlayerPrefs
        private void SaveAccuracy()
        {
            PlayerPrefs.SetFloat("PlayerAccuracy", accuracy);
            PlayerPrefs.Save();
            Debug.Log("Accuracy saved to PlayerPrefs: " + accuracy);
        }

        // Call this method when the game ends or when you want to store accuracy
        public void EndLevel()
        {
            SaveAccuracy();
            Debug.Log("Level ended. Accuracy saved.");
        }
    }
}