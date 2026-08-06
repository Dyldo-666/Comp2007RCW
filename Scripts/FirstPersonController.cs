using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstPersonController : MonoBehaviour
{
    public float speed = 5f;
    public float mouseSensitivity = 2f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;

    public Transform cameraTransform;

    private CharacterController controller;
    private Vector3 velocity;
    private float xRotation = 0f;

void Start()
{
    controller = GetComponent<CharacterController>();

    if (cameraTransform == null)
    {
        cameraTransform = GetComponentInChildren<Camera>().transform;
    }

    Cursor.lockState = CursorLockMode.None;
    Cursor.visible = true;
}
    void Update()
{
    if (Time.timeScale == 0f) return;

    MovePlayer();
    LookAround();
}
void MovePlayer()
{
    bool grounded = controller.isGrounded;

    if (grounded && velocity.y < 0)
    {
        velocity.y = -2f;
    }

    float x = Input.GetAxis("Horizontal");
    float z = Input.GetAxis("Vertical");

    Vector3 move = transform.right * x + transform.forward * z;
    controller.Move(move * speed * Time.deltaTime);

    if (Input.GetKeyDown(KeyCode.Space) && grounded)
    {
        velocity.y = 7f;
    }

    velocity.y += gravity * Time.deltaTime;
    controller.Move(velocity * Time.deltaTime);
}
    void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}