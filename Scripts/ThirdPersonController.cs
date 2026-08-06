using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform characterModel;
    [SerializeField] private Animator animator;

    [Header("Movement")]
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float runSpeed = 6f;
    [SerializeField] private float rotationSpeed = 12f;

    [Header("Jumping and gravity")]
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float gravity = -20f;

    private CharacterController controller;
    private Vector3 verticalVelocity;

    private static readonly int SpeedParameter =
        Animator.StringToHash("Speed");

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // Prevent movement and animation changes while menus are open.
        if (Time.timeScale == 0f)
        {
            if (animator != null)
            {
                animator.SetFloat(SpeedParameter, 0f);
            }

            return;
        }

        HandleMovement();
        HandleGravityAndJump();
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 inputDirection =
            new Vector3(horizontal, 0f, vertical).normalized;

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        if (inputDirection.sqrMagnitude > 0.01f)
        {
            // Convert input into a direction relative to the camera.
            Vector3 cameraForward = cameraTransform.forward;
            Vector3 cameraRight = cameraTransform.right;

            cameraForward.y = 0f;
            cameraRight.y = 0f;

            cameraForward.Normalize();
            cameraRight.Normalize();

            Vector3 movementDirection =
                cameraForward * inputDirection.z +
                cameraRight * inputDirection.x;

            movementDirection.Normalize();

            controller.Move(
                movementDirection * currentSpeed * Time.deltaTime
            );

            Quaternion targetRotation =
                Quaternion.LookRotation(movementDirection);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        UpdateAnimation(inputDirection, isRunning);
    }

    private void HandleGravityAndJump()
    {
        if (controller.isGrounded && verticalVelocity.y < 0f)
        {
            verticalVelocity.y = -2f;
        }

        if (
            Input.GetKeyDown(KeyCode.Space) &&
            controller.isGrounded
        )
        {
            verticalVelocity.y =
                Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        verticalVelocity.y += gravity * Time.deltaTime;

        controller.Move(verticalVelocity * Time.deltaTime);
    }

    private void UpdateAnimation(
        Vector3 inputDirection,
        bool isRunning
    )
    {
        if (animator == null)
        {
            return;
        }

        float animationSpeed = 0f;

        if (inputDirection.sqrMagnitude > 0.01f)
        {
            animationSpeed = isRunning ? 1f : 0.5f;
        }

        animator.SetFloat(
            SpeedParameter,
            animationSpeed,
            0.1f,
            Time.deltaTime
        );
    }
}