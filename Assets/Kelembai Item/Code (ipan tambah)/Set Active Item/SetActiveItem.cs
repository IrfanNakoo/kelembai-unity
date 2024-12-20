using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SetActiveItem : MonoBehaviour
{
    [SerializeField] private GameObject objectToActivate;     // Object to activate
    [SerializeField] private GameObject objectToDeactivate;   // Object to deactivate
    [SerializeField] private UnityEvent onActivationComplete; // Event called when activation is complete

    private bool isDeactivated = false; // Tracks if the objectToDeactivate has been deactivated

    // Method to deactivate and activate objects
    public void ToggleActivation()
    {
        if (!isDeactivated && objectToDeactivate != null)
        {
            objectToDeactivate.SetActive(false); // Deactivate the target object
            isDeactivated = true;                // Mark as deactivated
        }

        if (objectToActivate != null)
        {
            objectToActivate.SetActive(true);    // Activate the target object
            Debug.Log($"{nameof(SetActiveItem)}: {objectToActivate.name} is now active.");
        }

        // Call the UnityEvent after activation
        onActivationComplete?.Invoke();
    }
}