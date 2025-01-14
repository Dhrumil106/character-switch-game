using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public bool isPlayerCollidingWithBottom = false; // Tracks if the player is colliding with the bottom

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Check if the collision is with the player's head or bottom part
            // assuming you are detecting collisions with the top part of the player
            isPlayerCollidingWithBottom = true; // Stop elevator if player's bottom hits elevator
            Debug.Log("Player collided with elevator's bottom. Stopping movement.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Reset when player is no longer colliding with the elevator
            isPlayerCollidingWithBottom = false;
            Debug.Log("Player left collision area. Elevator can resume.");
        }
    }
}
