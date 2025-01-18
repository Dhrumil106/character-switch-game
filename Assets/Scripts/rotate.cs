using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour
{
    [SerializeField] private float rotationAmount = 10f;  // Degrees to rotate per press
    [SerializeField] private float rotationDelay = 0.5f;  // Time delay between each rotation (in seconds)
    [SerializeField] private KeyCode interactKey = KeyCode.E; // Interaction key
    [SerializeField] private string playerTag = "Player";  // Tag for the player
    [SerializeField] private float interactionRange = 5f;  // Range within which interaction is possible

    private Transform playerTransform;  // Reference to the player's Transform
    private float timeSinceLastRotation = 0f;  // Timer for controlling the rotation speed

    private void Start()
    {
        // Find the player object by tag and get its Transform
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            
        }
    }

    private void Update()
    {
        // Only proceed if the player is assigned
        if (playerTransform != null)
        {
            // Calculate the distance between the player and the object
            float distance = Vector3.Distance(playerTransform.position, transform.position);

            // Check if the player is within interaction range
            if (distance <= interactionRange)
            {
                // If the interact key is being held down, rotate the object slowly
                if (Input.GetKey(interactKey))
                {
                    timeSinceLastRotation += Time.deltaTime;  // Increment the timer

                    // Check if the time since the last rotation exceeds the delay
                    if (timeSinceLastRotation >= rotationDelay)
                    {
                        RotateObject();
                        timeSinceLastRotation = 0f;  // Reset the timer after rotating
                    }
                }
            }
        }
    }

    private void RotateObject()
    {
        // Rotate the object by exactly 10 degrees each time
        transform.Rotate(0, rotationAmount, 0, Space.Self);
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a sphere in the editor to visualize the interaction range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}
