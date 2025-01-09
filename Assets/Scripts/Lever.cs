using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public GameObject door; // Reference to the door object
    public Vector3 doorOpenPosition; // Target position for the open door
    public Vector3 doorClosedPosition; // Target position for the closed door
    public float doorMoveSpeed = 2.0f; // Speed at which the door moves
    public float leverRotationAngle = 45f; // How far the lever rotates
    private bool isDoorOpen = false; // To track the door state

    private Quaternion initialLeverRotation;

    private void Start()
    {
        initialLeverRotation = transform.rotation;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.P))
        {
            ToggleLever();
        }
    }

    void ToggleLever()
    {
        if (isDoorOpen)
        {
            CloseDoor();
            RotateLeverBack();
        }
        else
        {
            OpenDoor();
            RotateLeverForward();
        }

        isDoorOpen = !isDoorOpen; // Toggle the door state
    }

    void OpenDoor()
    {
        StopAllCoroutines(); // Stop any ongoing movement
        StartCoroutine(MoveDoor(doorClosedPosition, doorOpenPosition));
    }

    void CloseDoor()
    {
        StopAllCoroutines(); // Stop any ongoing movement
        StartCoroutine(MoveDoor(doorOpenPosition, doorClosedPosition));
    }

    System.Collections.IEnumerator MoveDoor(Vector3 startPosition, Vector3 endPosition)
    {
        float elapsedTime = 0;
        while (elapsedTime < 1)
        {
            door.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime);
            elapsedTime += Time.deltaTime * doorMoveSpeed;
            yield return null;
        }
        door.transform.position = endPosition; // Ensure exact position
    }

    void RotateLeverForward()
    {
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x - leverRotationAngle, transform.eulerAngles.y, transform.eulerAngles.z);
    }

    void RotateLeverBack()
    {
        transform.rotation = initialLeverRotation;
    }
}
