using UnityEngine;

public class ShipFollowCursor : MonoBehaviour
{
    [SerializeField] private Transform lookTarget;  // The target the ship will look at (e.g., cursor position)
    [SerializeField] private float rotationSpeed = 2f;  // Speed at which the ship rotates towards the target
    [SerializeField] private float smoothingFactor = 0.1f;  // Damping or smoothing factor for movement

    private Vector3 currentVelocity = Vector3.zero;  // For smoothing the movement

    void Update()
    {
        // Calculate direction towards the look target
        Vector3 directionToTarget = (lookTarget.position - transform.position).normalized;

        // Get the target rotation based on the direction to the target
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget, Vector3.up);

        // Smooth the rotation towards the target using Quaternion.Slerp
        // The smoothedSpeed controls how quickly we rotate towards the target
        float smoothedSpeed = Mathf.Lerp(0f, rotationSpeed, Time.deltaTime / smoothingFactor);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothedSpeed);

        // Optionally, if you also want to smooth the movement towards the target (useful if ship has movement):
        Vector3 smoothPosition = Vector3.SmoothDamp(transform.position, lookTarget.position, ref currentVelocity, smoothingFactor);
        transform.position = smoothPosition;
    }
}
