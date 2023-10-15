using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;            // Reference to the player's Transform.
    public float smoothSpeed = 5.0f;    // The higher, the faster the camera follows the player

    private Vector3 offset;             // The initial offset between the camera and player

    private void Start()
    {
        // Calculate the initial offset.
        offset = transform.position - target.position;
    }

    private void LateUpdate()
    {
        // Calculate the target position for the camera.
        Vector3 targetPosition = target.position + offset;

        // Smoothly interpolate between the current camera position and the target position.
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }
}
