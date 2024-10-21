using System.Collections;
using Cinemachine;
using Player;
using Player.Ship;
using Third_Party.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Environment.Global_Map.Systems
{
    public class MapCameraMovement : MonoBehaviour
    {
        [SerializeField] private Vector3 cameraPositionOffset = new Vector3(0, 20, -30);
        [SerializeField] private Vector3 cameraRotationOffset = new Vector3(30, 0, 0);
        [SerializeField] private float cameraMovementSpeed;
        [SerializeField] private float cameraMaxReturnSpeed;
        [SerializeField] private float cameraRotationSpeed;
        [SerializeField] private float cameraZoomSpeed = 0.01f;
        [SerializeField] private float minZoom = 5, maxZoom = 20, defaultZoom = 15;
        [SerializeField] [OnChangedCall(nameof(HandleFollowModeChanged))]
        // Should not be changed directly, instead use the property so that
        // HandleFollowModeChanged is called consistently
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
        private Coroutine _movementCoroutine, _followShipCoroutine, _rotationCoroutine; 

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
        }

        private void HandleSceneLoaded(Scene arg0, LoadSceneMode loadSceneMode)
        {
            _camera = GetComponent<CinemachineVirtualCamera>();

            SetUpCamera();
        }

        private void HandleStartMoveCamera(InputAction.CallbackContext ctx)
        {
            _moveDirection = ctx.ReadValue<Vector2>();

            _movementCoroutine ??= StartCoroutine(MoveCameraCoroutine());
        }
        
        private IEnumerator MoveCameraCoroutine()
        {
            while (true)
            {
                var zMovementScale = _camera.m_Lens.OrthographicSize / defaultZoom;
                var dir = new Vector3(_moveDirection.x, 0, _moveDirection.y * zMovementScale) 
                          * (cameraMovementSpeed * Time.deltaTime);
                
                _camera.transform.position += dir;
                
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
            var shipPos = PlayerShipMap.Instance.transform.position;
            
            _camera.transform.position = new Vector3(shipPos.x + cameraPositionOffset.x, 
                shipPos.y + cameraPositionOffset.y, shipPos.z + cameraPositionOffset.z);
            _camera.m_Lens.OrthographicSize = defaultZoom;
        }

        private void SetUpCamera()
        {
            var shipPos = PlayerShipMap.Instance.transform.position;
            
            _camera.transform.position = new Vector3(shipPos.x + cameraPositionOffset.x, 
                shipPos.y + cameraPositionOffset.y, shipPos.z + cameraPositionOffset.z);
            _camera.m_Lens.OrthographicSize = defaultZoom;

            _camera.transform.rotation = Quaternion.Euler(cameraRotationOffset);
        }

        private void HandleZoomCamera(InputAction.CallbackContext ctx)
        {
            var zoomDir = ctx.ReadValue<float>();
            
            _camera.m_Lens.OrthographicSize += zoomDir * cameraZoomSpeed * Time.deltaTime;
            _camera.m_Lens.OrthographicSize = Mathf.Clamp(_camera.m_Lens.OrthographicSize, minZoom, maxZoom);
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
                var cameraPosition = _camera.transform.position;
                var orbitCenter = cameraPosition - cameraPositionOffset;
                var directionToCamera = cameraPosition - orbitCenter;
                var rotation = Quaternion.Euler(0, 
                    _rotationDirection * cameraRotationSpeed * Time.deltaTime, 0);
                
                directionToCamera = rotation * directionToCamera;
                cameraPosition = orbitCenter + directionToCamera;
                
                _camera.transform.position = cameraPosition;
                _camera.transform.rotation = Quaternion.Euler(cameraRotationOffset);

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
                _followShipCoroutine ??= StartCoroutine(FollowShipCoroutine());
            }
            else if (_followShipCoroutine != null)
            {
                StopCoroutine(_followShipCoroutine);
                _followShipCoroutine = null;
            }
        }

        private IEnumerator FollowShipCoroutine()
        {
            while (true)
            {
                var shipPos = PlayerShipMap.Instance.transform.position;
                
                _camera.transform.position = new Vector3(shipPos.x + cameraPositionOffset.x, 
                    shipPos.y + cameraPositionOffset.y, shipPos.z + cameraPositionOffset.z);

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
            
            StopAllCoroutines();
        }
    }
}
