using System;
using Player;
using UnityEngine;

namespace Testing
{
    public class CursorMovements : MonoBehaviour
    {
        [SerializeField] private float followSmoothing = 0.1f;
        [SerializeField] private float cursorMovementScale = 0.1f;
        [SerializeField] private float distanceToShip = 20f;
        [SerializeField] private Transform droneSetupCenter;
        

        private Vector3 _currentVelocity = Vector3.zero;
        private PlayerInputActions _playerInputActions;
        private Vector3 _targetPosition;
        private Vector3 _posCenteredUnscaled;
        private Transform _mainCam;
       


        private void Awake()
        {
            _playerInputActions = PlayerActions.InputActions;
            _playerInputActions.PlayerCamera.Enable();

            _mainCam = Camera.main.transform;
        }

        private void OnEnable()
        {
            _playerInputActions.PlayerCamera.CameraMovement.performed += OnMouseMovement;
        }

        private void OnDisable()
        {
            _playerInputActions.PlayerCamera.CameraMovement.performed -= OnMouseMovement;
        }

        private void OnMouseMovement(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            Vector2 mouseDelta = _playerInputActions.PlayerCamera.CameraMovement.ReadValue<Vector2>();
            
            Vector3 rightMovement = _mainCam.right * mouseDelta.x;
            Vector3 forwardMovement = _mainCam.up * mouseDelta.y;
            
            Vector3 deltaWorld = (rightMovement + forwardMovement) * cursorMovementScale;

            _posCenteredUnscaled = ((_targetPosition + deltaWorld) - droneSetupCenter.position);
            
            _targetPosition = distanceToShip / Mathf.Sqrt(
                Mathf.Pow(_posCenteredUnscaled.x, 2) +  
                Mathf.Pow(_posCenteredUnscaled.y, 2) + 
                Mathf.Pow(_posCenteredUnscaled.z, 2)) * _posCenteredUnscaled + droneSetupCenter.position;
        }

        private void FixedUpdate()
        {
            
            Vector3 smoothPosition = Vector3.SmoothDamp(transform.position, _targetPosition, ref _currentVelocity, followSmoothing);
            transform.position = smoothPosition;
        }
    }
}
