using cakeslice;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Lever : MonoBehaviour
{

    public GameObject door1; // First sliding door
    public GameObject door2; // Second sliding door
    public Vector3 door1OpenLocalPosition; // Open position for the first door
    public Vector3 door2OpenLocalPosition; // Open position for the second door
    public float doorMoveSpeed = 2.0f; // Speed at which the doors move
    private bool isDoorOpen = false; // To track the door state
    private bool isInteractable = true; // Prevent multiple interactions at once

    private Vector3 door1ClosedLocalPosition; // Initial closed position of the first door
    private Vector3 door2ClosedLocalPosition; // Initial closed position of the second door
    private Rigidbody door1Rigidbody; // Rigidbody for door1
    private Rigidbody door2Rigidbody; // Rigidbody for door2
    private Animator leverAnimator; // Reference to the lever's Animator
    public Outline outline;
    public float doorDelay = 1.0f; // Delay before doors start opening/closing

    public AudioSource leverSound; // AudioSource for the lever sound
    public AudioSource doorSound; // Shared AudioSource for the door sound effect

    private void Start()
    {
        // Store the initial local positions of both doors
        door1ClosedLocalPosition = door1.transform.localPosition;
        door2ClosedLocalPosition = door2.transform.localPosition;

        // Get Rigidbody components
        door1Rigidbody = door1.GetComponent<Rigidbody>();
        door2Rigidbody = door2.GetComponent<Rigidbody>();

        // Get the Animator component on the lever
        leverAnimator = GetComponent<Animator>();
        outline.enabled = false;
    }

    private void OnTriggerStay(Collider other)
    {
        var thirdPerson = other.GetComponent<ThirdPerson>();
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E) && isInteractable && thirdPerson.enabled == true)
        {
            ToggleLever();
        }
        if (other.CompareTag("Player") && isInteractable && thirdPerson.enabled == true)
        {
            outline.enabled = true;
        }
        if ((thirdPerson == null || thirdPerson.enabled == false) && other.CompareTag("Player"))
        {
            outline.enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var thirdPerson = other.GetComponent<ThirdPerson>();
        if (other.CompareTag("Player") && thirdPerson.enabled == true)
        {
            outline.enabled = false;
        }
    }

    void ToggleLever()
    {
        if (!isInteractable) return; // Prevent multiple interactions at the same time

        isInteractable = false; // Disable interactions during the toggle process

        // Play lever interaction sound
        if (leverSound != null)
        {
            leverSound.Play();
        }

        if (isDoorOpen)
        {
            leverAnimator.SetTrigger("RotateBack"); // Trigger animation to rotate the lever back
            Invoke(nameof(CloseDoors), doorDelay); // Wait before closing doors
        }
        else
        {
            leverAnimator.SetTrigger("RotateForward"); // Trigger animation to rotate the lever forward
            Invoke(nameof(OpenDoors), doorDelay); // Wait before opening doors
        }

        isDoorOpen = !isDoorOpen; // Toggle the state
        Invoke(nameof(ResetInteractable), 0.5f); // Allow interaction again after a short delay
    }

    public void OpenDoors()
    {
        StopAllCoroutines(); // Stop any ongoing movement

        // Play shared door sound
        if (doorSound != null)
        {
            doorSound.Play();
        }

        StartCoroutine(MoveDoor(door1Rigidbody, door1ClosedLocalPosition, door1OpenLocalPosition));
        StartCoroutine(MoveDoor(door2Rigidbody, door2ClosedLocalPosition, door2OpenLocalPosition));
    }

    public void CloseDoors()
    {
        StopAllCoroutines(); // Stop any ongoing movement

        // Play shared door sound
        if (doorSound != null)
        {
            doorSound.Play();
        }

        StartCoroutine(MoveDoor(door1Rigidbody, door1OpenLocalPosition, door1ClosedLocalPosition));
        StartCoroutine(MoveDoor(door2Rigidbody, door2OpenLocalPosition, door2ClosedLocalPosition));
    }

    System.Collections.IEnumerator MoveDoor(Rigidbody doorRigidbody, Vector3 startPosition, Vector3 endPosition)
    {
        float elapsedTime = 0;
        float duration = 1f / doorMoveSpeed;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            Vector3 newPosition = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);

            // Move the Rigidbody to the new position
            doorRigidbody.MovePosition(doorRigidbody.transform.parent.TransformPoint(newPosition));
            yield return null;
        }

        // Ensure the final position is exact
        doorRigidbody.MovePosition(doorRigidbody.transform.parent.TransformPoint(endPosition));
    }

    void ResetInteractable()
    {
        isInteractable = true; // Re-enable interaction
    }
}
