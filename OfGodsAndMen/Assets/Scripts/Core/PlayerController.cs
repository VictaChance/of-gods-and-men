using System;
using UnityEngine;

// PlayerController manages player movement with WASD/arrow keys and jumping
public class PlayerController : MonoBehaviour
{
    // Movement settings
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    // Camera target
    public Transform cameraTarget; // Add this field

    // Components
    private Rigidbody rb;
    private Animator animator;
    
    // State
    private bool isGrounded;
    
    // Animation parameters
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int IsJumping = Animator.StringToHash("IsJumping");
    
    private void Start()
    {
        // Get components
        rb = GetComponent<Rigidbody>();
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
        if (Input.GetButtonDown("Jump") && isGrounded)
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