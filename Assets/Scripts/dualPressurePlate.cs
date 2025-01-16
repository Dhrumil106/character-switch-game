using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dualPressurePlate : MonoBehaviour
{
    [Header("Elevator Settings")]
    public Transform elevator; // The elevator object to move
    public Transform startPosition; // The start position for the elevator
    public Transform endPosition; // The end position for the elevator
    public float moveSpeed = 2f; // Speed of the elevator movement

    [Header("Pressure Plate Settings")]
    public pressurePlate plate1; // Reference to the first pressure plate
    public pressurePlate plate2; // Reference to the second pressure plate

    void Update()
    {
        // Check if both plates are pressed
        if (plate1.IsPressed && plate2.IsPressed)
        {
            MoveElevator(endPosition.position); // Move elevator to the end position
        }
        else
        {
            MoveElevator(startPosition.position); // Move elevator to the start position
        }
    }

    private void MoveElevator(Vector3 targetPosition)
    {
        // Smoothly move the elevator toward the target position
        elevator.position = Vector3.MoveTowards(elevator.position, targetPosition, moveSpeed * Time.deltaTime);
    }
}

