using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoringOnProjectile : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Projectile collided with: " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Projectile hit an enemy: " + collision.gameObject.name);

            // Try to get the Enemy component
            ScoringOnEnemy enemy = collision.gameObject.GetComponent<ScoringOnEnemy>();

            // Ensure the enemy has an Enemy script attached
            if (enemy != null)
            {
                Debug.Log("ScoringOnEnemy component found on: " + collision.gameObject.name);

                // Call the enemy's TakeDamage method
                enemy.TakeDamage();
            }
            else
            {
                Debug.LogWarning("Enemy component not found on object with 'Enemy' tag: " + collision.gameObject.name);
            }

            // Destroy the projectile
            Debug.Log("Projectile destroyed.");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Projectile hit a non-enemy object: " + collision.gameObject.name);
        }
    }
}
