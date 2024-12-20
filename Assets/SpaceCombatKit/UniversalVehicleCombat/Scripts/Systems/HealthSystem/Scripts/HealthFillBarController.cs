using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VSX.Utilities.UI;
using TMPro;

namespace VSX.UniversalVehicleCombat
{
    /// <summary>
    /// Controls a health bar that uses Unity's UI.
    /// </summary>
    public class HealthFillBarController : UIFillBarController
    {

        [Tooltip("The health type that is health bar should display.")]
        [SerializeField]
        protected HealthType healthType;
        public HealthType HealthType { get { return healthType; } }

        [Tooltip("The health component that this health bar displays information for.")]
        [SerializeField]
        protected Health health;

        [Tooltip("The text component that displays the health.")]
        [SerializeField]
        protected TMP_Text healthText; // Text UI element to display health.


        // Called every frame
        protected virtual void Update()
        {
            // Update the health bar
            if (health != null && healthText != null)
            {
                //SetFillAmount(health.GetCurrentHealthFractionByType(healthType));
                // Get the current and maximum health for the specified health type
                float currentHealth = health.GetCurrentHealthByType(healthType);
                float maxHealth = health.GetHealthCapacityByType(healthType);

                // Update the health text (e.g., "75 / 100")
                healthText.text = $"{(int)currentHealth} / {(int)maxHealth}";

                // (Optional) Update the fill bar if needed
                SetFillAmount(currentHealth / maxHealth);
            }
        }
    }
}
//nie code lama yang dimodify