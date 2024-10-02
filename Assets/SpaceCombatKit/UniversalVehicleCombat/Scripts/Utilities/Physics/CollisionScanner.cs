using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace VSX.UniversalVehicleCombat
{
    [System.Serializable]
    public class OnCollisionScannerHitDetectedEventHandler : UnityEvent<RaycastHit> { }

    public class CollisionScanner : MonoBehaviour, IRootTransformUser
    {
        [Header("Settings")]
        [SerializeField] protected LayerMask hitMask = Physics.DefaultRaycastLayers;
        public LayerMask HitMask { get { return hitMask; } }

        [SerializeField] protected HitScanIntervalType hitScanIntervalType = HitScanIntervalType.FrameInterval;
        [SerializeField] protected int hitScanFrameInterval = 1;
        protected int frameCountSinceLastScan = 1;

        [SerializeField] protected float hitScanTimeInterval;
        protected float lastHitScanTime;
        protected Vector3 lastPosition;

        [SerializeField] protected bool ignoreTriggerColliders = false;
        [SerializeField] protected bool ignoreHierarchyCollision = true;
        [SerializeField] protected Transform rootTransform;
        public Transform RootTransform { set { rootTransform = value; } }

        [SerializeField] protected Rigidbody m_Rigidbody;
        public Rigidbody Rigidbody { get { return m_Rigidbody; } set { m_Rigidbody = value; } }

        [Header("Events")]
        public OnCollisionScannerHitDetectedEventHandler onHitDetected;
        protected bool disabled = false;

        protected virtual void Reset() { m_Rigidbody = GetComponent<Rigidbody>(); }

        private void OnEnable() { disabled = false; lastPosition = transform.position; }

        // Do a single hit scan
        protected void DoHitScan()
        {
            if (disabled) return;

            RaycastHit[] hits;
            float scanDistance = Vector3.Distance(lastPosition, transform.position);

            hits = Physics.RaycastAll(lastPosition, transform.forward, scanDistance, hitMask, ignoreTriggerColliders ? QueryTriggerInteraction.Ignore : QueryTriggerInteraction.Collide);
            System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

            for (int i = 0; i < hits.Length; ++i)
            {
                if (ignoreHierarchyCollision && hits[i].transform.IsChildOf(rootTransform))
                    continue;

                // Move the projectile to the hit point
                transform.position = hits[i].point;

                // Check if the hit object is tagged as "Enemy"
                if (hits[i].collider.CompareTag("Enemy"))
                {
                    // Try to get the ScoringOnEnemy component
                    ScoringOnEnemy enemy = hits[i].collider.GetComponent<ScoringOnEnemy>();

                    if (enemy != null)
                    {
                        // Call the TakeDamage method
                        enemy.TakeDamage();

                        // Log the scoring event
                        Debug.Log("Projectile hit an enemy: " + hits[i].collider.gameObject.name);
                    }
                    else
                    {
                        Debug.LogWarning("Enemy component not found on object tagged 'Enemy': " + hits[i].collider.gameObject.name);
                    }

                    // Destroy the projectile after hitting the enemy
                    Debug.Log("Projectile destroyed.");
                    Destroy(gameObject);
                    return;
                }

                // If hit something other than an enemy, just log it
                Debug.Log("Projectile hit a non-enemy object: " + hits[i].collider.gameObject.name);

                disabled = true;
                onHitDetected.Invoke(hits[i]);
                break;
            }

            lastPosition = transform.position;
        }

        public void SetHitScanDisabled() { disabled = true; }

        public void SetHitScanEnabled() { disabled = false; }

        private void Update()
        {
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

            frameCountSinceLastScan += 1;
        }
    }
}
