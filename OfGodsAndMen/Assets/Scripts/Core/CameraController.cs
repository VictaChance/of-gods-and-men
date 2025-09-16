using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float distance = 5f;
    public float height = 2f;
    public float smoothSpeed = 10f;
    public float rotationSmoothSpeed = 5f;

    public float minVerticalAngle = -30f;
    public float maxVerticalAngle = 60f;

    public float inputSensitivity = 2f;

    private float rotationX = 0f;
    private float rotationY = 0f;

    private Vector3 currentRotation;
    private Vector3 smoothVelocity = Vector3.zero;

    private void Start()
    {
        // Find player if target not assigned
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                PlayerController playerController = player.GetComponent<PlayerController>();
                if (playerController != null && playerController.cameraTarget != null)
                {
                    target = playerController.cameraTarget;
                }
                else
                {
                    target = player.transform;
                }
            }
        }

        // Initialize rotation
        rotationY = transform.eulerAngles.y;

        // Lock and hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // Only process input if target exists
        if (target == null)
            return;

        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * inputSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * inputSensitivity;

        // Adjust rotation
        rotationY += mouseX;
        rotationX -= mouseY;

        // Clamp vertical rotation
        rotationX = Mathf.Clamp(rotationX, minVerticalAngle, maxVerticalAngle);

        // Create target rotation
        Vector3 targetRotation = new Vector3(rotationX, rotationY, 0);

        // Smoothly interpolate current rotation
        currentRotation = Vector3.SmoothDamp(currentRotation, targetRotation, ref smoothVelocity, 1f / rotationSmoothSpeed);

        // Apply rotation
        transform.eulerAngles = currentRotation;

        // Calculate position
        Vector3 targetPosition = target.position - transform.forward * distance + Vector3.up * height;

        // Smoothly move camera
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void ToggleCursorLock()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}