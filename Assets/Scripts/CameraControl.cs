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

        mouseX *= -1f;
        mouseY *= -1f;

        transform.RotateAround(target.position, Vector3.up, mouseX * rotationSpeed);
        transform.RotateAround(target.position, transform.right, mouseY * rotationSpeed);

        transform.LookAt(target);
    }

    private IEnumerator RotateShipSmoothly(Quaternion targetRotation)
    {
        if (isRotating) yield break;

        isRotating = true;

        preRotationCameraPosition = transform.position;
        preRotationCameraRotation = transform.localRotation;

        transform.parent = null;

        float elapsedTime = 0f;
        float duration = 1f; 

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


        transform.parent = target;

        transform.localRotation = Quaternion.identity;
        transform.localPosition = Vector3.zero;

        isRotating = false;
    }

    private void MoveShipWithCamera()
    {
        MovementInputSystem movementInput = target.GetComponent<MovementInputSystem>();
        initialShipRotation = target.rotation;

        if (Input.GetMouseButton(0) && Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X");

            Vector3 cameraRotation = transform.localRotation.eulerAngles;
            Quaternion newShipRotation = initialShipRotation * Quaternion.Euler(cameraRotation);

            StartCoroutine(RotateShipSmoothly(newShipRotation));

        }
    }
}
