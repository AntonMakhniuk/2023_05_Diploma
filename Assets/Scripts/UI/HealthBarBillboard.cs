using UnityEngine;

public class HealthBarBillboard : MonoBehaviour
{
    [SerializeField] private Transform playerCamera;

    private void LateUpdate()
    {
        if (playerCamera != null)
        {
            // Rotate the health bar to always face the player camera
            transform.LookAt(transform.position + playerCamera.forward);
        }
    }
}
