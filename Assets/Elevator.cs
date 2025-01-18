using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public bool isPlayerCollidingWithBottom = false; // Tracks if the player is colliding with the elevator

    void OnTriggerEnter(Collider other)
    {
        // Check if the player is colliding with the elevator's trigger zone
        if (other.CompareTag("Elevator"))
        {
            isPlayerCollidingWithBottom = true; // Set true when player enters the elevator trigger
            Debug.Log("Player collided with the elevator's trigger.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Check if the player has exited the elevator's trigger zone
        if (other.CompareTag("Elevator"))
        {
            isPlayerCollidingWithBottom = false; // Reset when player leaves the elevator trigger
            Debug.Log("Player left the elevator's trigger.");
        }
    }
}
