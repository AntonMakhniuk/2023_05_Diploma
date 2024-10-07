using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Movement
{
    public class PlayerClickMovement : MonoBehaviour
    {
        public static PlayerClickMovement Instance;
        
        private PlayerInputActions _playerInputActions;
        
        [SerializeField] private Camera mainCamera;
        
        [SerializeField] private float maxSpeed = 10;
        [SerializeField] private float accelerationRate = 4f;
        [SerializeField] private float stopDistance = 0.05f;
        [SerializeField] private float decelerationDistance = 3.0f;
        [SerializeField] private float rotationSpeed = 5f;
        [SerializeField] private float rotationStopMargin = 1f;

        private Plane _movementPlane;
        private Vector3 _targetPosition;
        private Vector3 _velocity = Vector3.zero;
        private bool _isMoving, _isRotating;
        private Coroutine _moveCoroutine; 
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
            
            _playerInputActions = PlayerActions.InputActions;
            _playerInputActions.Disable();
            _playerInputActions.PlayerShipMap.Enable();

            _playerInputActions.PlayerShipMap.MoveToPoint.performed += MoveToPoint;
        }

        private void Start()
        {
            _movementPlane = new Plane(Vector3.up, Vector3.zero);
        }

        private void MoveToPoint(InputAction.CallbackContext context)
        {
            var ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            
            if (!_movementPlane.Raycast(ray, out var enter))
            {
                return;    
            }
            
            _targetPosition = ray.GetPoint(enter);
            _targetPosition.y = 0;
            
            if (_moveCoroutine != null)
            {
                StopCoroutine(_moveCoroutine);
            }
            
            _moveCoroutine = StartCoroutine(MoveToPointCoroutine());
        }

        private IEnumerator MoveToPointCoroutine()
        {
            _isMoving = true;
            _isRotating = true;
            
            while (_isMoving || _isRotating)
            {
                var newPos = transform.position;
                var direction = (_targetPosition - newPos).normalized;
                
                if (_isMoving)
                {
                    var distance = Vector3.Distance(newPos, _targetPosition);
                    var decelerationFactor = Mathf.Clamp01(distance / decelerationDistance);
                    var currentSpeed = maxSpeed * decelerationFactor;
                
                    _velocity += direction * (accelerationRate * Time.fixedDeltaTime);
                    _velocity = Vector3.ClampMagnitude(_velocity, currentSpeed);
                
                    newPos += _velocity * Time.deltaTime;

                    transform.position = newPos;
                
                    if (distance < stopDistance)
                    {
                        _velocity = Vector3.zero;
                        _isMoving = false;
                    }
                }

                var newRotation = Quaternion.LookRotation(direction);
                
                if (_isRotating)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, 
                        newRotation, Time.fixedDeltaTime * rotationSpeed);

                    Debug.Log(transform.rotation.eulerAngles.y + " current");
                    Debug.Log(newRotation.eulerAngles.y + " desired");
                    
                    if (Math.Abs(transform.rotation.eulerAngles.y - newRotation.eulerAngles.y) <= rotationStopMargin)
                    {
                        _isRotating = false;
                    }
                }
                
                yield return new WaitForFixedUpdate();
            }
        }
    }
}
