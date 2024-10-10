using Player;
using UnityEngine;

namespace Testing
{
    public class CursorMovements : MonoBehaviour
    {
        [SerializeField] private Transform ship;
        [SerializeField] private float distanceFromShip = 15f; 
        [SerializeField] private float followSmoothing = 0.1f;
        [SerializeField] private float cursorMovementScale = 0.1f;

        private Vector3 currentVelocity = Vector3.zero;
        private PlayerInputActions _playerInputActions;
        private Vector3 targetPosition;

        private void Awake()
        {
            _playerInputActions = PlayerActions.InputActions;
            _playerInputActions.PlayerCamera.Enable();
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
            
            Camera cam = Camera.main;
            
            Vector3 rightMovement = cam.transform.right * mouseDelta.x;
            Vector3 forwardMovement = cam.transform.up * mouseDelta.y;
            
            Vector3 deltaWorld = (rightMovement + forwardMovement) * cursorMovementScale;
            
            targetPosition += deltaWorld;
        }

        private void FixedUpdate()
        {
            Vector3 smoothPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, followSmoothing);
            transform.position = smoothPosition;
        }
    }
}
