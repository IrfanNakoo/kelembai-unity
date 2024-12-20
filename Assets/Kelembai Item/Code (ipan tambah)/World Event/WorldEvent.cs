using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WorldEvent : MonoBehaviour
{
    [Header("Settings")]
    public Transform characterA;  // Reference to the character being tracked
    public Transform targetLocation;  // Reference to the target location
    public float reachRadius = 1f;  // How close the character needs to be to trigger the event
    public float approachRadius = 5f;  // Optional: Range for "approaching" event

    private bool locationReached = false;  // Flag to track if the location was reached
    private bool isApproaching = false;  // Flag to track if the character is approaching

    [Header("Events")]
    public UnityEvent onLocationReached;  // Event when character reaches the target
    public UnityEvent onApproachingLocation;  // Event when character is approaching
    public UnityEvent onLeftLocation;  // Event when character leaves the location

    private float distanceToTargetZ;

    void Start()
    {
        // Ensure events are initialized if not set in the Inspector
        if (onLocationReached == null) onLocationReached = new UnityEvent();
        if (onApproachingLocation == null) onApproachingLocation = new UnityEvent();
        if (onLeftLocation == null) onLeftLocation = new UnityEvent();

        // Start the coroutine to log distance every second
        StartCoroutine(LogDistanceEverySecond());
    }

    void Update()
    {
        // Ensure that CharacterA and TargetLocation are assigned
        if (characterA == null || targetLocation == null)
        {
            Debug.LogWarning("CharacterA or TargetLocation is not assigned!");
            return;
        }

        // Calculate the distance on the X-axis between CharacterA and TargetLocation
        distanceToTargetZ = Mathf.Abs(characterA.position.z - targetLocation.position.z);

        // Check if the character is within the reach radius
        if (distanceToTargetZ <= reachRadius && !locationReached)
        {
            locationReached = true;
            isApproaching = false;
            onLocationReached.Invoke();  // Trigger the "reached" event
            Debug.Log("CharacterA reached the location on the Z-axis!");
        }
        else if (distanceToTargetZ > reachRadius && locationReached)
        {
            locationReached = false;
            onLeftLocation.Invoke();  // Trigger the "left" event
            Debug.Log("CharacterA left the location on the Z-axis!");
        }

        // Check if the character is approaching
        if (distanceToTargetZ <= approachRadius && distanceToTargetZ > reachRadius && !isApproaching)
        {
            isApproaching = true;
            onApproachingLocation.Invoke();  // Trigger the "approaching" event
            Debug.Log("CharacterA is approaching the location on the Z-axis!");
        }
        else if (distanceToTargetZ > approachRadius && isApproaching)
        {
            isApproaching = false;  // Reset approaching state
        }
    }

    // Coroutine to log the distance every second
    IEnumerator LogDistanceEverySecond()
    {
        while (true)
        {
            Debug.Log("Distance to Target on Z-axis: " + distanceToTargetZ);
            yield return new WaitForSeconds(1f);  // Wait for 1 second
        }
    }
}
