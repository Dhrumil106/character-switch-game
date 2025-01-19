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

    [Header("Audio Settings")]
    public AudioSource audioSource; // The AudioSource component to play the sound
    public AudioClip doorSound; // The sound to play when plates are activated or deactivated

    private bool platesAreBothPressed = false; // Tracks the state of both plates being pressed

    void Update()
    {
        // Check if both plates are pressed
        bool bothPressed = plate1.IsPressed && plate2.IsPressed;

        if (bothPressed && !platesAreBothPressed)
        {
            // Play sound when both plates are activated
            PlaySound();
            platesAreBothPressed = true;
        }
        else if (!bothPressed && platesAreBothPressed)
        {
            // Play sound when one or both plates are deactivated
            PlaySound();
            platesAreBothPressed = false;
        }

        // Move objects based on the state of the plates
        if (platesAreBothPressed)
        {
            MoveObject(elevator, elevatorEndPosition.position);
            MoveObject(platform, platformEndPosition.position);
        }
        else
        {
            MoveObject(elevator, elevatorStartPosition.position);
            MoveObject(platform, platformStartPosition.position);
        }
    }

    private void MoveObject(Transform obj, Vector3 targetPosition)
    {
        // Smoothly move the object toward the target position
        obj.position = Vector3.MoveTowards(obj.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    private void PlaySound()
    {
        if (audioSource != null && doorSound != null)
        {
            audioSource.PlayOneShot(doorSound);
        }
    }
}




