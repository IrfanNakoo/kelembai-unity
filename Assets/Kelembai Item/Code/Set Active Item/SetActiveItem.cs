using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SetActiveItem : MonoBehaviour
{
    public GameObject objectToActivate;
    public float activationDelay = 5f; // Delay in seconds before activating the object
    public UnityEvent onActivationComplete;

    public VSX.UniversalVehicleCombat.Timer timer; // Assuming Timer is in the VSX.UniversalVehicleCombat namespace

    void Start()
    {
        // Start the timer
        timer.StartTimerDelayed(activationDelay);

        // Add listener for the timer's finished event
        timer.onTimerFinished.AddListener(ActivateObject);
    }

    // Method to activate the object
    public void ActivateObject()
    {
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(true);
        }

        // Call the UnityEvent to notify listeners that activation is complete
        onActivationComplete.Invoke();
    }
}