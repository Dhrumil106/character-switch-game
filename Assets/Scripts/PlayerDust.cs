using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDust : MonoBehaviour
{
    public GameObject dustPrefab; // Assign the dust prefab in the Inspector
    public Transform leftFoot;    // Assign the left foot transform
    public Transform rightFoot;   // Assign the right foot transform
    public float dustLifetime = 2f; // Time in seconds before the dust particle is destroyed

    private bool isLeftStep = true;
    private float stepDelay = 0.5f; // Time delay between steps
    private float nextStepTime;

    private CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        if (!dustPrefab)
        {
            Debug.LogError("Dust Prefab is not assigned!");
        }
        if (!leftFoot || !rightFoot)
        {
            Debug.LogError("Left or Right Foot transform is not assigned!");
        }
    }

    void Update()
    {
        // Check if the player is moving
        if (characterController.velocity.magnitude > 0.1f && IsGrounded())
        {
            if (Time.time > nextStepTime)
            {
                CreateDust();
                nextStepTime = Time.time + stepDelay; // Set the time for the next step
            }
        }
    }

    private void CreateDust()
    {
        // Determine which foot is stepping and spawn the dust at that position
        Transform footTransform = isLeftStep ? leftFoot : rightFoot;
        GameObject dustInstance = Instantiate(dustPrefab, footTransform.position, Quaternion.identity);

        // Destroy the dust instance after the specified lifetime
        Destroy(dustInstance, dustLifetime);

        // Alternate between left and right steps
        isLeftStep = !isLeftStep;
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 0.1f);
    }
}
