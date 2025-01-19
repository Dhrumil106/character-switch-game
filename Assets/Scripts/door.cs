using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door : MonoBehaviour
{

    public GameObject door1; // The first door to move
    public Transform targetTransform1; // The target position for door1
    public GameObject door2; // The second door to move
    public Transform targetTransform2; // The target position for door2
    public float moveSpeed = 2f; // Speed of the doors' movement

    private Vector3 originalPosition1; // Store the original position of door1
    private Vector3 originalPosition2; // Store the original position of door2
    private bool isLaserHitting = false; // Tracks whether the laser is hitting

    private void Start()
    {
        // Store the original positions of the doors
        originalPosition1 = door1.transform.position;
        originalPosition2 = door2.transform.position;
    }

    private void Update()
    {
        if (isLaserHitting)
        {
            // Move door1 to its target position
            door1.transform.position = Vector3.MoveTowards(door1.transform.position, targetTransform1.position, moveSpeed * Time.deltaTime);

            // Move door2 to its target position
            door2.transform.position = Vector3.MoveTowards(door2.transform.position, targetTransform2.position, moveSpeed * Time.deltaTime);
        }
        else
        {
            // Return door1 to its original position
            door1.transform.position = Vector3.MoveTowards(door1.transform.position, originalPosition1, moveSpeed * Time.deltaTime);

            // Return door2 to its original position
            door2.transform.position = Vector3.MoveTowards(door2.transform.position, originalPosition2, moveSpeed * Time.deltaTime);
        }
    }

    // This method will be called by the laser script to set the laser hitting state
    public void SetLaserHitting(bool isHitting)
    {
        isLaserHitting = isHitting;
    }
}
