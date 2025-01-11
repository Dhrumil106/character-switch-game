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
    public float leverRotationAngle = 45f; // How far the lever rotates
    private bool isDoorOpen = false; // To track the door state
    private bool isInteractable = true; // Prevent multiple interactions at once

    private Vector3 door1ClosedLocalPosition; // Initial closed position of the first door
    private Vector3 door2ClosedLocalPosition; // Initial closed position of the second door
    private Quaternion initialLeverRotation;

    private void Start()
    {
        // Store the initial local positions of both doors
        door1ClosedLocalPosition = door1.transform.localPosition;
        door2ClosedLocalPosition = door2.transform.localPosition;

        // Store the lever's initial rotation
        initialLeverRotation = transform.rotation;
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
        isInteractable = false; // Prevent additional interactions until the toggle completes

        if (isDoorOpen)
        {
            CloseDoors();
            RotateLeverBack();
        }
        else
        {
            OpenDoors();
            RotateLeverForward();
        }

        isDoorOpen = !isDoorOpen; // Toggle the door state
        Invoke(nameof(ResetInteractable), 0.5f); // Re-enable interaction after a short delay
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

    void RotateLeverForward()
    {
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x - leverRotationAngle, transform.eulerAngles.y, transform.eulerAngles.z);
    }

    void RotateLeverBack()
    {
        transform.rotation = initialLeverRotation;
    }

    void ResetInteractable()
    {
        isInteractable = true; // Re-enable interaction
    }
}
