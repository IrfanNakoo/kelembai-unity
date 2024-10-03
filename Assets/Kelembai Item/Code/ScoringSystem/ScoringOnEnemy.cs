/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoringOnEnemy : MonoBehaviour
{
    // Enemy health
    public int enemyHealth = 100;

    // Points awarded when this enemy is destroyed
    public int pointsForDestruction = 100;

    // Optionally, you can handle collision or other forms of interaction
    // Example of collision damage:
    //part nie just on klau missile ad collider
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerProjectile"))
        {
            TakeDamage(5000);  // Example damage value
        }
    }

    // This function can be called when the enemy takes damage
    public void TakeDamage(int damage)
    {
        enemyHealth -= damage;

        if (enemyHealth <= 0)
        {
            DestroyEnemy();
            Debug.Log("Dia detect enemy dah mati");
            Debug.Log("n nie maknanya on trigger enter collider part hidup");
        }
    }

    // Function to handle enemy destruction
    void DestroyEnemy()
    {
        // Award points to the player
        if (ScoringSystem.instance != null)
            Debug.Log("Dia detect scoring system wujud");
        {
            ScoringSystem.instance.AddScore(pointsForDestruction);
        }

        // Destroy the enemy game object
        Destroy(gameObject);
    }
}*/