using UnityEngine;

public class ThirdPersonCameraCollision : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform cameraTransform;

    [SerializeField] private float normalDistance = 4f;
    [SerializeField] private float minimumDistance = 0.5f;
    [SerializeField] private float collisionOffset = 0.2f;
    [SerializeField] private float cameraMoveSpeed = 12f;

    [SerializeField] private LayerMask collisionLayers = ~0;

    private void LateUpdate()
    {
        if (target == null || cameraTransform == null)
        {
            return;
        }

        Vector3 desiredDirection =
            -cameraTransform.parent.forward;

        float desiredDistance = normalDistance;

        if (
            Physics.Raycast(
                target.position,
                desiredDirection,
                out RaycastHit hit,
                normalDistance,
                collisionLayers,
                QueryTriggerInteraction.Ignore
            )
        )
        {
            desiredDistance = Mathf.Max(
                minimumDistance,
                hit.distance - collisionOffset
            );
        }

        Vector3 targetLocalPosition =
            new Vector3(0f, 0f, -desiredDistance);

        cameraTransform.localPosition =
            Vector3.Lerp(
                cameraTransform.localPosition,
                targetLocalPosition,
                cameraMoveSpeed * Time.deltaTime
            );
    }
}