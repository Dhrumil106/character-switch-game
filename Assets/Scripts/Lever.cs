using System.Collections;
using System.Collections.Generic;
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
    private Animator leverAnimator; // Reference to the lever's Animator

    public float doorDelay = 1.0f; // Delay before doors start opening/closing

    private void Start()
    {
        // Store the initial local positions of both doors
        door1ClosedLocalPosition = door1.transform.localPosition;
        door2ClosedLocalPosition = door2.transform.localPosition;

        // Get the Animator component on the lever
        leverAnimator = GetComponent<Animator>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E) && isInteractable)
        {
            ToggleLever();
        }
    }

    void ToggleLever()
    {
        if (!isInteractable) return; // Prevent multiple interactions at the same time

        isInteractable = false; // Disable interactions during the toggle process

        // Log current state for debugging
        Debug.Log($"Toggle Lever: isDoorOpen = {isDoorOpen}");

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
        StartCoroutine(MoveDoor(door1, door1ClosedLocalPosition, door1OpenLocalPosition));
        StartCoroutine(MoveDoor(door2, door2ClosedLocalPosition, door2OpenLocalPosition));
    }

    public void CloseDoors()
    {
        StopAllCoroutines(); // Stop any ongoing movement
        StartCoroutine(MoveDoor(door1, door1OpenLocalPosition, door1ClosedLocalPosition));
        StartCoroutine(MoveDoor(door2, door2OpenLocalPosition, door2ClosedLocalPosition));
    }

    System.Collections.IEnumerator MoveDoor(GameObject door, Vector3 startPosition, Vector3 endPosition)
    {
        float elapsedTime = 0;

        while (elapsedTime < 1)
        {
            // Smoothly move the door between start and end local positions
            door.transform.localPosition = Vector3.Lerp(startPosition, endPosition, elapsedTime);
            elapsedTime += Time.deltaTime * doorMoveSpeed;
            yield return null;
        }

        door.transform.localPosition = endPosition; // Ensure exact local position
    }

    void ResetInteractable()
    {
        isInteractable = true; // Re-enable interaction
    }
}
