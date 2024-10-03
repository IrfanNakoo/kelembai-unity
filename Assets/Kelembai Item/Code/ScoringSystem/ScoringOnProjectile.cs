 /*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoringOnProjectile : MonoBehaviour
{
    public float activationDelay = 0.5f;
    private Collider projectileCollider;

    private void Awake()
    {
        // Get the collider component
        projectileCollider = GetComponent<Collider>();

        // Initially disable the collider
        projectileCollider.enabled = false;

        // Enable the collider after a delay
        Invoke("EnableCollider", activationDelay);
    }

    void EnableCollider()
    {
        projectileCollider.enabled = true;
    }

   
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            // Damage logic for the enemy
            // Destroy the projectile
            Destroy(gameObject);
        }
    }
    
}
*/