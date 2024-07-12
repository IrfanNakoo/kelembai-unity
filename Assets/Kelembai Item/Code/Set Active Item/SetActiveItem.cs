using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SetActiveItem : MonoBehaviour
{
    public GameObject objectToActivate;
    public GameObject objectToDeactivate;
    public float activationDelay = 5f; // Delay in seconds before activating the object
    public UnityEvent onActivationComplete;

    public VSX.UniversalVehicleCombat.Timer timer; // Assuming Timer is in the VSX.UniversalVehicleCombat namespace

    void Start()
    {
        if (timer != null)
        {
            // Start the timer with a delay
            timer.StartTimerDelayed(activationDelay);

            // Add listener for the timer's finished event
            timer.onTimerFinished.AddListener(ActivateObject);
        }
        else
        {
            Debug.LogError("Timer is not assigned.");
        }
    }

    // Method to activate the object
    public void ActivateObject()
    {
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(true);
            Debug.Log("This GameObject is active in Update.");
        }
        else
        {
            Debug.LogWarning("Object to activate is not assigned.");
        }

        // Call the UnityEvent to notify listeners that activation is complete
        onActivationComplete.Invoke();
    }

    public void DeactivateObject()
    {
        if (objectToDeactivate != null)
        {
            objectToDeactivate.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Object to deactivate is not assigned.");
        }
    }
}
