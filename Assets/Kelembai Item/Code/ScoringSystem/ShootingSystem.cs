using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingSystem : MonoBehaviour
{
    private int totalShotsFired = 0;   // Total shots fired
    private int shotsHit = 0;          // Total shots that hit a target

    // Call this when a shot is fired
    public void ShotFired()
    {
        totalShotsFired++;
        Debug.Log("Shot fired. Total shots: " + totalShotsFired);
    }

    // Call this when a shot successfully hits a target
    public void ShotHit()
    {
        shotsHit++;
        Debug.Log("Shot hit! Total hits: " + shotsHit);
    }

    // Calculate and return the accuracy percentage
    public float GetAccuracy()
    {
        if (totalShotsFired == 0) return 0;
        return (float)shotsHit / totalShotsFired * 100f;
    }
}
