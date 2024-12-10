using System.Collections;
using Player.Movement.Miscellaneous;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Player.Movement.Global_Map_Movement
{
    public class PlayerClickMovement : MonoBehaviour
    {
        private PlayerInputActions _playerInputActions;
        
        [SerializeField] private float maxSpeed = 10f;
        [SerializeField] private float accelerationRate = 4f;
        [SerializeField] private float decelerationRate = 4f;
        [SerializeField] private float minDampen = 0.1f;
        [SerializeField] private float maxDampen = 0.8f;
        [SerializeField] private float movementStopMargin = 0.05f;
        [SerializeField] private float rotationSpeed = 5f;
        [SerializeField] private float rotationStopMargin = 1f;

        private Camera _mainCamera;
        private Rigidbody _rigidBody;
        private Plane _movementPlane;
        private Vector3 _targetPosition;
        private Vector3 _currentVelocity = Vector3.zero;
        private bool _isMoving, _isRotating;
        private Coroutine _moveCoroutine;
        private float _currentSpeed;
        
        private void Awake()
        {
            _playerInputActions = PlayerActions.InputActions;
            _playerInputActions.Disable();
            _playerInputActions.PlayerShipMap.Enable();

            _playerInputActions.PlayerShipMap.MoveToPoint.performed += MoveToPoint;
            SceneManager.sceneLoaded += UpdateMainCamera;
        }

        private void Start()
        {
            _rigidBody = GetComponent<Rigidbody>();
            _movementPlane = new Plane(Vector3.up, Vector3.zero);
        }
        
        private void UpdateMainCamera(Scene arg0, LoadSceneMode arg1)
        {
            _mainCamera = Camera.main;
        }

        private void MoveToPoint(InputAction.CallbackContext context)
        {
            var ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            
            if (!_movementPlane.Raycast(ray, out var enter))
            {
                return;    
            }
            
            _targetPosition = ray.GetPoint(enter);
            _targetPosition.y = 0;
            
            if (_moveCoroutine == null)
            {
                _moveCoroutine = StartCoroutine(MoveToPointCoroutine());
            }
            else
            {
                _isMoving = true;
                _isRotating = true;
            }
        }

        private IEnumerator MoveToPointCoroutine()
        {
            _isMoving = true;
            _isRotating = true;
            
            while (_isMoving || _isRotating)
            {
                var pos = transform.position;
                var targetDirection = (_targetPosition - pos).normalized;
                var distanceToTarget = Vector3.Distance(pos, _targetPosition);
                
                if (_isMoving)
                {
                    var forwardAlignment = Vector3.Dot(transform.forward, targetDirection);
                    var angleToTarget = Vector3.Angle(transform.forward, targetDirection);
                    var currentDampening = Mathf.Lerp(minDampen, maxDampen, angleToTarget / 180f);
                    
                    _currentVelocity = Vector3.Lerp(_currentVelocity, Vector3.zero, 
                        currentDampening * Time.fixedDeltaTime);
                    
                    var currentSpeed = _currentVelocity.magnitude;
                    var stoppingDistance = currentSpeed * currentSpeed / (2 * decelerationRate);
                    Vector3 desiredVelocity;
                    
                    if (stoppingDistance >= distanceToTarget)
                    {
                        desiredVelocity = targetDirection * Mathf.Lerp(_currentVelocity.magnitude, 
                            0, decelerationRate * Time.fixedDeltaTime);
                    }
                    else
                    {
                        desiredVelocity = targetDirection * (maxSpeed * Mathf.Lerp(0.5f, 1f, forwardAlignment));
                    }
                    
                    var parallelVelocity = Vector3.Project(_currentVelocity, targetDirection);
                    var perpendicularVelocity = _currentVelocity - parallelVelocity;
                    
                    perpendicularVelocity *= Mathf.Lerp(1f, 0.5f, 
                        Mathf.InverseLerp(0, 1, forwardAlignment));
                    
                    var steeringForce = desiredVelocity - (parallelVelocity + perpendicularVelocity);
                    var steeringRate = distanceToTarget > stoppingDistance ? accelerationRate : decelerationRate;
                    
                    steeringForce = Vector3.ClampMagnitude(steeringForce, steeringRate * Time.fixedDeltaTime);
                    _currentVelocity += steeringForce;
                    _currentVelocity = Vector3.ClampMagnitude(_currentVelocity, maxSpeed);
                    
                    var newPosition = pos + _currentVelocity * Time.fixedDeltaTime;
                    
                    if (distanceToTarget < movementStopMargin)
                    {
                        newPosition = _targetPosition;
                        _currentVelocity = Vector3.zero;
                        _isMoving = false;
                    }
                    
                    _rigidBody.MovePosition(newPosition);
                }
                
                if (_isRotating && targetDirection != Vector3.zero)
                {
                    var newRotation = Quaternion.LookRotation(targetDirection);
                    _rigidBody.MoveRotation(Quaternion.Slerp(transform.rotation, 
                        newRotation, Time.fixedDeltaTime * rotationSpeed));
                    
                    if (Quaternion.Angle(transform.rotation, newRotation) <= rotationStopMargin)
                    {
                        _isRotating = false;
                    }
                }
                
                yield return new WaitForFixedUpdate();
            }
                
            _moveCoroutine = null;
        }

        private void OnDestroy()
        {
            _playerInputActions.PlayerShipMap.MoveToPoint.performed -= MoveToPoint;
            SceneManager.sceneLoaded -= UpdateMainCamera;
        }
    }
}
