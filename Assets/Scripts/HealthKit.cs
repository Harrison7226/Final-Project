using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthKit : MonoBehaviour
{
    [SerializeField]
    private float healAmount = 20;

    [SerializeField]
    private AudioClip healSound;

    PlayerHealth health;

    public void Start()
    {
        health = FindObjectOfType<PlayerHealth>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // If the player's health is already full, don't do anything
            if (health.GetHealth() == health.GetMaxHealth())
            {
                return;
            }

            // Otherwise, restore the player's health and destroy the health kit
            health.RestoreHealth(healAmount);
            AudioSource.PlayClipAtPoint(healSound, transform.position);
            Destroy(gameObject);
        }
    }
}
