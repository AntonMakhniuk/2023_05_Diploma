using System.Linq;
using System.Collections;
using AYellowpaper.SerializedCollections;
using Cinemachine;
using NaughtyAttributes;
using Player.Movement.Miscellaneous;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Movement.Drone_Movement
{
    public class DroneController : MonoBehaviour
    {
        public static DroneController Instance;

        [Foldout("Drone Movement Data")]
        [SerializeField]
        [MinValue(0f)]
        private float thrustForce = 1500f;
        [Foldout("Drone Movement Data")]
        [SerializeField]
        [MinValue(0f)]
        private float yawForce = 1500f;
        [Foldout("Drone Movement Data")]
        [SerializeField]
        [MinValue(0f)]
        private float pitchForce = 1500f;
        [Foldout("Drone Movement Data")]
        [SerializeField]
        [MinValue(1f)]
        private Vector3 speedBoostMultiplier = new(3f, 1.5f, 1.5f);

 
        [Foldout("Drone Movement Data")]
        [SerializeField]
        [MinValue(0f)]
        private float dodgeForce = 20f;
        [SerializeField]
        [MinValue(0f)]
        private float dodgeCooldown = 1f; 

        [Foldout("Drone Rotation Data")]
        [SerializeField]
        private float rotateXForce = 50f;
        [Foldout("Drone Rotation Data")]
        [SerializeField]
        private float rotateYForce = 50f;
        [Foldout("Drone Rotation Data")]
        [SerializeField]
        private float rollForce = 50f;
        [Foldout("Drone Rotation Data")]
        [SerializeField]
        [MinValue(1f)]
        private Vector3 rotationBoostMultiplier = new(1.25f, 1.25f, 1.25f);
        [Foldout("Drone Rotation Data")]
        [SerializeField]
        private float deadZoneRadius = 0.1f;

        [Foldout("Camera Data")]
        [SerializeField]
        private CinemachineVirtualCamera thirdPersonCamera;
        [Foldout("Camera Data")]
        [SerializeField]
        private CinemachineFreeLook orbitCamera;

        [Foldout("Visual Effects Data")]
        [SerializeField]
        [SerializedDictionary("Engine Group Type", "Associated Engines")]
        private SerializedDictionary<EngineGroupType, EngineGroup> engineGroupDictionary;

        private PlayerInputActions _playerInputActions;
        private CinemachineBrain _cinemachineBrain;
        private Rigidbody _rigidBody;

        [Range(-1f, 1f)]
        private float _mouseX,
            _mouseY,
            _rollAmount,
            _thrustAmount,
            _yawAmount,
            _pitchAmount;

        private bool _isBoosted;
        private bool _isDodging = false; 
        private float _dodgeCooldownTimer = 0f; 

        [MinValue(0f)]
        private float _thrustMultiplier = 1f,
            _yawMultiplier = 1f,
            _pitchMultiplier = 1f,
            _rotateXMultiplier = 1f,
            _rotateYMultiplier = 1f,
            _rollMultiplier = 1f;

        private static Vector2 ScreenCenter => new(Screen.width * 0.5f, Screen.height * 0.5f);

        private ICinemachineCamera _thirdPersonCameraInterface, _orbitCameraInterface;

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

            _rigidBody = GetComponent<Rigidbody>();
            _playerInputActions = PlayerActions.InputActions;

            _playerInputActions.PlayerCamera.Enable();

            _playerInputActions.PlayerCamera.DroneOrbitCamera.started += ToggleCameras;
            _playerInputActions.PlayerCamera.DroneOrbitCamera.canceled += ToggleCameras;

            _playerInputActions.PlayerShip.Thrust.performed += UpdateDroneDirection;
            _playerInputActions.PlayerShip.Thrust.canceled += RemoveThrust;
            _playerInputActions.PlayerShip.Pitch.performed += UpdateDroneDirection;
            _playerInputActions.PlayerShip.Pitch.canceled += RemovePitch;
            _playerInputActions.PlayerShip.Yaw.performed += UpdateDroneDirection;
            _playerInputActions.PlayerShip.Yaw.canceled += RemoveYaw;
            _playerInputActions.PlayerShip.Roll.performed += UpdateDroneDirection;
            _playerInputActions.PlayerShip.Roll.canceled += RemoveRoll;
            _playerInputActions.PlayerShip.Boost.started += ApplyBoost;
            _playerInputActions.PlayerShip.Boost.canceled += RemoveBoost;
            _playerInputActions.PlayerShip.Dodge.performed += PerformDodge;

            _cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();

            _thirdPersonCameraInterface = thirdPersonCamera.GetComponent<ICinemachineCamera>();
            _orbitCameraInterface = orbitCamera.GetComponent<ICinemachineCamera>();

            thirdPersonCamera.MoveToTopOfPrioritySubqueue();
        }

        private void UpdateDroneDirection(InputAction.CallbackContext _)
        {
            _thrustAmount = _playerInputActions.PlayerShip.Thrust.ReadValue<float>();
            _pitchAmount = _playerInputActions.PlayerShip.Pitch.ReadValue<float>();
            _yawAmount = _playerInputActions.PlayerShip.Yaw.ReadValue<float>();
            _rollAmount = _playerInputActions.PlayerShip.Roll.ReadValue<float>();
        }

        private void RemoveThrust(InputAction.CallbackContext _)
        {
            _thrustAmount = 0f;
        }

        private void RemovePitch(InputAction.CallbackContext _)
        {
            _pitchAmount = 0f;
        }

        private void RemoveYaw(InputAction.CallbackContext _)
        {
            _yawAmount = 0f;
        }

        private void RemoveRoll(InputAction.CallbackContext _)
        {
            _rollAmount = 0f;
        }

        private void FixedUpdate()
        {
            var mousePosition = _playerInputActions.PlayerCamera.MousePosition.ReadValue<Vector2>();
            (_mouseX, _mouseY) = CalculateMousePos(mousePosition);

            if (_dodgeCooldownTimer > 0f)
            {
                _dodgeCooldownTimer -= Time.fixedDeltaTime;
            }

            ApplyMovementForces();
            UpdateEngineEffects();
        }

        private (float, float) CalculateMousePos(Vector2 mousePosition)
        {
            var calculateMouseX = (mousePosition.x - ScreenCenter.x) / ScreenCenter.x;
            var calculateMouseY = (mousePosition.y - ScreenCenter.y) / ScreenCenter.y;

            return (Mathf.Abs(calculateMouseX) > deadZoneRadius ? calculateMouseX : 0f,
                Mathf.Abs(calculateMouseY) > deadZoneRadius ? calculateMouseY * -1 : 0f);
        }

        private void ApplyBoost(InputAction.CallbackContext _)
        {
            (_thrustMultiplier, _yawMultiplier, _pitchMultiplier) =
                (speedBoostMultiplier.x, speedBoostMultiplier.y, speedBoostMultiplier.z);
            (_rotateXMultiplier, _rotateYMultiplier, _rollMultiplier) =
                (rotationBoostMultiplier.x, rotationBoostMultiplier.y, rotationBoostMultiplier.z);
        }

        private void RemoveBoost(InputAction.CallbackContext _)
        {
            (_thrustMultiplier, _yawMultiplier, _pitchMultiplier) = (1f, 1f, 1f);
            (_rotateXMultiplier, _rotateYMultiplier, _rollMultiplier) = (1f, 1f, 1f);
        }

        private void PerformDodge(InputAction.CallbackContext context)
        {
            if (_isDodging || _dodgeCooldownTimer > 0f) return;

            float dodgeDirection = context.ReadValue<float>();
            if (Mathf.Approximately(dodgeDirection, 0f)) return;

            _isDodging = true;
            Vector3 dodgeVector = transform.right * dodgeDirection; 
            _rigidBody.AddForce(dodgeVector * dodgeForce, ForceMode.Impulse);

            StartCoroutine(DodgeCooldown());
        }

        private IEnumerator DodgeCooldown()
        {
            _dodgeCooldownTimer = dodgeCooldown;

            while (_dodgeCooldownTimer > 0f)
            {
                _dodgeCooldownTimer -= Time.deltaTime;
                yield return null;
            }

            _isDodging = false;
        }

        private void ApplyMovementForces()
        {
            if (!Mathf.Approximately(0f, _thrustAmount))
            {
                _rigidBody.AddForce(transform.forward * (thrustForce * _thrustMultiplier *
                                                         _thrustAmount * Time.fixedDeltaTime));
            }
            if (!Mathf.Approximately(0f, _yawAmount))
            {
                _rigidBody.AddForce(transform.right * (yawForce * _yawMultiplier *
                                                       _yawAmount * Time.fixedDeltaTime));
            }
            if (!Mathf.Approximately(0f, _pitchAmount))
            {
                _rigidBody.AddForce(transform.up * (pitchForce * _pitchMultiplier *
                                                    _pitchAmount * Time.fixedDeltaTime));
            }

            if (!Mathf.Approximately(0f, _mouseX))
            {
                _rigidBody.AddTorque(transform.up * (rotateXForce * _rotateXMultiplier *
                                                     _mouseX * Time.fixedDeltaTime));
            }

            if (!Mathf.Approximately(0f, _mouseY))
            {
                _rigidBody.AddTorque(transform.right * (rotateYForce * _rotateYMultiplier *
                                                        _mouseY * Time.fixedDeltaTime));
            }

            if (!Mathf.Approximately(0f, _rollAmount))
            {
                _rigidBody.AddTorque(transform.forward * (rollForce * _rollMultiplier *
                                                          _rollAmount * Time.fixedDeltaTime));
            }
        }

        private void UpdateEngineEffects()
        {
            foreach (var engine in engineGroupDictionary.Values.SelectMany(e => e.engines))
            {
                engine.Stop();
            }

            if (_thrustAmount > 0f)
            {
                ActivateEngines(engineGroupDictionary[EngineGroupType.Back]);
            }
            else if (_thrustAmount < 0f)
            {
                ActivateEngines(engineGroupDictionary[EngineGroupType.Front]);
            }

            if (_pitchAmount > 0f)
            {
                ActivateEngines(engineGroupDictionary[EngineGroupType.Bottom]);
            }
            else if (_pitchAmount < 0f)
            {
                ActivateEngines(engineGroupDictionary[EngineGroupType.Top]);
            }

            if (_yawAmount > 0f)
            {
                ActivateEngines(engineGroupDictionary[EngineGroupType.Left]);
            }
            else if (_yawAmount < 0f)
            {
                ActivateEngines(engineGroupDictionary[EngineGroupType.Right]);
            }

            if (_rollAmount > 0f)
            {
                ActivateEngines(new[] { engineGroupDictionary[EngineGroupType.Top],
                    engineGroupDictionary[EngineGroupType.Right] });
            }
            else if (_rollAmount < 0f)
            {
                ActivateEngines(new[] { engineGroupDictionary[EngineGroupType.Top],
                    engineGroupDictionary[EngineGroupType.Left] });
            }
        }

        private void ActivateEngines(EngineGroup[] engineGroups)
        {
            foreach (var engineGroup in engineGroups)
            {
                ActivateEngines(engineGroup);
            }
        }

        private void ActivateEngines(EngineGroup engineGroup)
        {
            foreach (var engine in engineGroup.engines)
            {
                engine.Play();
                //engine.SetFloat("duration", +0.5f);
            }
        }

        private void ToggleCameras(InputAction.CallbackContext _)
        {
            if (_cinemachineBrain.ActiveVirtualCamera == _orbitCameraInterface)
            {
                thirdPersonCamera.MoveToTopOfPrioritySubqueue();
            }
            else if (_cinemachineBrain.ActiveVirtualCamera == _thirdPersonCameraInterface)
            {
                orbitCamera.MoveToTopOfPrioritySubqueue();
            }
        }

        public void SetThrust(float setThrust)
        {
            thrustForce = setThrust;
        }

        private void OnDestroy()
        {
            _playerInputActions.PlayerCamera.DroneOrbitCamera.started -= ToggleCameras;
            _playerInputActions.PlayerCamera.DroneOrbitCamera.canceled -= ToggleCameras;

            _playerInputActions.PlayerShip.Thrust.performed -= UpdateDroneDirection;
            _playerInputActions.PlayerShip.Thrust.canceled -= RemoveThrust;
            _playerInputActions.PlayerShip.Pitch.performed -= UpdateDroneDirection;
            _playerInputActions.PlayerShip.Pitch.canceled -= RemovePitch;
            _playerInputActions.PlayerShip.Yaw.performed -= UpdateDroneDirection;
            _playerInputActions.PlayerShip.Yaw.canceled -= RemoveYaw;
            _playerInputActions.PlayerShip.Roll.performed -= UpdateDroneDirection;
            _playerInputActions.PlayerShip.Roll.canceled -= RemoveRoll;
            _playerInputActions.PlayerShip.Boost.performed -= ApplyBoost;
            _playerInputActions.PlayerShip.Boost.canceled -= RemoveBoost;
            _playerInputActions.PlayerShip.Dodge.performed -= PerformDodge;
        }
    }
}
