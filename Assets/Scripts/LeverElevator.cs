using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverElevator : MonoBehaviour
{
    [Header("Elevator Settings")]
    public Transform elevator; // The elevator object to move
    public Transform startPosition; // The start position for the elevator
    public Transform endPosition; // The end position for the elevator
    public float moveSpeed = 2f; // Speed of the elevator movement

    [Header("Pressure Plate Settings")]
     // Reference to the pressure plate
    public Elevator elevatorScript;
    public Lever2 leverScript;
    void Update()
    {
        if (!elevatorScript.isHeadCollidingWithElevator)
        // Check if the pressure plate is pressed
        {


            if (leverScript.isLeverActivated)
            {
                MoveElevator(endPosition.position); // Move the elevator to the end position
            }
            else
            {
                MoveElevator(startPosition.position); // Move the elevator back to the start position
            }
        }
    }

    private void MoveElevator(Vector3 targetPosition)
    {
        // Smoothly move the elevator toward the target position
        elevator.position = Vector3.MoveTowards(elevator.position, targetPosition, moveSpeed * Time.deltaTime);
    }
}
