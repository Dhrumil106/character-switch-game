using cakeslice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour
{
    [SerializeField] private float rotationAmount = 10f; // Degrees to rotate per press
    [SerializeField] private float rotationDelay = 0.5f; // Time delay between each rotation (in seconds)
    [SerializeField] private KeyCode interactKey = KeyCode.E; // Interaction key
    [SerializeField] private string playerTag = "Player"; // Tag for the player
    public Outline outline;
    private float timeSinceLastRotation = 0f; // Timer for controlling the rotation speed
    private bool isPlayerInRange = false; // Track whether the player is within the trigger

    private void Start()
    {
        
        outline.enabled = false;
    }
    private void Update()
    {
        if (isPlayerInRange && Input.GetKey(interactKey))
        {
            timeSinceLastRotation += Time.deltaTime; // Increment the timer

            // Check if the time since the last rotation exceeds the delay
            if (timeSinceLastRotation >= rotationDelay)
            {
                RotateObject();
                timeSinceLastRotation = 0f; // Reset the timer after rotating
            }
        }

    }

    private void RotateObject()
    {
        // Rotate the object by the specified amount
        transform.Rotate(0, rotationAmount, 0, Space.Self);
    }


    private void OnTriggerStay(Collider other)
    {
        var thirdPerson = other.GetComponent<ThirdPerson>();
        if (other.CompareTag("Player")  && thirdPerson.enabled == true)
        {
            isPlayerInRange = true;
        }
        if (other.CompareTag("Player") && thirdPerson.enabled == true)
        {
            outline.enabled = true;
        }
        if ((thirdPerson == null || !thirdPerson.enabled) && other.CompareTag("Player"))
        {
            outline.enabled = false;
            isPlayerInRange = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the object leaving the trigger has the correct tag
        if (other.CompareTag(playerTag))
        {
            isPlayerInRange = false;
            outline.enabled = false;
        }
    }

}
