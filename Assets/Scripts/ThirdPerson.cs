using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPerson : MonoBehaviour
{
    [Header("References")]
    private CharacterController controller;
    public Animator anim;
    public Elevator elevatorScript;
    [SerializeField] private Transform camera;
    [SerializeField] private AudioSource audioSource; // Reference to the AudioSource for footstep sounds
    [SerializeField] private AudioClip footstepSound; // Footstep sound effect
    [SerializeField] private AudioClip jumpSound; // Jump sound effect

    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float turningspeed = 2f;
    [SerializeField] private float sprintSpeed = 5f;
    [SerializeField] private float sprintTransitSpeed = 2.5f;
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float footstepInterval = 0.5f; // Interval between footstep sounds

    private float verticalVelocity;
    private float speed;
    private float footstepTimer;

    [Header("Input")]
    private float moveInput;
    private float turnInput;

    [Header("Animation")]
    private int animMoveSpeed;
    private int animJump;
    private int animGrounded;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = gameObject.GetComponentInChildren<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SetUpAnimation();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Update()
    {
        if (!elevatorScript.isHeadCollidingWithElevator)
        {
            InputManagement();
            Movement();
            PlayFootstepSound();
        }
    }

    private void Movement()
    {
        GroundMovement();
        Turn();
    }

    private void GroundMovement()
    {
        Vector3 move = new Vector3(turnInput, 0, moveInput);
        move = camera.transform.TransformDirection(move).normalized;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = Mathf.Lerp(speed, sprintSpeed, sprintTransitSpeed * Time.deltaTime);
        }
        else
        {
            speed = Mathf.Lerp(speed, walkSpeed, sprintTransitSpeed * Time.deltaTime);
        }

        move.y = 0;

        move *= speed;

        move.y = verticalForceCalculation();

        controller.Move(move * Time.deltaTime);

        // Animation
        anim.SetFloat(animMoveSpeed, speed * Mathf.Max(Mathf.Abs(moveInput), Mathf.Abs(turnInput)));
    }

    private void InputManagement()
    {
        moveInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");
    }

    private void Turn()
    {
        if (Mathf.Abs(turnInput) > 0 || Mathf.Abs(moveInput) > 0)
        {
            Vector3 currentLookDirection = controller.velocity.normalized;
            currentLookDirection.y = 0;

            currentLookDirection.Normalize();

            Quaternion targetRotation = Quaternion.LookRotation(currentLookDirection);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turningspeed);
        }
    }

    private float verticalForceCalculation()
    {
        if (controller.isGrounded)
        {
            verticalVelocity = -1f;
            anim.SetBool(animGrounded, true);
            if (Input.GetButtonDown("Jump"))
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * gravity * 2);

                anim.SetTrigger(animJump);
                audioSource.PlayOneShot(jumpSound); // Play jump sound when jumping
            }
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
            anim.SetBool(animGrounded, false);
        }
        return verticalVelocity;
    }

    private void PlayFootstepSound()
    {
        if (controller.isGrounded && (Mathf.Abs(moveInput) > 0 || Mathf.Abs(turnInput) > 0))
        {
            footstepTimer += Time.deltaTime;
            if (footstepTimer >= footstepInterval)
            {
                footstepTimer = 0;
                audioSource.PlayOneShot(footstepSound);
            }
        }
        else
        {
            footstepTimer = 0; // Reset timer if the player isn't moving
        }
    }

    private void SetUpAnimation()
    {
        animMoveSpeed = Animator.StringToHash("MoveSpeed");
        animJump = Animator.StringToHash("Jump");
        animGrounded = Animator.StringToHash("Grounded");
    }
}