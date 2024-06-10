using System.Collections;
using UnityEngine;

public class OnOffItem : MonoBehaviour
{
    public GameObject objectToActivate;
    public GameObject dependentObject;
    public GameObject objectToDeactivate;

    // Method to activate the object
    public void ActivateObject()
    {
        if (dependentObject != null && objectToActivate != null)
        {
            objectToActivate.SetActive(true);
            Debug.Log(objectToActivate.name + " is now active.");
        }
        else
        {
            if (dependentObject == null)
            {
                Debug.LogWarning("DependentObject is null, cannot activate object.");
            }
            if (objectToActivate == null)
            {
                Debug.LogWarning("ObjectToActivate is null, cannot activate object.");
            }
        }
    }

    public void DeactivateObject()
    {
        if (dependentObject == null && objectToDeactivate != null)
        {
            objectToDeactivate.SetActive(false);
            Debug.Log(objectToDeactivate.name + " is now inactive.");
        }
        else
        {
            if (dependentObject != null)
            {
                Debug.LogWarning("DependentObject is not null, will not deactivate object.");
            }
            if (objectToDeactivate == null)
            {
                Debug.LogWarning("ObjectToDeactivate is null, cannot deactivate object.");
            }
        }
    }
}
