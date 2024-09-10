using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debugThisActive : MonoBehaviour
{
    void Start()
    {
        // Check if the GameObject this script is attached to is active
        if (gameObject.activeInHierarchy)
        {
            Debug.Log("This GameObject chatbox is active.");
        }
        else
        {
            Debug.LogWarning("This GameObject chatbox is not active.");
        }
    }

    void Update()
    {
        // Optionally, you can continuously check and log the active status in the Update method
        if (gameObject.activeInHierarchy)
        {
            Debug.Log("This GameObject chatbox is active in Update.");
        }
        else
        {
            Debug.LogWarning("This GameObject chatbox is not active in Update.");
        }
    }
}