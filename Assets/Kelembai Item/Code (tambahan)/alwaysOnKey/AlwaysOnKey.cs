using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysOnKey : MonoBehaviour
{
    [Tooltip("The object to check for within the collider.")]
    public GameObject objectToCheck;

    [Tooltip("The key to ensure is held down (default: Left Alt).")]
    public KeyCode altKey = KeyCode.LeftAlt;

    private bool isObjectWithinCollider = false;

    void OnTriggerEnter(Collider other)
    {
        // Check if the entering object matches the public object
        if (other.gameObject == objectToCheck)
        {
            isObjectWithinCollider = true;
            Debug.Log("Object entered the collider.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Check if the exiting object matches the public object
        if (other.gameObject == objectToCheck)
        {
            isObjectWithinCollider = false;
            EnableMouse(); // Re-enable the mouse when the object exits the collider
            Debug.Log("Object exited the collider.");
        }
    }

    void Update()
    {
        if (isObjectWithinCollider)
        {
            // Check if the Alt key is held down
            if (!Input.GetKey(altKey))
            {
                EnableMouse(); // Re-enable the mouse if the key is not held
                Debug.LogWarning($"The {altKey} key is not held down while the object is within the collider!");
            }
            else
            {
                DisableMouse(); // Disable the mouse if the conditions are met
                Debug.Log($"{altKey} key is being held down while the object is within the collider.");
            }
        }
        else
        {
            EnableMouse(); // Re-enable the mouse if the object is not within the collider
        }
    }

    // Disable the mouse
    void DisableMouse()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
        Cursor.visible = false; // Hide the cursor
    }

    // Enable the mouse
    void EnableMouse()
    {
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        Cursor.visible = true; // Show the cursor
    }
}
