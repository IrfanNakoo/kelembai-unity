using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOffItem : MonoBehaviour
{
    public GameObject objectToActivate;
    public GameObject dependentObject;

    void Update()
    {
        if (objectToActivate != null && dependentObject != null)
        {
            objectToActivate.SetActive(dependentObject.activeSelf);
            Debug.Log(objectToActivate.name + " is now " + (dependentObject.activeSelf ? "active" : "inactive") + ".");
        }
        else
        {
            if (dependentObject == null)
            {
                Debug.LogWarning("DependentObject is null, cannot update object state.");
            }
            if (objectToActivate == null)
            {
                Debug.LogWarning("ObjectToActivate is null, cannot update object state.");
            }
        }
    }
}
