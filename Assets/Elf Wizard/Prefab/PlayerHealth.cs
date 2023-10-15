using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClearSky {
    public class PlayerHealth : MonoBehaviour
    {
        public Transform hpBarFolder; // Reference to the folder containing the hearts.
        
        private int maxHealth = 4;
        public float currentHealth;
        private SimplePlayerController playerController; // Reference to SimplePlayerController.
        public bool restoreHP = false; // Trigger full health
        public bool isAttacked = false;

        private void Start()
        {
            currentHealth = maxHealth; // Initialize the player's health
            // Get a reference to the SimplePlayerController component.
            playerController = FindObjectOfType<SimplePlayerController>();
            UpdateUI();
        }

        public void UpdateUI()
        {
            // Calculate how many full, half, and empty hearts are needed
            int fullHearts = Mathf.FloorToInt(currentHealth);
            int halfHearts = Mathf.RoundToInt((currentHealth - fullHearts) * 2); // Round to the nearest half-heart
            int emptyHearts = maxHealth - fullHearts - (halfHearts > 0 ? 1 : 0); // Decrease empty hearts only if half hearts are present

            // Disable all heart icons initially
            foreach (Transform child in hpBarFolder)
            {
                child.gameObject.SetActive(false);
            }

            // Enable the necessary heart icons based on health
            for (int i = 0; i < fullHearts; i++)
            {
                Transform heart = hpBarFolder.Find($"Heart {i + 1} Full");
                if (heart != null)
                {
                    heart.gameObject.SetActive(true);
                }
            }

            // Handle the transition from a full heart to a half heart
            if (fullHearts < maxHealth && halfHearts > 0)
            {
                Transform heart = hpBarFolder.Find($"Heart {fullHearts + 1} Half");
                if (heart != null)
                {
                    heart.gameObject.SetActive(true);
                }
            }

            for (int i = fullHearts + halfHearts; i < fullHearts + halfHearts + emptyHearts; i++)
            {
                Transform heart = hpBarFolder.Find($"Heart {i + 1} Empty");
                if (heart != null)
                {
                    heart.gameObject.SetActive(true);
                }
            }
            
        }

        private void Update()
        {
            if (isAttacked)
            {
                currentHealth -= 0.5f; // Taking half heart damage.
                playerController.canHurt = true;
                UpdateUI();
                if (currentHealth <= 0f) {
                    if (playerController != null) {
                        playerController.alive = false;
                        playerController.Die();
                    }
                }
                isAttacked = false;
            }
        }

        public void RestoreHealth()
        {
            if (restoreHP) {
                currentHealth = maxHealth; // Reset current health to maximum
                UpdateUI(); // Update the UI to reflect the restored health
                restoreHP = false;
            }
        }

    }
}