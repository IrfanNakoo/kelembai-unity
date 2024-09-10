using UnityEngine;
using UnityEngine.Events;

public class SetActiveItem : MonoBehaviour
{
    [SerializeField] private GameObject objectToActivate;
    [SerializeField] private GameObject objectToDeactivate;
    [SerializeField] private UnityEvent onActivationComplete;

    private bool isDeactivated = false; // Tracks if the objectToDeactivate has been deactivated

    private void Update()
    {
        // Check if objectToDeactivate is inactive and objectToActivate is not yet active
        if (!isDeactivated && objectToDeactivate != null && !objectToDeactivate.activeSelf)
        {
            isDeactivated = true;
            ActivateObject();
        }
    }

    // Method to activate the object
    private void ActivateObject()
    {
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(true);
            Debug.Log($"{nameof(SetActiveItem)}: {objectToActivate.name} is now active.");
        }

        // Call the UnityEvent to notify listeners that activation is complete
        onActivationComplete?.Invoke();
    }

    private void OnDestroy()
    {
        // Cleanup if needed
    }
}



//noted
/*
using UnityEngine;
using UnityEngine.Events;

public class SetActiveItem : MonoBehaviour
{
    [SerializeField] private GameObject objectToActivate;
    [SerializeField] private GameObject objectToDeactivate;
    [SerializeField] private float activationDelay = 5f; // Delay in seconds before activating the object
    [SerializeField] private UnityEvent onActivationComplete;

    [SerializeField] private VSX.UniversalVehicleCombat.Timer timer; // Assuming Timer is in the VSX.UniversalVehicleCombat namespace

    private void Start()
    {
        // Validate inputs
        if (timer == null)
        {
            Debug.LogError($"{nameof(SetActiveItem)}: Timer is not assigned.");
            return;
        }

        if (objectToActivate == null && objectToDeactivate == null)
        {
            Debug.LogWarning($"{nameof(SetActiveItem)}: Neither objectToActivate nor objectToDeactivate is assigned.");
            return;
        }

        // Start the timer with a delay
        timer.StartTimerDelayed(activationDelay);

        // Add listener for the timer's finished event
        timer.onTimerFinished.AddListener(ActivateObject);
    }

    // Method to activate the object
    private void ActivateObject()
    {
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(true);
            Debug.Log($"{nameof(SetActiveItem)}: {objectToActivate.name} is now active.");
        }

        // Call the UnityEvent to notify listeners that activation is complete
        onActivationComplete?.Invoke();
    }

    public void DeactivateObject()
    {
        if (objectToDeactivate != null)
        {
            objectToDeactivate.SetActive(false);
            Debug.Log($"{nameof(SetActiveItem)}: {objectToDeactivate.name} is now inactive.");
        }
    }

    private void OnDestroy()
    {
        // Remove the listener when the object is destroyed to prevent memory leaks
        if (timer != null)
        {
            timer.onTimerFinished.RemoveListener(ActivateObject);
        }
    }
}
*/