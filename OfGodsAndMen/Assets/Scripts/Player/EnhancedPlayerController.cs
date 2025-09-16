using System;
using UnityEngine;
using UnityEngine.InputSystem;

// EnhancedPlayerController manages player movement with WASD/arrow keys and jumping, with additional features
public class EnhancedPlayerController : MonoBehaviour
{
    // Movement settings
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float sprintMultiplier = 2f;
    public float rotationSpeed = 10f;
    
    // Components
    private Rigidbody rb;
    private Animator animator;
    private Camera playerCamera;
    
    // State
    private bool isGrounded;
    private bool isSprinting = false;
    private Vector2 moveInput; // Stores movement input
    private bool jumpInput;    // Stores jump input
    private bool sprintInput;    // Stores sprint input

    // Animation parameters
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int IsJumping = Animator.StringToHash("IsJumping");
    private static readonly int IsSprinting = Animator.StringToHash("IsSprinting");
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");
    
    private void Start()
    {
        // Get components
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        playerCamera = Camera.main;
        
        // Ensure we have required components
        if (rb == null)
        {
            Debug.LogError("EnhancedPlayerController requires a Rigidbody component");
        }
        
        if (animator == null)
        {
            Debug.LogError("EnhancedPlayerController requires an Animator component");
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // Capture movement input
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        // Capture jump input
        if (context.performed)
        {
            jumpInput = true;
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        // Capture sprint input
        isSprinting = context.ReadValue<float>() > 0;
    }


    private void Update()
    {
        // Handle jumping
        if (jumpInput && isGrounded)
        {
            Jump();
            jumpInput = false; // Reset jump input
        }

        // Handle sprinting
        isSprinting = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        
        // Update animation parameters
        if (animator != null)
        {
            // Calculate movement speed for animation
            float currentSpeed = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z).magnitude;
            animator.SetFloat(Speed, currentSpeed);
            
            // Update jumping state
            animator.SetBool(IsJumping, !isGrounded);
            
            // Update sprinting state
            animator.SetBool(IsSprinting, isSprinting);
            
            // Update walking state (when moving but not sprinting)
            animator.SetBool(IsWalking, currentSpeed > 0.1f && !isSprinting);
        }
    }
    
    private void FixedUpdate()
    {
        // Handle movement
        MovePlayer();
        
        // Handle rotation
        RotatePlayer();
        
        // Check if grounded
        CheckGrounded();
    }
    
    private void MovePlayer()
    {
        // Get input
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        
        // Create movement vector
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        
        // Normalize movement vector for diagonal movement
        if (movement.magnitude > 1f)
        {
            movement.Normalize();
        }
        
        // Apply sprint multiplier
        float currentMoveSpeed = isSprinting ? moveSpeed * sprintMultiplier : moveSpeed;
        
        // Apply movement
        if (rb != null)
        {
            rb.linearVelocity = new Vector3(movement.x * currentMoveSpeed, rb.linearVelocity.y, movement.z * currentMoveSpeed);
        }
    }
    
    private void RotatePlayer()
    {
        if (playerCamera == null) return;
        
        // Get input
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        
        // Only rotate when moving
        if (Mathf.Abs(moveHorizontal) > 0.1f || Mathf.Abs(moveVertical) > 0.1f)
        {
            // Calculate direction vector
            Vector3 direction = new Vector3(moveHorizontal, 0.0f, moveVertical);
            
            // Rotate player to face movement direction
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
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