using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoringOnEnemy : MonoBehaviour
{
    public int points = 100;  // Points awarded when this enemy is destroyed

    // Example method to destroy the enemy when hit
    public void TakeDamage()
    {
        Debug.Log("Enemy " + gameObject.name + " has taken damage.");

        // Destroy the enemy
        Destroy(gameObject);
        Debug.Log("Enemy " + gameObject.name + " has been destroyed.");

        // Add points to the score, if ScoringSystem instance exists
        if (ScoringSystem.instance != null)
        {
            Debug.Log("Adding " + points + " points to the score.");
            ScoringSystem.instance.AddScore(points);
        }
        else
        {
            Debug.LogWarning("ScoringSystem instance not found! Unable to add score.");
        }
    }
}
