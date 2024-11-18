using Player;
using Systems.Mining.Tools.Miscellaneous;
using UnityEngine;

namespace Testing
{
    public class CursorMovements : MonoBehaviour
    {
        [SerializeField] private float followSmoothing = 0.15f;
        [SerializeField] private float cursorMovementScale = 0.1f;
        [SerializeField] private float distanceToShip = 20f;
        [SerializeField] private float maxRotationSpeed = 5f;
        [SerializeField] private float deadZoneRadius = 30f;
        [SerializeField] private AnimationCurve rotationSpeedCurve;
        
        [SerializeField] private RectTransform crosshairUI;
        [SerializeField] private Transform droneBody;
        [SerializeField] private Crosshair crosshair;

        private Vector3 _currentVelocity = Vector3.zero;
        private PlayerInputActions _playerInputActions;
        private Vector3 _targetPosition;
        private Vector3 _posCenteredUnscaled;
        private Transform _mainCam;
        private Vector3 _lastDronePosition;

        private void Awake()
        {
            _playerInputActions = PlayerActions.InputActions;
            _playerInputActions.PlayerCamera.Enable();
            _mainCam = Camera.main.transform;
            _lastDronePosition = droneBody.position;
        }

        private void FixedUpdate()
        {
            CalculateTargetRotation(); 
            Vector3 droneMovementDelta = droneBody.position - _lastDronePosition;
            _targetPosition += droneMovementDelta;
            
            Vector3 smoothPosition = Vector3.SmoothDamp(transform.position, _targetPosition, ref _currentVelocity, followSmoothing);
            transform.position = smoothPosition;
            _lastDronePosition = droneBody.position;
        }
        private void CalculateTargetRotation()
        {
            Vector2 crosshairPosition = crosshairUI.localPosition;
            float distanceFromCenter = crosshairPosition.magnitude;

            if (distanceFromCenter > deadZoneRadius)
            {
                float normalizedDistance = Mathf.Clamp01((distanceFromCenter - deadZoneRadius) / (crosshair.maxRadius - deadZoneRadius));
                float rotationSpeed = maxRotationSpeed * rotationSpeedCurve.Evaluate(normalizedDistance);

                Vector3 rightMovement = _mainCam.right * crosshairPosition.x;
                Vector3 forwardMovement = _mainCam.up * crosshairPosition.y;
                Vector3 deltaWorld = (rightMovement + forwardMovement) * cursorMovementScale;

                _posCenteredUnscaled = ((_targetPosition + deltaWorld) - droneBody.position);
                _targetPosition = distanceToShip / Mathf.Sqrt(
                    Mathf.Pow(_posCenteredUnscaled.x, 2) +  
                    Mathf.Pow(_posCenteredUnscaled.y, 2) + 
                    Mathf.Pow(_posCenteredUnscaled.z, 2)) * _posCenteredUnscaled + droneBody.position;

                if (deltaWorld != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(deltaWorld);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                }
            }
        }


    }
}