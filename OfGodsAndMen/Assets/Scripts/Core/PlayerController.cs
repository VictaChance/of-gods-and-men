using System;
using UnityEngine;
using UnityEngine.InputSystem;

// PlayerController manages player movement with WASD/arrow keys and jumping
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float sprintSpeed = 8f;
    public float jumpForce = 5f;

    [Header("Player Stats")]
    public float maxHealth = 100f;
    public float currentHealth = 100f;

    private CharacterController controller;
    private PlayerController playerController;
    private Rigidbody rb; // Keep this declaration
    private Vector3 velocity;
    private bool isGrounded;

    // Ground check object
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    // Camera target
    public Transform cameraTarget;

    // Components
    private Animator animator;

    // Animation parameters
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int IsJumping = Animator.StringToHash("IsJumping");

    // Input System
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;

    private void Awake()
    {
        // Get the character controller component
        controller = GetComponent<CharacterController>();

        // Create ground check if missing
        if (groundCheck == null)
        {
            GameObject groundCheckObj = new GameObject("GroundCheck");
            groundCheckObj.transform.SetParent(transform);
            groundCheckObj.transform.localPosition = new Vector3(0, -0.5f, 0);
            groundCheck = groundCheckObj.transform;
        }

        rb = GetComponent<Rigidbody>();

        // Initialize Input System
        playerInput = GetComponent<PlayerInput>();
        if (playerInput == null)
        {
            Debug.LogError("PlayerInput component is missing on this GameObject.");
            return;
        }

        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
    }

    private void Start()
    {
        // Get components
        animator = GetComponent<Animator>();

        // Ensure we have required components
        if (rb == null)
        {
            Debug.LogError("PlayerController requires a Rigidbody component");
        }

        if (animator == null)
        {
            Debug.LogError("PlayerController requires an Animator component");
        }
    }

    private void Update()
    {
        // Handle jumping
        if (jumpAction.triggered && isGrounded)
        {
            Jump();
        }

        // Update animation parameters
        if (animator != null)
        {
            // Calculate movement speed for animation
            float currentSpeed = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z).magnitude;
            animator.SetFloat(Speed, currentSpeed);

            // Update jumping state
            animator.SetBool(IsJumping, !isGrounded);
        }
    }

    private void FixedUpdate()
    {
        // Handle movement
        MovePlayer();

        // Check if grounded
        CheckGrounded();
    }

    private void MovePlayer()
    {
        // Get input from the New Input System
        Vector2 input = moveAction.ReadValue<Vector2>();

        // Create movement vector
        Vector3 movement = new Vector3(input.x, 0.0f, input.y);

        // Normalize movement vector for diagonal movement
        if (movement.magnitude > 1f)
        {
            movement.Normalize();
        }

        // Apply movement
        if (rb != null)
        {
            rb.linearVelocity = new Vector3(movement.x * moveSpeed, rb.linearVelocity.y, movement.z * moveSpeed);
        }
    }

    private void Jump()
    {
        // Apply jump force
        if (rb != null)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void CheckGrounded()
    {
        // Simple grounded check using raycast
        if (rb != null)
        {
            RaycastHit hit;
            isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, 1.1f);
        }
    }
}