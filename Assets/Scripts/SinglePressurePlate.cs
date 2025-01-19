using System.Collections;
using UnityEngine;

public class SinglePressurePlate : MonoBehaviour
{

    [Header("Elevator Settings")]
    public Transform elevator; // The elevator object to move
    public Transform startPosition; // The start position for the elevator
    public Transform endPosition; // The end position for the elevator
    public float moveSpeed = 2f; // Speed of the elevator movement

    [Header("Player Settings")]
    public Elevator player1; // Reference to the player's elevator collision script
    public Elevator player2;

    [Header("Pressure Plate Settings")]
    public pressurePlate pressurePlate; // Reference to the pressure plate

    [Header("Sound Settings")]
    public AudioSource elevatorSound; // AudioSource for the elevator sound
    public AudioClip movingSound; // Sound to play when the elevator is moving
  
    private void Start()
    {
        if (elevatorSound != null && movingSound != null )
        {
            elevatorSound.clip = movingSound;
        }
    }

    void Update()
    {
        // Only move the elevator if the player's head is not colliding with the elevator
        if (pressurePlate.IsPressed && !player1.isHeadCollidingWithElevator && !player2.isHeadCollidingWithElevator)
        {
            MoveElevator(endPosition.position); // Move elevator to end position if pressure plate is pressed
        }
        else if (!pressurePlate.IsPressed)
        {
            MoveElevator(startPosition.position); // Move elevator back to start position when pressure plate is not pressed
        }
    }

    private void MoveElevator(Vector3 targetPosition)
    {
        // Smoothly move the elevator toward the target position
        elevator.position = Vector3.MoveTowards(elevator.position, targetPosition, moveSpeed * Time.deltaTime);

        // Play the moving sound if the elevator is moving and the sound isn't already playing
        if (elevatorSound != null && !elevatorSound.isPlaying && !player1.isHeadCollidingWithElevator && !player2.isHeadCollidingWithElevator)
        {
            elevatorSound.Play();
        }

        // Stop the moving sound when the elevator reaches the target position
        if (elevator.position == targetPosition && elevatorSound.isPlaying)
        {
            elevatorSound.Stop();
        }
        if (player1.isHeadCollidingWithElevator && player2.isHeadCollidingWithElevator )
        {
            elevatorSound.Stop();
        }
    }
}