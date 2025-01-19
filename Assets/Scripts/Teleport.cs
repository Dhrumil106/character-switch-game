using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [Header("Teleportation Settings")]
    public string teleportTag = "Teleport"; // The tag of objects that trigger teleportation

    [Header("Effects Settings")]
    public bool playEffect = true; // Whether to play a visual effect on the player when teleporting
    public ParticleSystem teleportEffect; // The particle effect to play on the player

    [Header("Audio Settings")]
    public bool playSound = true; // Whether to play a sound when teleporting
    public AudioSource audioSource; // Reference to the AudioSource
    public AudioClip teleportSound; // The sound to play when teleporting

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player touched a teleport object
        if (other.CompareTag(teleportTag))
        {
            // Check for the Teleporter component on the touched object
            Teleporter teleporter = other.GetComponent<Teleporter>();
            if (teleporter != null && teleporter.teleportTarget != null)
            {
                TeleportPlayer(teleporter.teleportTarget);

                // Play the particle effect on the player
                if (playEffect && teleportEffect != null)
                {
                    ParticleSystem effect = Instantiate(teleportEffect, transform.position, Quaternion.identity);
                    effect.Play();
                    Destroy(effect.gameObject, effect.main.duration); // Automatically clean up the effect after it finishes
                }

                // Play the teleport sound
                if (playSound && audioSource != null && teleportSound != null)
                {
                    audioSource.PlayOneShot(teleportSound);
                }

                Debug.Log("Player teleported to: " + teleporter.teleportTarget.position);
            }
        }
    }

    private void TeleportPlayer(Transform destination)
    {
        // Move the player to the specified destination
        transform.position = destination.position;
        transform.rotation = destination.rotation;
    }
}
