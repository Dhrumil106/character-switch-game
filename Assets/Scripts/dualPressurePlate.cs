using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dualPressurePlate : MonoBehaviour
{
    [Header("Elevator Settings")]
    public Transform elevator; // The elevator object to move
    public Transform elevatorStartPosition; // The start position for the elevator
    public Transform elevatorEndPosition; // The end position for the elevator

    [Header("Platform Settings")]
    public Transform platform; // The platform object to move
    public Transform platformStartPosition; // The start position for the platform
    public Transform platformEndPosition; // The end position for the platform

    public float moveSpeed = 2f; // Speed of the objects' movement

    [Header("Pressure Plate Settings")]
    public pressurePlate plate1; // Reference to the first pressure plate
    public pressurePlate plate2; // Reference to the second pressure plate

    void Update()
    {
        // Check if both plates are pressed
        if (plate1.IsPressed && plate2.IsPressed)
        {
            // Move both objects to their end positions
            MoveObject(elevator, elevatorEndPosition.position);
            MoveObject(platform, platformEndPosition.position);
        }
        else
        {
            // Move both objects to their start positions
            MoveObject(elevator, elevatorStartPosition.position);
            MoveObject(platform, platformStartPosition.position);
        }
    }

    private void MoveObject(Transform obj, Vector3 targetPosition)
    {
        // Smoothly move the object toward the target position
        obj.position = Vector3.MoveTowards(obj.position, targetPosition, moveSpeed * Time.deltaTime);
    }
}

