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

    private Transform objectToPull;
    private bool isPulling = false;
    private Renderer objectRenderer;  // Renderer to change the color of the object
    private Color originalColor;      // To store the original color of the object

    [Header("References")]
    public Transform playerPullPosition; // Position where the object should move towards.
    public LineRenderer lineRenderer;    // The LineRenderer component to draw the line

    void Start()
    {
        // Initialize lineRenderer if it was not set in the inspector
        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        // Make sure the LineRenderer is disabled at the start
        lineRenderer.enabled = false;
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
            UpdateLineRenderer();  // Update the line renderer while pulling
            ChangeObjectColor(Color.green);  // Change to green while pulling
            CheckMaxDistance();
        }
    }
    void FixedUpdate()
    {
        // Check for input to start pulling.


        // Handle pulling if active.
        if (isPulling && objectToPull != null)
        {
            PullObject();
            UpdateLineRenderer();  // Update the line renderer while pulling
            ChangeObjectColor(Color.green);  // Change to green while pulling
            CheckMaxDistance();
        }
        else
        {
            // If the object is within pull range but not being pulled, change color to red.
            if (objectToPull != null)
            {
                ChangeObjectColor(Color.red);
                ShowLineRenderer();  // Show line renderer when in range but not pulling
            }
            // Reset color when not pulling and hide the line renderer
            else
            {
                ResetObjectColor();
                lineRenderer.enabled = false;
            }
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

                // Change the color to red when in pulling range, before starting the pull
                ChangeObjectColor(Color.red);

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
        lineRenderer.enabled = false;  // Disable the line renderer when pulling stops
    }

    void PullObject()
    {
        // Calculate the direction of movement towards the player (ignoring Y-axis).
        Vector3 pullDirection = new Vector3(playerPullPosition.position.x - objectToPull.position.x, 0f, playerPullPosition.position.z - objectToPull.position.z).normalized;

        // Calculate the distance to the player.
        float distanceToPlayer = Vector3.Distance(objectToPull.position, playerPullPosition.position);

        // Only move the object if it's farther than the minDistance.
        if (distanceToPlayer > minDistance && distanceToPlayer <= maxDistance)
        {
            // Move the object towards the player using Vector3.MoveTowards.
            Vector3 targetPosition = Vector3.MoveTowards(objectToPull.position, playerPullPosition.position, pullSpeed * Time.deltaTime);

            // Ensure the Y position remains the same to prevent vertical movement.
            targetPosition.y = objectToPull.position.y;

            // Set the object�s position.
            objectToPull.position = targetPosition;
        }
        // If the object is within the minDistance, stop pulling completely (no movement).
        else if (distanceToPlayer <= minDistance)
        {
            // Stop the object's movement completely without pushing it out or back.
            Debug.Log("Object is within minimum distance, stopping pull.");
        }
        else if (distanceToPlayer > maxDistance)
        {
            // Stop pulling if the object exceeds the maximum distance.
            Debug.Log("Object exceeded maximum distance, stopping pull.");
            StopPulling();
        }
        lineRenderer.enabled = true;
    }

    void ResetObjectColor()
    {
        // Reset the object color to the original color when it's no longer being pulled
        if (objectRenderer != null && !isPulling)
        {
            objectRenderer.material.color = originalColor;
        }
    }

    void ChangeObjectColor(Color newColor)
    {
        // Change the object's color
        if (objectRenderer != null)
        {
            objectRenderer.material.color = newColor;
        }
    }

    void ShowLineRenderer()
    {
        // Show line renderer when within pull range
        if (lineRenderer != null && objectToPull != null)
        {

            lineRenderer.SetPosition(0, playerPullPosition.position);  // Set the start position (player)
            lineRenderer.SetPosition(1, objectToPull.position);        // Set the end position (object)
        }
    }

    void UpdateLineRenderer()
    {
        // Update the line renderer during pulling
        if (lineRenderer != null && objectToPull != null)
        {
            lineRenderer.SetPosition(0, playerPullPosition.position);  // Player position
            lineRenderer.SetPosition(1, objectToPull.position);        // Object position
        }
    }

    void CheckMaxDistance()
    {
        // Stop pulling if the object exceeds the maximum distance.
        float distanceToPlayer = Vector3.Distance(objectToPull.position, transform.position);
        if (distanceToPlayer > maxDistance)
        {
            Debug.Log("Object exceeded maximum distance, stopping pull.");
            StopPulling();
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the pull range in the scene view.
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pullRange);

        // Visualize the maximum distance.
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, maxDistance);

        // Visualize the minimum distance.
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(playerPullPosition.position, minDistance);
    }
}


