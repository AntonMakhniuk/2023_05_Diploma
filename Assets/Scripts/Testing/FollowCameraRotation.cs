using UnityEngine;

public class FollowCameraRotation : MonoBehaviour
{
    public Transform cameraTransform;
    public float rotationSpeed = 5f;

    void Update()
    {
        Quaternion targetRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
