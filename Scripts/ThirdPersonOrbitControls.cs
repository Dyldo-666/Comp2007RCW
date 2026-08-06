using UnityEngine;

public class ThirdPersonOrbitControls : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform target;
    [SerializeField] private Transform cameraPivot;

    [Header("Camera settings")]
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float minimumPitch = -25f;
    [SerializeField] private float maximumPitch = 60f;

    private float yaw;
    private float pitch = 15f;

    private void Start()
    {
        if (target != null)
        {
            yaw = target.eulerAngles.y;
        }
    }

    private void LateUpdate()
    {
        // Keep the camera still while a menu is open.
        if (Time.timeScale == 0f)
        {
            return;
        }

        float mouseX =
            Input.GetAxis("Mouse X") * mouseSensitivity;

        float mouseY =
            Input.GetAxis("Mouse Y") * mouseSensitivity;

        yaw += mouseX;
        pitch -= mouseY;

        pitch = Mathf.Clamp(
            pitch,
            minimumPitch,
            maximumPitch
        );

        cameraPivot.rotation =
            Quaternion.Euler(pitch, yaw, 0f);
    }
}
