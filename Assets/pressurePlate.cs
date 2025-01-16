using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pressurePlate : MonoBehaviour
{
    [Header("Plate Settings")]
    public float plateDownHeight = 0.2f; // How far the plate moves when pressed
    public float moveSpeed = 2f; // Speed at which the plate moves
    private Vector3 originalPosition; // The original position of the plate
    private HashSet<GameObject> objectsOnPlate = new HashSet<GameObject>(); // Tracks objects on the plate

    public bool IsPressed { get; private set; } // Public property to check if the plate is pressed

    void Start()
    {
        originalPosition = transform.position; // Store the original position of the plate
    }

    void Update()
    {
        // Smoothly move the plate up or down based on its pressed state
        Vector3 targetPosition = IsPressed
            ? originalPosition - new Vector3(0, plateDownHeight, 0)
            : originalPosition;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        // Add object to the plate and update its state
        if (other.CompareTag("Player") || other.CompareTag("Pullable"))
        {
            objectsOnPlate.Add(other.gameObject);
            UpdatePlateState();
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Remove object from the plate and update its state
        if (other.CompareTag("Player") || other.CompareTag("Pullable"))
        {
            objectsOnPlate.Remove(other.gameObject);
            UpdatePlateState();
        }
    }

    private void UpdatePlateState()
    {
        // Plate is pressed if any object is on it
        IsPressed = objectsOnPlate.Count > 0;
    }
}
