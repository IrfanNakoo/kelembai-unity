using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingSystem : MonoBehaviour
{
    public AccuracySystem shootingSystem;

    void Start()
    {
        // If no AccuracySystem is assigned in the Inspector, find one in the scene
        if (shootingSystem == null)
        {
            shootingSystem = FindObjectOfType<AccuracySystem>();
        }

        // If still null, print an error to help with debugging
        if (shootingSystem == null)
        {
            Debug.LogError("AccuracySystem not found in the scene. Make sure it exists.");
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1")) // Replace with your shooting input
        {
            shootingSystem?.ShotFired(); // Safe call with the null-conditional operator
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) // Assuming enemies have the tag "Enemy"
        {
            shootingSystem?.ShotHit(); // Safe call with the null-conditional operator
        }
    }
}
