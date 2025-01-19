using cakeslice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever2 : MonoBehaviour
{
    public bool isLeverActivated = false; // Boolean to track if the lever is pulled
    private bool playerInRange = false; // Tracks if the player is in range to interact
    public Outline outline;
    public AudioSource audioSource;
 

    [Header("Animator Settings")]
    public Animator leverAnimator; // Reference to the Animator

    private void Start()
    {
        outline.enabled = false;
    }
    void Update()
    {
        // If the player is in range and presses the "E" key
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            audioSource.Play();
            ToggleLeverState(); // Toggle the lever's state
            
        }
    }

    
    private void OnTriggerStay(Collider other)
    {
        var thirdPerson = other.GetComponent<ThirdPerson>();
        if (other.CompareTag("Player") && thirdPerson.enabled == true)
        {
            playerInRange = true;
        }
        if (other.CompareTag("Player") && thirdPerson.enabled == true)
        {
            outline.enabled = true;
        }
        if ((thirdPerson == null || !thirdPerson.enabled) && other.CompareTag("Player"))
        {
            outline.enabled = false;
            playerInRange = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Check if the player exits the interaction range
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            outline.enabled = false;
            Debug.Log("Player is out of range to interact with the lever.");
        }
    }

    private void ToggleLeverState()
    {
        // Toggle the lever's activation state
        isLeverActivated = !isLeverActivated;

        // Trigger the appropriate animation based on the lever's state
        if (isLeverActivated)
        {
            leverAnimator.SetTrigger("RotateForward"); // Play the "RotateForward" animation
            Debug.Log("Lever activated");
        }
        else
        {
            leverAnimator.SetTrigger("RotateBack"); // Play the "RotateBack" animation
            Debug.Log("Lever deactivated");
        }
    }
}
