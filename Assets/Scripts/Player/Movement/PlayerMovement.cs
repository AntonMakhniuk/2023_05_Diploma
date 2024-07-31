using UnityEngine;
using UnityEngine.SceneManagement;
using Wagons.Systems;

namespace Player.Movement
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour 
    {
        public static PlayerMovement Instance;
        
        private PlayerInputActions _playerInputActions;
        private Rigidbody _rb;
        private Camera _mainCamera;
        
        private bool _isPitchingX, _isYawingY, _isRollingZ;

        [Header("Ship Movement Parameters")]
        [SerializeField] private float moveSpeed = 1;
        [SerializeField] private float accelerationDrag = 0.3f;
        [SerializeField] private float brakesDrag = 1.5f;
        [Space]
        [Header("Ship Rotation Parameters")]
        [SerializeField] private float rotationSpeed = 1;
        [SerializeField] private float maxAngularVelocity = 1;
        [SerializeField] private float accelerationAngularDrag = 0.1f;
        [SerializeField] private float brakesAngularDrag = 1f;
        [SerializeField] private float cameraAlignRotationSpeed = 1f;

        private const float MaxSpeedModifier = 5, MinSpeedModifier = 0.1f;
        private float _speedModifier = 1;
        public float SpeedModifier
        {
            get => _speedModifier;
            set
            {
                _speedModifier = value switch
                {
                    > MaxSpeedModifier => MaxSpeedModifier,
                    < MinSpeedModifier => MinSpeedModifier,
                    _ => value
                };
            }
        }

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
            _playerInputActions.PlayerShip.Enable();
        
            _rb = GetComponent<Rigidbody>();
            
            _rb.maxAngularVelocity = maxAngularVelocity;
            
            _rb.drag = accelerationDrag;
            _rb.angularDrag = accelerationAngularDrag;
        
            // TODO: return it to how it was (without the null check)
            if (WagonManager.Instance != null)
            {
                WagonManager.Instance.SetDragValuesForAttachedWagons(accelerationDrag, accelerationAngularDrag);
            }
        
            _mainCamera = Camera.main;
        
            // Brakes
            _playerInputActions.PlayerShip.Brakes.performed += _ =>
            {
                _rb.drag = brakesDrag;
                _rb.angularDrag = brakesAngularDrag;
            
                // TODO: return this back to how it was (without the null check)
                if (WagonManager.Instance != null)
                {
                    WagonManager.Instance.SetDragValuesForAttachedWagons(brakesDrag, brakesAngularDrag);
                }
            };

            _playerInputActions.PlayerShip.Brakes.canceled += _ =>
            {
                _rb.drag = accelerationDrag;
                _rb.angularDrag = accelerationAngularDrag;
            
                // TODO: return this back to how it was (without the null check)
                if (WagonManager.Instance != null)
                {
                    WagonManager.Instance.SetDragValuesForAttachedWagons(accelerationDrag, accelerationAngularDrag);
                }
            };

            // Pitch (X axis)
            _playerInputActions.PlayerShip.Pitch.performed += _ => _isPitchingX = true;
            _playerInputActions.PlayerShip.Pitch.canceled += _ => _isPitchingX = false;
        
            // Yaw (Y axis)
            _playerInputActions.PlayerShip.Yaw.performed += _ => _isYawingY = true;
            _playerInputActions.PlayerShip.Yaw.canceled += _ => _isYawingY = false;
        
            // Roll (Z axis)
            _playerInputActions.PlayerShip.Roll.performed += _ =>  _isRollingZ = true;
            _playerInputActions.PlayerShip.Roll.canceled += _ => _isRollingZ = false;

            SceneManager.sceneLoaded += HandleSceneLoaded;
        }

        private void HandleSceneLoaded(Scene _, LoadSceneMode __)
        {
            _mainCamera = Camera.main;
        }

        private void FixedUpdate() 
        {
            if (_playerInputActions.PlayerShip.Thrust.IsPressed())
            {
                Move(new Vector2(_playerInputActions.PlayerShip.Thrust.ReadValue<float>(), 0));
            }
        
            if (_playerInputActions.PlayerShip.Strafe.IsPressed())
            {
                Move(new Vector2(0, _playerInputActions.PlayerShip.Strafe.ReadValue<float>()));
            }
        
            if (_isPitchingX)
            {
                Rotate(new Vector3(_playerInputActions.PlayerShip.Pitch.ReadValue<float>(), 0, 0));
            }
        
            if (_isYawingY)
            {
                Rotate(new Vector3(0, _playerInputActions.PlayerShip.Yaw.ReadValue<float>(), 0));
            }
        
            if (_isRollingZ)
            {
                Rotate(new Vector3(0, 0, _playerInputActions.PlayerShip.Roll.ReadValue<float>() / 2f));
            }
        
            // Check if camera alignment is active, rotate if yes
            if (_playerInputActions.PlayerShip.AlignWithCamera.IsPressed())
            {
                AlignWithCamera();
            }
        }
    
        // Move along Z and Y axis (forward or backward / up or down)
        private void Move(Vector2 movementVector) 
        {
            _rb.AddRelativeForce(moveSpeed * _speedModifier * Time.deltaTime 
                * new Vector3(movementVector.y, 0, movementVector.x), ForceMode.Force);
        }

        // Rotate along the input vector
        private void Rotate(Vector3 inputVector) 
        {
            _rb.AddRelativeTorque(rotationSpeed * _speedModifier * Time.deltaTime * inputVector, ForceMode.Force);
        }

        // Rotate to align with the direction where camera is pointed at
        private void AlignWithCamera()
        {
            _rb.MoveRotation
            (
                Quaternion.Slerp
                (
                    transform.rotation, 
                    _mainCamera.transform.rotation, 
                    cameraAlignRotationSpeed * Time.deltaTime
                )
            );
        }
    }
}
