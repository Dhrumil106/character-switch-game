using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public bool isHeadCollidingWithElevator = false; // Track if the head is colliding with the bottom of the elevator

    void OnTriggerEnter(Collider other)
    {
        // Check if the trigger is from the elevator's bottom part (the part the player collides with)
        if (other.CompareTag("Elevator"))
        {
            isHeadCollidingWithElevator = true; // Set true when player's head collides with elevator
            Debug.Log("Player's head is colliding with the elevator's bottom.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Check if the player is no longer colliding with the elevator's bottom part
        if (other.CompareTag("Elevator"))
        {
            isHeadCollidingWithElevator = false; // Reset when the player's head is no longer colliding
            Debug.Log("Player's head is no longer colliding with the elevator's bottom.");
        }
    }
}
