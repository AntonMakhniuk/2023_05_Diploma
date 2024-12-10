using System.Collections;
using Cinemachine;
using NaughtyAttributes;
using Player.Movement.Miscellaneous;
using Player.Ship;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Player.Movement.Global_Map_Movement
{
    public class MapCameraMovement : MonoBehaviour
    {
        [Foldout("Camera Data")] [SerializeField] 
        private Rigidbody pivotRigidBody;
        
        [Foldout("Camera Movement Data")] [SerializeField] 
        private float movementSpeed = 100;
        [Foldout("Camera Movement Data")] [SerializeField] 
        private float maxReturnSpeed = 20f;
        [Foldout("Camera Movement Data")] [SerializeField] 
        private float returnAcceleration = 5f;
        [Foldout("Camera Movement Data")] [SerializeField] 
        private float returnDecelerationDistance = 5f;
        
        [Foldout("Camera Rotation Data")] [SerializeField] 
        private float rotationSpeed = 100;

        [Foldout("Camera Zoom Data")] [SerializeField]
        private float zoomSpeed = 0.8f;
        [Foldout("Camera Zoom Data")] [SerializeField]
        private float minZoom = 5;
        [Foldout("Camera Zoom Data")] [SerializeField]
        private float maxZoom = 20; 
        [Foldout("Camera Zoom Data")] [SerializeField]
        private float defaultZoom = 15;
        [Foldout("Camera Zoom Data")] [SerializeField] 
        private float zoomSmoothTime = 0.2f;
        
        // Should not be changed directly unless you know what you're doing,
        // instead use the property so that HandleFollowModeChanged is called consistently
        [Foldout("Camera Data")] [SerializeField] [OnValueChanged(nameof(HandleFollowModeChanged))]
        private bool cameraFollowsShipMovement;
        
        // For use anywhere outside the inspector
        public bool CameraFollowsShipMovement
        {
            get => cameraFollowsShipMovement;
            set
            {
                cameraFollowsShipMovement = value;
                
                HandleFollowModeChanged();
            }
        }

        private CinemachineVirtualCamera _camera;
        private PlayerInputActions _inputActions;
        private Vector2 _moveDirection;
        private float _rotationDirection;
        private Coroutine _movementCoroutine, _followShipCoroutine, 
            _rotationCoroutine, _zoomCoroutine, _returnCoroutine;
        private Transform _pivotTransform;
        private float _targetZoom, _zoomVelocity;

        private void Start()
        {
            _camera = GetComponent<CinemachineVirtualCamera>();
            _inputActions = PlayerActions.InputActions;
            
            SceneManager.sceneLoaded += HandleSceneLoaded;
            _inputActions.PlayerShipMap.MoveCamera.performed += HandleStartMoveCamera;
            _inputActions.PlayerShipMap.MoveCamera.canceled += HandleStopMoveCamera;
            _inputActions.PlayerShipMap.ReturnCameraToShip.performed += HandleReturnCameraToShip;
            _inputActions.PlayerShipMap.ZoomCamera.performed += HandleZoomCamera;
            _inputActions.PlayerShipMap.RotateCamera.performed += HandleStartRotateCamera;
            _inputActions.PlayerShipMap.RotateCamera.canceled += HandleStopRotateCamera;
            _inputActions.PlayerShipMap.FollowShip.performed += HandleFollowShip;

            _pivotTransform = pivotRigidBody.transform;
        }

        private void HandleSceneLoaded(Scene arg0, LoadSceneMode loadSceneMode)
        {
            _camera = GetComponent<CinemachineVirtualCamera>();

            SetUpCamera();
        }

        private void HandleStartMoveCamera(InputAction.CallbackContext ctx)
        {
            _moveDirection = ctx.ReadValue<Vector2>();
            
            if (CameraFollowsShipMovement)
            {
                CameraFollowsShipMovement = false;
            }

            if (_returnCoroutine != null)
            {
                StopCoroutine(_returnCoroutine);
                _returnCoroutine = null;
            }
            
            _movementCoroutine ??= StartCoroutine(MoveCameraCoroutine());
        }
        
        private IEnumerator MoveCameraCoroutine()
        {
            while (true)
            {
                var dirFromPivot = 
                    _pivotTransform.right * _moveDirection.x + _pivotTransform.forward * _moveDirection.y;
                var newPos = _pivotTransform.position + dirFromPivot * (movementSpeed * Time.deltaTime);
                
                pivotRigidBody.MovePosition(newPos);
        
                yield return null;
            }
        }
        
        private void HandleStopMoveCamera(InputAction.CallbackContext obj)
        {
            if (_movementCoroutine == null)
            {
                return;
            }
            
            StopCoroutine(_movementCoroutine);
            _movementCoroutine = null;
        }
        
        
        private void HandleReturnCameraToShip(InputAction.CallbackContext ctx)
        {
            if (_movementCoroutine != null)
            {
                return;
            }
            
            _returnCoroutine ??= StartCoroutine(ReturnCameraToShipCoroutine());
        }

        private IEnumerator ReturnCameraToShipCoroutine()
        {
            var targetPosition = PlayerShipMap.Instance.transform.position;
            var currentSpeed = 0f;

            while ((pivotRigidBody.position - targetPosition).sqrMagnitude > 0.01f)
            {
                targetPosition = PlayerShipMap.Instance.transform.position;
                
                var distanceToTarget = Vector3.Distance(pivotRigidBody.position, targetPosition);
                
                currentSpeed = Mathf.Min(currentSpeed + returnAcceleration * Time.deltaTime, maxReturnSpeed);
                
                var decelerationFactor = Mathf.Clamp01(distanceToTarget / returnDecelerationDistance);
                var adjustedSpeed = currentSpeed * decelerationFactor;
                
                var newPosition = Vector3.MoveTowards(pivotRigidBody.position,
                    targetPosition, adjustedSpeed * Time.deltaTime
                );

                pivotRigidBody.MovePosition(newPosition);

                yield return null;
            }

            _followShipCoroutine ??= StartCoroutine(FollowShipCoroutine());
            _returnCoroutine = null;
        }

        private void SetUpCamera()
        {
            pivotRigidBody.MovePosition(PlayerShipMap.Instance.transform.position);
            pivotRigidBody.MoveRotation(Quaternion.Euler(0, 0, 0));
            _camera.m_Lens.OrthographicSize = defaultZoom;
        }

        private void HandleZoomCamera(InputAction.CallbackContext ctx)
        {
            _targetZoom += ctx.ReadValue<float>() * zoomSpeed * Time.deltaTime;
            _targetZoom = Mathf.Clamp(_targetZoom, minZoom, maxZoom);
            
            _zoomCoroutine ??= StartCoroutine(ZoomCameraCoroutine());
        }

        private IEnumerator ZoomCameraCoroutine()
        {
            while (Mathf.Abs(_camera.m_Lens.OrthographicSize - _targetZoom) > 0.01f)
            {
                _camera.m_Lens.OrthographicSize = Mathf.SmoothDamp(_camera.m_Lens.OrthographicSize,
                    _targetZoom,
                    ref _zoomVelocity,
                    zoomSmoothTime
                );

                yield return null;
            }
            
            _zoomCoroutine = null;
        }
        
        private void HandleStartRotateCamera(InputAction.CallbackContext ctx)
        {
            _rotationDirection = ctx.ReadValue<float>();

            _rotationCoroutine ??= StartCoroutine(RotateCameraCoroutine());
        }
        
        private IEnumerator RotateCameraCoroutine()
        {
            while (true)
            {
                var rotationDelta = 
                    Quaternion.Euler(0, _rotationDirection * rotationSpeed * Time.deltaTime, 0);
                
                pivotRigidBody.MoveRotation(pivotRigidBody.rotation * rotationDelta);

                yield return null;
            }
        }
        
        private void HandleStopRotateCamera(InputAction.CallbackContext ctx)
        {
            if (_rotationCoroutine == null)
            {
                return;
            }
            
            StopCoroutine(_rotationCoroutine);
            _rotationCoroutine = null;
        }

        // Has to be public for OnChangedCall
        public void HandleFollowModeChanged()
        {
            if (cameraFollowsShipMovement)
            {
                _returnCoroutine ??= StartCoroutine(ReturnCameraToShipCoroutine());
            }
            else 
            {
                if (_returnCoroutine != null)
                {
                    StopCoroutine(_returnCoroutine);
                    _returnCoroutine = null;
                }
                
                if (_followShipCoroutine != null)
                {
                    StopCoroutine(_followShipCoroutine);
                    _followShipCoroutine = null;
                }
            }
        }
        
        private void HandleFollowShip(InputAction.CallbackContext obj)
        {
            CameraFollowsShipMovement = !CameraFollowsShipMovement;
        }

        private IEnumerator FollowShipCoroutine()
        {
            if (!cameraFollowsShipMovement)
            {
                cameraFollowsShipMovement = true;
            }
            
            while (true)
            {
                pivotRigidBody.MovePosition(PlayerShipMap.Instance.transform.position);

                yield return null;
            }
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= HandleSceneLoaded;
            _inputActions.PlayerShipMap.MoveCamera.performed -= HandleStartMoveCamera;
            _inputActions.PlayerShipMap.MoveCamera.canceled -= HandleStopMoveCamera;
            _inputActions.PlayerShipMap.ReturnCameraToShip.performed -= HandleReturnCameraToShip;
            _inputActions.PlayerShipMap.ZoomCamera.performed -= HandleZoomCamera;
            _inputActions.PlayerShipMap.RotateCamera.performed -= HandleStartRotateCamera;
            _inputActions.PlayerShipMap.RotateCamera.canceled -= HandleStopRotateCamera;
            _inputActions.PlayerShipMap.FollowShip.performed -= HandleFollowShip;
            
            StopAllCoroutines();
        }
    }
}
