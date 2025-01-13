using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace VSX.UniversalVehicleCombat
{
    public class onDestroyedWave : MonoBehaviour
    {
        [Header("Game Objects to Track")]
        public List<GameObject> gameObjectsToTrack; // List of GameObjects to track

        [Header("Events")]
        public UnityEvent onAllObjectsInactive;     // Event triggered when all objects are inactive

        private bool allObjectsInactive = false;    // Flag to track if all objects are inactive

        void Update()
        {
            CheckObjectsStatus();
        }

        // This method checks if all game objects are inactive
        void CheckObjectsStatus()
        {
            if (allObjectsInactive) return; // If already triggered, no need to check again

            bool anyActive = false;

            // Iterate through the list and check if any object is still active
            foreach (GameObject obj in gameObjectsToTrack)
            {
                if (obj != null && obj.activeInHierarchy)
                {
                    anyActive = true;
                    break;
                }
            }

            // If none of the objects are active, trigger the event
            if (!anyActive)
            {
                allObjectsInactive = true; // Prevent the event from firing multiple times
                onAllObjectsInactive.Invoke(); // Trigger the Unity event
            }
        }

        // Optional: A method to reset the state for rechecking
        public void ResetWave()
        {
            allObjectsInactive = false;
        }
    }
}
