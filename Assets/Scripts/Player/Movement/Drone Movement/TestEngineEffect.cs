using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using Cinemachine;
using NaughtyAttributes;
using Player.Movement.Miscellaneous;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

namespace Player.Movement.Drone_Movement
{
    public class TestEngineEffect : MonoBehaviour
    {
        public static TestEngineEffect Instance;
    
        [Foldout("Drone Movement Data")] [SerializeField] [MinValue(0f)]
        private float thrustForce = 1500f;
        [Foldout("Drone Movement Data")] [SerializeField] [MinValue(0f)]
        private float yawForce = 1500f;
        [Foldout("Drone Movement Data")] [SerializeField] [MinValue(0f)]
        private float pitchForce = 1500f;
        [Foldout("Drone Movement Data")] [SerializeField] [MinValue(1f)]
        private Vector3 speedBoostMultiplier = new(3f, 1.5f, 1.5f);
        
        [Foldout("Drone Rotation Data")] [SerializeField] 
        private float rotateXForce = 50f;
        [Foldout("Drone Rotation Data")] [SerializeField] 
        private float rotateYForce = 50f;
        [Foldout("Drone Rotation Data")] [SerializeField] 
        private float rollForce = 50f;
        [Foldout("Drone Rotation Data")] [SerializeField] [MinValue(1f)]
        private Vector3 rotationBoostMultiplier = new(1.25f, 1.25f, 1.25f);
        [Foldout("Drone Rotation Data")] [SerializeField] 
        private float deadZoneRadius = 0.1f;
        
        [Foldout("Camera Data")] [SerializeField] 
        private CinemachineVirtualCamera thirdPersonCamera;
        [Foldout("Camera Data")] [SerializeField] 
        private CinemachineFreeLook orbitCamera;
        
        [Foldout("Visual Effects Data")] [SerializeField] 
        [SerializedDictionary("Engine Group Type", "Associated Engines")]
        private SerializedDictionary<EngineGroupType, EngineGroup> engineGroupDictionary;
        [Foldout("Visual Effects Data")] [SerializeField] 
        [SerializedDictionary("Engine Group Type", "effect duration time")]
        private float duration;
        [Foldout("Visual Effects Data")] [SerializeField] 
        [SerializedDictionary("Engine Group Type", "Engine States")]
        private Dictionary<EngineGroupType, EngineStateInfo> _engineStates;
        
        private class EngineStateInfo
        {
            public EngineVFXState CurrentState;
            public float StartTimer;
        }

        private PlayerInputActions _playerInputActions;
        private CinemachineBrain _cinemachineBrain;
        private Rigidbody _rigidBody;
        private Coroutine engineCoroutine;
        
        
        [Range(-1f, 1f)]
        private float _mouseX, 
            _mouseY, 
            _rollAmount, 
            _thrustAmount, 
            _yawAmount, 
            _pitchAmount;
        
        private bool _isBoosted;

        [MinValue(0f)]
        private float _thrustMultiplier = 1f,
            _yawMultiplier = 1f,
            _pitchMultiplier = 1f,
            _rotateXMultiplier = 1f,
            _rotateYMultiplier = 1f,
            _rollMultiplier = 1f;

        private static Vector2 ScreenCenter => new(Screen.width * 0.5f, Screen.height * 0.5f);

        private ICinemachineCamera _thirdPersonCameraInterface, _orbitCameraInterface;
        
        private enum EngineVFXState
        {
            Start,
            Active,
            End
        }
        
