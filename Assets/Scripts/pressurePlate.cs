using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pressurePlate : MonoBehaviour
{

    [Header("Pressure Plate Settings")]
    public Transform cube; // The cube to move
    public Transform startPosition; // The start position for the cube
    public Transform endPosition; // The end position for the cube
    public float moveSpeed = 2f; // Speed at which the cube moves
    private bool isPressed = false; // Tracks if the plate is stepped on
    private Collider plateCollider;
    public float plateUpHeight = 0.5f; // Height to move the plate up when pressed
    private Vector3 originalPlatePosition; // Original position of the plate
    private HashSet<string> objectsOnPlate = new HashSet<string>(); // To track objects on the plate

    void Start()
    {
        plateCollider = GetComponent<Collider>(); // Get the collider of the plate to detect collisions
        originalPlatePosition = transform.position; // Store the original position of the plate
    }

    void Update()
    {
        // Move the cube based on whether the plate is stepped on or not
        if (isPressed)
        {
            MoveCube(endPosition.position); // Move the cube to the end position
            MovePlateUp(); // Move the plate up
        }
        else
        {
            MoveCube(startPosition.position); // Move the cube back to the start position
            MovePlateDown(); // Move the plate back down to its original position
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Detect if the player or any object steps on the plate
        if (other.CompareTag("Player") || other.CompareTag("Pullable")) // Detect both the player and pullable objects
        {
            objectsOnPlate.Add(other.tag); // Add the object to the set
            CheckPressurePlateState(); // Check if the plate should be pressed
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Detect if the player or any object leaves the plate
        if (other.CompareTag("Player") || other.CompareTag("Pullable")) // Detect both the player and pullable objects
        {
            objectsOnPlate.Remove(other.tag); // Remove the object from the set
            CheckPressurePlateState(); // Check if the plate should be unpressed
        }
    }

    void CheckPressurePlateState()
    {
        // If any object is on the plate, keep it pressed
        isPressed = objectsOnPlate.Count > 0;
    }

    void MoveCube(Vector3 target)
    {
        // Move the cube smoothly towards the target position
        cube.position = Vector3.MoveTowards(cube.position, target, moveSpeed * Time.deltaTime);
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
}