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
    private Vector3 allowedPullDirection;

    [Header("References")]
    public Transform playerPullPosition; // Position where the object should move towards.

    void Update()
    {
        // Check for input to start pulling.
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

        // Handle pulling if active.
        if (isPulling && objectToPull != null)
        {
            PullObject();
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
                isPulling = true;

                // Calculate the allowed pull direction based on relative positions.
                allowedPullDirection = (transform.position - objectToPull.position).normalized;
                allowedPullDirection.y = 0; // Restrict to horizontal plane.

                Debug.Log("Started pulling: " + objectToPull.name);
                return;
            }
        }

        Debug.Log("No pullable object in range.");
    }

    void StopPulling()
    {
        isPulling = false;
        objectToPull = null;
        Debug.Log("Stopped pulling.");
    }

    void PullObject()
    {
        // Calculate the direction of movement.
        Vector3 pullDirection = (playerPullPosition.position - objectToPull.position).normalized;

        // Restrict movement to the allowed pull direction.
        pullDirection = Vector3.Project(pullDirection, allowedPullDirection);

        // Calculate the target position using Vector3.MoveTowards.
        Vector3 targetPosition = Vector3.MoveTowards(
            objectToPull.position,
            objectToPull.position + pullDirection,
            pullSpeed * Time.deltaTime
        );

        // Check the distance to ensure it does not get too close to the player.
        float distanceToPlayer = Vector3.Distance(targetPosition, playerPullPosition.position);
        if (distanceToPlayer > minDistance && distanceToPlayer <= maxDistance)
        {
            // Maintain the object's original Y-position.
            targetPosition.y = objectToPull.position.y;

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



