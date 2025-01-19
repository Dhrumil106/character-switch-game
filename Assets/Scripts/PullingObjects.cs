using cakeslice;
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
    public Outline outline;

    [Header("Audio Settings")]
    public AudioSource audioSource; // Reference to the AudioSource
    public AudioClip pullSound; // The sound to play while pulling

    void Start()
    {
        // Ensure the particle system is stopped at the start
        if (pullParticles != null)
        {
            pullParticles.Stop();
        }
        outline.enabled = false;

        // Ensure the AudioSource is properly configured
        if (audioSource != null)
        {
            audioSource.loop = true; // Set the audio to loop
            audioSource.clip = pullSound; // Assign the pulling sound clip
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
            UpdateParticleSystem();
            ChangeObjectColor(Color.black);  // Change to black while pulling
            CheckMaxDistance();
        }

        // Handle highlighting and outlines for pullable objects
        HandleObjectOutlines();
    }

    void FixedUpdate()
    {
        if (isPulling && objectToPull != null)
        {
            PullObject();
            UpdateParticleSystem();
            ChangeObjectColor(Color.black);
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
                objectRenderer = objectToPull.GetComponent<Renderer>();
                originalColor = objectRenderer.material.color; // Store the original color
                isPulling = true;

                if (pullParticles != null)
                {
                    pullParticles.Play(); // Start the particle effect
                }

                // Play pulling sound
                if (audioSource != null && pullSound != null)
                {
                    audioSource.Play();
                }

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
        ResetObjectColor();

        if (pullParticles != null)
        {
            pullParticles.Stop(); // Stop the particle effect
        }

        // Stop pulling sound
        if (audioSource != null)
        {
            audioSource.Stop();
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
            Vector3 midPoint = (playerPullPosition.position + objectToPull.position) / 2;
            midPoint.x += x;
            midPoint.y += y;
            midPoint.z += z;
            pullParticles.transform.position = midPoint;

            var shape = pullParticles.shape;
            shape.rotation = Quaternion.LookRotation(objectToPull.position - playerPullPosition.position).eulerAngles;

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

    private void HandleObjectOutlines()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, pullRange);
        bool isAnyPullableInRange = false;
        var playerMove = GetComponent<ThirdPerson>();
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag(pullableTag) && playerMove.enabled == true)
            {
                isAnyPullableInRange = true;

                Outline objectOutline = collider.GetComponent<Outline>();
                if (objectOutline != null)
                {
                    objectOutline.enabled = true;
                }
            }
            else
            {
                outline.enabled = false;
            }
        }

        if (!isAnyPullableInRange)
        {
            foreach (Collider collider in Physics.OverlapSphere(transform.position, pullRange * 2))
            {
                if (collider.CompareTag(pullableTag))
                {
                    Outline objectOutline = collider.GetComponent<Outline>();
                    if (objectOutline != null)
                    {
                        objectOutline.enabled = false;
                    }
                }
            }
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


