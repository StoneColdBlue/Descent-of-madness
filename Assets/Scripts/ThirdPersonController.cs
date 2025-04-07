using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public float gravity = -9.81f;
    public float jumpHeight = 2f;

    [Header("Camera Settings")]
    public Transform cameraPivot;
    public float cameraDistance = 3f;
    public float cameraHeight = 1.5f;
    public float cameraRotationSpeed = 2f;
    public float minPitchAngle = -30f;
    public float maxPitchAngle = 60f;

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool isGrounded;
    private float cameraPitch;
    private float cameraYaw;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;

        // Initialize camera angles
        cameraYaw = transform.eulerAngles.y;
        cameraPitch = cameraPivot.localEulerAngles.x;
    }

    void Update()
    {
        HandleMovement();
        HandleCameraRotation();
    }

    void HandleMovement()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }

        // Get input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(horizontal, 0, vertical).normalized;

        // Convert input to world space relative to camera
        if (moveDirection.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + cameraYaw;
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir * moveSpeed * Time.deltaTime);

            // Smooth rotation towards movement direction
            Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Apply gravity
        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    void HandleCameraRotation()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * cameraRotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * cameraRotationSpeed;

        // Rotate player horizontally with mouse
        cameraYaw += mouseX;
        transform.rotation = Quaternion.Euler(0f, cameraYaw, 0f);

        // Rotate camera vertically
        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, minPitchAngle, maxPitchAngle);
        cameraPivot.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);

        // Position camera behind player
        Vector3 cameraOffset = -cameraPivot.forward * cameraDistance;
        cameraOffset.y = cameraHeight;
        Camera.main.transform.position = cameraPivot.position + cameraOffset;
        Camera.main.transform.LookAt(cameraPivot.position);
    }
}