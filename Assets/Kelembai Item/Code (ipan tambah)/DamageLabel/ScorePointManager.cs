/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorePointManager : MonoBehaviour
{
    // Start is called before the first frame update
    // SCRIPT NIE DUDUK DALAM ENEMY SCORE MANAGERs
    // SCRIPT NIE CONTROL SCOREPOINT UNTUK AKTIF/TAK AKTIF
    // SCRIPT NIE AKAN TUNGGU SIGNAL DARI COLLISION SCANNER UNUTK AKTIF

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }



    public static ScorePointManager Instance { get; private set; }
    private int score = 0;

    public 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddScore(int points)
    {
        score += points;
        Debug.Log("Score Updated: " + score);
    }

    public int GetScore()
    {
        return score;
    }
}
*/
/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace VSX.UniversalVehicleCombat
{
    /// <summary>
    /// Unity event for running functions when a raycast hit is detected.
    /// </summary>
    [System.Serializable]
    public class OnCollisionScannerHitDetectedEventHandler : UnityEvent<RaycastHit> { }

    /// <summary>
    /// This class uses a raycast from the transform's previous position to its current one to detect hits on colliders regardless of speed.
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

        //////////////////////////////////////////////////////////////////////

        [Header("Damage Settings")]
        [SerializeField] private int critChance = 10; // Critical hit chance percentage
        [SerializeField] private int critMultiplier = 2; // Critical damage multiplier
        [SerializeField] private int minDamage = 10; // Minimum damage
        [SerializeField] private int maxDamage = 30; // Maximum damage

        //////////////////////////////////////////////////////////////////////

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

        /// <summary>
        /// Perform a single hit scan.
        /// </summary>
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
                // Ignore hits on the object itself
                if (ignoreHierarchyCollision && hits[i].transform.IsChildOf(rootTransform))
                {
                    continue;
                }

                // Check if the hit object has the tag "EnemyCapitalShip"
                if (hits[i].collider.CompareTag("EnemyCapitalShip"))
                {
                    /////////////////////////////////////////////////////////
                    ProcessEnemyHit(hits[i]);  // Process the enemy hit logic
                    transform.position = hits[i].point; // Set position at hit point
                    disabled = true;
                    onHitDetected.Invoke(hits[i]);
                    break;
                }

                /* Check if the hit object has the tag "EnemyCapitalShip"///////////////////////////
                if (hits[i].collider.CompareTag("Enemy"))
                {
                    ProcessEnemyHit(hits[i]);  // Process the enemy hit logic
                    transform.position = hits[i].point; // Set position at hit point
                    disabled = true;
                    onHitDetected.Invoke(hits[i]);
                    break;
                    //////////////////////////////////////////////////////////////////////////// ///breaker kt sini
            }

            // Update the last position
            lastPosition = transform.position;
        }


        /// <summary>
        /// Process logic for hitting an enemy.
        /// </summary>
        /// <param name="hit">Raycast hit information.</param>
        private void ProcessEnemyHit(RaycastHit hit)
        {
            Debug.Log("Enemy hit!");

            // Generate random damage
            int damage = Random.Range(minDamage, maxDamage);

            // Determine if the hit is a critical hit
            bool isCrit = Random.Range(0, 100) < critChance;
            if (isCrit)
            {
                damage *= critMultiplier; // Apply critical damage multiplier
            }

            // Trigger the damage popup
            if (SpawnsDamagePopups.Instance != null) // Ensure the singleton instance exists
            {
                SpawnsDamagePopups.Instance.DamageDone(damage, hit.point, isCrit);
            }
            else
            {
                Debug.LogWarning("SpawnsDamagePopups.Instance is not set!");
            }

            // Increment hit count for debugging purposes
            IncrementShotsHit();
        }

        /// <summary>
        /// Increment and log the number of successful shots.
        /// </summary>
        private void IncrementShotsHit()
        {
            shotsHit++;
            Debug.Log($"Shots Hit: {shotsHit}");
        }

        // Tracks successful hits
        private int shotsHit = 0;

        /// <summary>
        /// Disable this collision scanner.
        /// </summary>
        public void SetHitScanDisabled()
        {
            disabled = true;
        }

        /// <summary>
        /// Enable this collision scanner.
        /// </summary>
        public void SetHitScanEnabled()
        {
            disabled = false;
        }

        // Called every frame
        private void Update()
        {
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
        }
    }
}
*/