using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door : MonoBehaviour
{

    public GameObject door1; // The door to move
    public Transform targetTransform; // The target position (can be a specific Transform or position)
    public float moveSpeed = 2f; // Speed of the door's movement

    private Vector3 originalPosition; // Store the original position of the door
    private bool isLaserHitting = false; // Tracks whether the laser is hitting

    private void Start()
    {
        // Store the original position of the door
        originalPosition = door1.transform.position;
    }

    private void Update()
    {
        // Check if the laser is hitting the door
        if (isLaserHitting)
        {
            // Move to the target position if the laser is hitting
            door1.transform.position = Vector3.MoveTowards(door1.transform.position, targetTransform.position, moveSpeed * Time.deltaTime);
        }
        else
        {
            // Return to the original position when the laser is not hitting
            door1.transform.position = Vector3.MoveTowards(door1.transform.position, originalPosition, moveSpeed * Time.deltaTime);
        }
    }

    // This method will be called by the laser script to set the laser hitting state
    public void SetLaserHitting(bool isHitting)
    {
        isLaserHitting = isHitting;
    }
}
