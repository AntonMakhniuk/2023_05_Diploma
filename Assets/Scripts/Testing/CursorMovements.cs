using Cinemachine;
using UnityEngine;

public class CursorMovements : MonoBehaviour
{
    [SerializeField] private Transform ship;
    [SerializeField] private CinemachineFreeLook freeLookCamera;
    [SerializeField] private float distanceFromShip = 15f;
    [SerializeField] private float followSmoothing = 0.1f; // Add smoothing for camera following

    private Vector3 currentVelocity = Vector3.zero;

    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        Vector3 targetPosition = ray.GetPoint(distanceFromShip);
        targetPosition.z = Mathf.Clamp(targetPosition.z, ship.position.z - 20, ship.position.z + 20);

        // Apply smoothing to camera movement to match the ship's movement
        Vector3 smoothPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, followSmoothing);
        transform.position = smoothPosition;
    }
}
