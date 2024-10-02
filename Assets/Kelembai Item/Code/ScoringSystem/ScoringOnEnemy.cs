using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoringOnEnemy : MonoBehaviour
{
    public int enemyHealth = 100;

    void TakeDamage(int damage)
    {
        enemyHealth -= damage;

        if (enemyHealth <= 0)
        {
            // Destroy the enemy object
            Destroy(gameObject);

            // Add score to the player
            ScoringSystem.instance.AddScore(ScoringSystem.instance.scorePerEnemy);
        }
    }
}