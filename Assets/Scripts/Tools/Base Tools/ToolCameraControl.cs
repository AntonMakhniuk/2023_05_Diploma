using UnityEngine;

namespace Tools.Base_Tool
{
    public class ToolCameraControl : MonoBehaviour 
    {
        private Rigidbody _rb;
        private Transform _mainCamera;
    
        [SerializeField] private Transform barrel;
        [SerializeField] private Transform leg;
        [SerializeField] private float rotationSpeed;
    
        private void Awake()
        {
            if (Camera.main != null)
            {
                _mainCamera = Camera.main.transform;
            }
        }

        private void Update() {
            RotateWithCamera();
        }

        private void RotateWithCamera() 
        {
            var targetRotationLeg = Quaternion.Euler(0, _mainCamera.eulerAngles.y, 0);
            leg.rotation = Quaternion.Lerp(leg.rotation, targetRotationLeg, rotationSpeed * Time.deltaTime);
        
            var targetRotationBarrel = Quaternion.Euler(_mainCamera.eulerAngles.x, leg.eulerAngles.y, 0);
            barrel.rotation = Quaternion.Lerp(barrel.rotation, targetRotationBarrel, rotationSpeed * Time.deltaTime);
        }
    }
}
