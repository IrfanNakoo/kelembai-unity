using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

namespace Christina.GameSystems
{
    // Class to manage spawning and pooling of damage popups
    public class SpawnsDamagePopups : MonoBehaviour
    {
        // Singleton instance for global access
        public static SpawnsDamagePopups Instance { get; private set; }

        // Object pool to manage damage label instances
        private ObjectPool<DamageLabel> _damageLabelPopupPool;

        [Header("Damage Label Popup")]
        [SerializeField] private DamageLabel damageLabelPrefab; // Prefab for the damage label

        [Header("Display Setup")]
        [Range(0.8f, 1.5f), SerializeField] public float displayLength = 1f; // Duration for popup display
        private Camera _mainCamera; // Reference to the main camera

        private void Awake()
        {
            // Singleton setup: Ensure only one instance exists
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject); // Persist across scenes
            }
            else
            {
                Destroy(gameObject); // Destroy duplicate instances
                return;
            }

            // Initialize the object pool for damage labels
            _damageLabelPopupPool = new ObjectPool<DamageLabel>(
                // Object creation method: Instantiate a new DamageLabel and initialize it
                () =>
                {
                    DamageLabel damageLabel = Instantiate(damageLabelPrefab, transform);
                    damageLabel.Initialize(displayLength, this); // Pass display duration and reference to this manager
                    return damageLabel;
                },
                // OnGet: Activate the damage label when retrieved from the pool
                damageLabel => damageLabel.gameObject.SetActive(true),
                // OnRelease: Deactivate the damage label when returned to the pool
                damageLabel => damageLabel.gameObject.SetActive(false)
            );

            // Subscribe to scene load event to update the main camera reference
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDestroy()
        {
            // Unsubscribe from scene load event when this object is destroyed
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        // Called whenever a new scene is loaded
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // Update reference to the main camera in the new scene
            _mainCamera = Camera.main;
        }

        // Public method to create a damage popup
        public void DamageDone(int damage, Vector3 position, bool isCrit)
        {
            // Convert world position to screen position for UI placement
            Vector3 screenPosition = _mainCamera.WorldToScreenPoint(position);
            screenPosition.z = 0; // Ensure depth is 0 for UI elements

            // Determine direction of movement for the popup (left or right)
            bool direction = screenPosition.x < Screen.width * 0.5f;

            // Spawn the damage popup
            SpawnDamagePopup(damage, screenPosition, direction, isCrit);
        }

        // Private method to spawn a damage popup
        private void SpawnDamagePopup(int damage, Vector3 position, bool direction, bool isCrit)
        {
            // Get a damage label from the object pool
            DamageLabel damageLabel = _damageLabelPopupPool.Get();

            // Set up the damage label with the appropriate data
            damageLabel.Display(damage, position, direction, isCrit);
        }

        // Return a damage label back to the pool
        public void ReturnDamageLabelToPool(DamageLabel damageLabel3d)
        {
            // Release the damage label to the pool, deactivating it
            _damageLabelPopupPool.Release(damageLabel3d);
        }
    }
}