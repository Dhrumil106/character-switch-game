using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PullingObjects : MonoBehaviour
{
    [Header("Pulling Settings")]
    public float pullSpeed = 5f; // Speed at which the object moves towards the player.
    public float pullRange = 2f; // Maximum distance to initiate pulling.
    public float minDistance = 0.5f; // Minimum distance the object can approach the player.
    public float maxDistance = 5f; // Maximum distance at which pulling is active.
    public string pullableTag = "Pullable"; // Tag for pullable objects.
    public float x, y, z;

    private Transform objectToPull;
    private bool isPulling = false;
    private Renderer objectRenderer;  // Renderer to change the color of the object
    private Color originalColor;      // To store the original color of the object

    [Header("References")]
    public Transform playerPullPosition; // Position where the object should move towards.
    public ParticleSystem pullParticles; // Reference to the Particle System

    void Start()
    {
        // Ensure the particle system is stopped at the start
        if (pullParticles != null)
        {
            pullParticles.Stop();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isPulling)
            {
                TryStartPulling();
            }
            else
            {
                StopPulling();
            }
        }

        if (isPulling && objectToPull != null)
        {
            PullObject();
            UpdateParticleSystem();  // Update the particle system while pulling
            ChangeObjectColor(Color.green);  // Change to green while pulling
            CheckMaxDistance();
        }
    }

    void FixedUpdate()
    {
        if (isPulling && objectToPull != null)
        {
            PullObject();
            UpdateParticleSystem();  // Update the particle system while pulling
            ChangeObjectColor(Color.green);  // Change to green while pulling
            CheckMaxDistance();
        }
    }

    void TryStartPulling()
    {
        // Find the nearest pullable object within range.
        Collider[] colliders = Physics.OverlapSphere(transform.position, pullRange);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag(pullableTag))
            {
                objectToPull = collider.transform;
                objectRenderer = objectToPull.GetComponent<Renderer>(); // Get the Renderer of the object
                originalColor = objectRenderer.material.color;  // Store the original color
                isPulling = true;

                if (pullParticles != null)
                {
                    pullParticles.Play(); // Start the particle effect
                }

                // Change the color to red when in pulling range, before starting the pull
                

                Debug.Log("Started pulling: " + objectToPull.name);
                return;
            }
        }

        Debug.Log("No pullable object in range.");
    }

    public void StopPulling()
    {
        isPulling = false;
        objectToPull = null;
        Debug.Log("Stopped pulling.");
        ResetObjectColor(); // Reset color when pulling stops

        if (pullParticles != null)
        {
            pullParticles.Stop(); // Stop the particle effect
        }
    }

    void PullObject()
    {
        Vector3 pullDirection = (playerPullPosition.position - objectToPull.position).normalized;
        float distanceToPlayer = Vector3.Distance(objectToPull.position, playerPullPosition.position);

        if (distanceToPlayer > minDistance && distanceToPlayer <= maxDistance)
        {
            Vector3 targetPosition = Vector3.MoveTowards(objectToPull.position, playerPullPosition.position, pullSpeed * Time.deltaTime);
            targetPosition.y = objectToPull.position.y; // Prevent vertical movement
            objectToPull.position = targetPosition;
        }
        else if (distanceToPlayer <= minDistance)
        {
            Debug.Log("Object is within minimum distance, stopping pull.");
        }
        else if (distanceToPlayer > maxDistance)
        {
            Debug.Log("Object exceeded maximum distance, stopping pull.");
            StopPulling();
        }
    }

    void ResetObjectColor()
    {
        if (objectRenderer != null && !isPulling)
        {
            objectRenderer.material.color = originalColor;
        }
    }

    void ChangeObjectColor(Color newColor)
    {
        if (objectRenderer != null)
        {
            objectRenderer.material.color = newColor;
        }
    }

    void UpdateParticleSystem()
    {
        if (pullParticles != null && objectToPull != null)
        {
            // Calculate the midpoint between the player and the object
            Vector3 midPoint = (playerPullPosition.position + objectToPull.position) / 2;

            // Adjust the Y-axis (e.g., raise the particles by 0.5 units)
            midPoint.x += x;
            midPoint.y += y;
            midPoint.z += z;
            // Update the particle system's position
            pullParticles.transform.position = midPoint;

            // Adjust the particle system's rotation to face the object
            var shape = pullParticles.shape;
            shape.rotation = Quaternion.LookRotation(objectToPull.position - playerPullPosition.position).eulerAngles;

            // Update the scale of the particle system to match the distance
            float distance = Vector3.Distance(playerPullPosition.position, objectToPull.position);
            shape.scale = new Vector3(0.1f, 0.1f, distance);
        }
    }

    void CheckMaxDistance()
    {
        float distanceToPlayer = Vector3.Distance(objectToPull.position, transform.position);
        if (distanceToPlayer > maxDistance)
        {
            Debug.Log("Object exceeded maximum distance, stopping pull.");
            StopPulling();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pullRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, maxDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(playerPullPosition.position, minDistance);
    }
}


