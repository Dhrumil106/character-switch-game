using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pressurePlate : MonoBehaviour
{

    [Header("Pressure Plate Settings")]
    public Transform elevator; // The elevator object to move (cube)
    public Transform startPosition; // The start position for the elevator
    public Transform endPosition; // The end position for the elevator
    public float moveSpeed = 2f; // Speed at which the elevator moves
    private bool isPressed = false; // Tracks if the plate is stepped on
    public float plateUpHeight = 0.5f; // Height to move the plate up when pressed
    private Vector3 originalPlatePosition; // Original position of the plate
    private HashSet<string> objectsOnPlate = new HashSet<string>(); // To track objects on the plate
    public Elevator script;

    void Start()
    {
        originalPlatePosition = transform.position; // Store the original position of the plate
    }

    void Update()
    {
        // If player isn't colliding with the bottom of the elevator, move it based on pressure plate state
        if (isPressed && !script.isPlayerCollidingWithBottom)
        {
            MoveElevator(endPosition.position); // Move the elevator to the end position
            MovePlateUp(); // Move the plate up when pressed
        }
        else if (!isPressed)
        {
            MoveElevator(startPosition.position); // Move the elevator back to the start position
            MovePlateDown(); // Move the plate down when not pressed or collision detected
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Pullable")) // Detect player or pullable objects
        {
            objectsOnPlate.Add(other.tag);
            CheckPressurePlateState();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Pullable"))
        {
            objectsOnPlate.Remove(other.tag);
            CheckPressurePlateState();
        }
    }

    void CheckPressurePlateState()
    {
        // If any object is on the plate, it is considered pressed
        isPressed = objectsOnPlate.Count > 0;
    }

    void MoveElevator(Vector3 target)
    {
        // Move the elevator smoothly towards the target position
        elevator.position = Vector3.MoveTowards(elevator.position, target, moveSpeed * Time.deltaTime);
    }

    void MovePlateUp()
    {
        // Move the plate upwards when pressed
        Vector3 targetPosition = originalPlatePosition + new Vector3(0, -plateUpHeight, 0);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    void MovePlateDown()
    {
        // Move the plate back down to its original position
        transform.position = Vector3.MoveTowards(transform.position, originalPlatePosition, moveSpeed * Time.deltaTime);
    }

    // Detect collision with player's head or bottom
}