using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class passiveGravity : MonoBehaviour
{
    private CharacterController controller;
    private Animator anim;
    private float verticalVelocity = 0f;
    private float gravity = 9.81f;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!enabled) return;

        // Apply gravity
        verticalVelocity -= gravity * Time.deltaTime;

        // Move the character down
        Vector3 move = new Vector3(0, verticalVelocity, 0);
        controller.Move(move * Time.deltaTime);

        // Reset vertical velocity when grounded
        if (controller.isGrounded)
        {
            verticalVelocity = -1f;
            anim.SetBool("Grounded", true);
        }
    }


}
