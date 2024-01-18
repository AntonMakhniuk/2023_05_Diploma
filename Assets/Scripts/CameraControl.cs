using System.Collections;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform target; // Target point to orbit around
    public float rotationSpeed = 3f;
    public float shipMoveSpeed = 5f;
    private Quaternion initialShipRotation;

    private Vector3 preRotationCameraPosition;
    private Quaternion preRotationCameraRotation;


    private bool isRotating = false;

    void Update()
    {
        OrbitCamera();
        MoveShipWithCamera();
    }

    private void OrbitCamera()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Reverse the rotation direction by multiplying with -1
        mouseX *= -1f;
        mouseY *= -1f;

        // Rotate the camera around the target based on mouse input
        transform.RotateAround(target.position, Vector3.up, mouseX * rotationSpeed);
        transform.RotateAround(target.position, transform.right, mouseY * rotationSpeed);

        // Ensure the camera is always looking at the target
        transform.LookAt(target);
    }

    private IEnumerator RotateShipSmoothly(Quaternion targetRotation)
    {
        if (isRotating) yield break;

        isRotating = true;

        // Detach the camera from the ship
        preRotationCameraPosition = transform.position;
        preRotationCameraRotation = transform.localRotation;

        transform.parent = null;

        float elapsedTime = 0f;
        float duration = 1f; // Adjust the rotation duration as needed

        Quaternion startRotation = target.rotation;

        while (elapsedTime < duration)
        {
            transform.position = preRotationCameraPosition;
            transform.localRotation = targetRotation;
            target.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        target.rotation = targetRotation;

        // Set camera's local rotation and position to (0, 0, 0) after the rotation is complete

        // Reattach the camera to the ship
        transform.parent = target;

        transform.localRotation = Quaternion.identity;
        transform.localPosition = Vector3.zero;

        isRotating = false;
    }

    private void MoveShipWithCamera()
    {
        MovementInputSystem movementInput = target.GetComponent<MovementInputSystem>();
        initialShipRotation = target.rotation;

        // Use turning left and right from the CameraControl script
        if (Input.GetMouseButton(0) && Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X");

            // Rotate the ship (target) based on the camera's rotation
            Vector3 cameraRotation = transform.localRotation.eulerAngles;
            Quaternion newShipRotation = initialShipRotation * Quaternion.Euler(cameraRotation);

            // Smoothly rotate the ship
            StartCoroutine(RotateShipSmoothly(newShipRotation));

            // Ensure the camera is always looking at the target
            transform.LookAt(target);

            // Keep the camera behind the ship by adjusting its local position
            Vector3 desiredCameraPosition = -target.forward * 10f; // Adjust the distance as needed
            transform.localPosition = Vector3.Lerp(transform.localPosition, desiredCameraPosition, Time.deltaTime * shipMoveSpeed);
        }
    }
}
