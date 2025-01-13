using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AccuracySystem : MonoBehaviour
{
    public int totalAmmo = 10;     // Total ammo available
    private int shotsFired = 0;    // Tracks how many shots have been fired
    private int shotsHit = 0;      // Tracks successful hits
    private float accuracy = 0f;   // Accuracy percentage

    public TMP_Text accuracyText;  // UI element to display accuracy

    // Call this method when the player fires a shot
    public void ShotFired()
    {
        if (shotsFired < totalAmmo)  // Ensure they can't shoot more than total ammo
        {
            shotsFired++;
            Debug.Log("Shot fired. Total shots: " + shotsFired);
            UpdateAccuracy();
        }
        else
        {
            Debug.Log("Out of ammo");
        }
    }

    // Call this method when the player hits a target
    public void ShotHit()
    {
        shotsHit++;
        Debug.Log("Shot hit! Total hits: " + shotsHit);
        UpdateAccuracy();
    }

    // Updates the accuracy based on total ammo and successful hits
    private void UpdateAccuracy()
    {
        if (totalAmmo > 0)  // Ensure no division by zero
        {
            accuracy = ((float)shotsHit / totalAmmo) * 100;  // Hits based on total ammo
            accuracyText.text = "Accuracy: " + accuracy.ToString("F2") + "%";  // Show accuracy with two decimal places
        }
        else
        {
            accuracyText.text = "Accuracy: N/A";
        }
    }

    // Reset accuracy when needed (e.g., at the start of a new level)
    public void ResetAccuracy()
    {
        shotsFired = 0;
        shotsHit = 0;
        accuracy = 0f;
        accuracyText.text = "Accuracy: N/A";
        Debug.Log("Shooting and accuracy system reset.");
    }
}