        private void InitializeEngineStates()
        {
            _engineStates = new Dictionary<EngineGroupType, EngineStateInfo>();
            foreach (var key in engineGroupDictionary.Keys)
            {
                _engineStates[key] = new EngineStateInfo
                {
                    CurrentState = EngineVFXState.End,
                    StartTimer = 0f
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

            _cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();

            _thirdPersonCameraInterface = thirdPersonCamera.GetComponent<ICinemachineCamera>();
            _orbitCameraInterface = orbitCamera.GetComponent<ICinemachineCamera>();
            
            thirdPersonCamera.MoveToTopOfPrioritySubqueue();

            InitializeEngineStates();
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
        
        private bool IsInputActiveForEngineType(EngineGroupType engineType)
        {
            bool isActive = engineType switch
            {
                EngineGroupType.Back => _thrustAmount > 0f,
                EngineGroupType.Front => _thrustAmount < 0f,
                EngineGroupType.Top => _pitchAmount < 0f || _rollAmount > 0f,
                EngineGroupType.Bottom => _pitchAmount > 0f || _rollAmount < 0f,
                EngineGroupType.Left => _yawAmount > 0f || _rollAmount > 0f,
                EngineGroupType.Right => _yawAmount < 0f || _rollAmount < 0f,
                _ => false
            };
            
            return isActive;
        }
        
        private void UpdateEngineEffects()
        {
            foreach (var kvp in _engineStates)
            {
                var engineType = kvp.Key;
                var stateInfo  = kvp.Value;
                var engineGroup = engineGroupDictionary[engineType];

                bool inputActive = IsInputActiveForEngineType(engineType); 

                switch (stateInfo.CurrentState)
                {
                    case EngineVFXState.Start:
                        stateInfo.StartTimer += Time.deltaTime;
    
                        if (inputActive)
                        {
                            if (stateInfo.StartTimer >= duration)
                            {
                                StopEffects(engineGroup.engineStartEffects, engineGroup);
                                stateInfo.CurrentState = EngineVFXState.Active;
                                PlayEffects(engineGroup.engineActiveEffects, engineGroup);
                            }
                        }
                        else
                        {
                            StopEffects(engineGroup.engineStartEffects, engineGroup);
                            stateInfo.StartTimer = 0f;
                            stateInfo.CurrentState = EngineVFXState.End;
                            StartCoroutine(EndSequence(engineGroup, stateInfo));
                        }
                        break;


                    case EngineVFXState.Active:
                        if (!inputActive)
                        {
                            StopEffects(engineGroup.engineActiveEffects, engineGroup);

                            stateInfo.CurrentState = EngineVFXState.End;
                            StartCoroutine(EndSequence(engineGroup, stateInfo));
                        }
                        break;

                    case EngineVFXState.End:
                        if (inputActive)
                        {
                            stateInfo.CurrentState = EngineVFXState.Start;
                            stateInfo.StartTimer = 0f;
                            PlayEffects(engineGroup.engineStartEffects, engineGroup);
                        }
                        break;

                    default:
                        if (inputActive)
                        {
                            stateInfo.CurrentState = EngineVFXState.Start;
                            stateInfo.StartTimer = 0f;
                            PlayEffects(engineGroup.engineStartEffects, engineGroup);
                        }
                        break;

                }
            }
        }
        
        private void PlayEffects(IEnumerable<VisualEffect> effects, EngineGroup engineGroup)
        {
            foreach (var effect in effects)
            {
                if (!engineGroup.IsEffectActive(effect))
                {
                    effect.Play();
                    engineGroup.SetEffectActive(effect, true);
                }
            }
        }

        private void StopEffects(IEnumerable<VisualEffect> effects, EngineGroup engineGroup)
        {
            foreach (var effect in effects)
            {
                if (engineGroup.IsEffectActive(effect))
                {
                    effect.Stop();
                    effect.Reinit();
                    engineGroup.SetEffectActive(effect, false);
                }
            }
        }

        
        private IEnumerator EndSequence(EngineGroup engineGroup, EngineStateInfo stateInfo)
        {
            PlayEffects(engineGroup.engineEndEffects, engineGroup);
            
            yield return new WaitForSeconds(duration);
            
            StopEffects(engineGroup.engineEndEffects, engineGroup);
            
            stateInfo.CurrentState = EngineVFXState.End;
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
        }
    }
}
