using Cinemachine;
using Player;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    public static DroneController Instance;
    [Header("Drone Movement Values")]
    [SerializeField] private float thrustForce = 1500f;
    [SerializeField] private float yawForce = 1500f;
    [SerializeField] private float pitchForce = 1500f;
    [SerializeField] private float rotateYForce = 50f;
    [SerializeField] private float rollForce = 50f;
    [SerializeField] private float rotateXForce = 50f;
    [SerializeField] private float deadZoneRadius = 0.1f;
    [Header("Camera Movement Values")]
    [SerializeField] private CinemachineVirtualCamera thirdPersonCamera;
    [SerializeField] private CinemachineFreeLook orbitCamera;

    private CinemachineBrain _cinemachineBrain;
    private int _priorityDiff = 10;
    private PlayerInputActions _playerInputActions;
    private Rigidbody _rigidBody;
    [Range(-1f, 1f)]
    private float _mouseY, _rollAmount, _mouseX, _thrustAmount, _yawAmount, _pitchAmount = 0f;

    private Vector2 ScreenCenter => new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);

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
        _playerInputActions.PlayerCamera.DroneOrbitCamera.started += _ => ToggleCameras();
        _playerInputActions.PlayerCamera.DroneOrbitCamera.canceled += _ => ToggleCameras();

        _cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
    }
    private float CalculateMouseX(Vector3 mousePosition)
    {
        float calculateMouseX = (mousePosition.x - ScreenCenter.x) / ScreenCenter.x;
        return Mathf.Abs(calculateMouseX) > deadZoneRadius ? calculateMouseX : 0f;
    }

    private float CalculateMouseY(Vector3 mousePosition)
    {
        float calculateMouseY = (mousePosition.y - ScreenCenter.y) / ScreenCenter.y;
        return Mathf.Abs(calculateMouseY) > deadZoneRadius ? calculateMouseY * -1 : 0f;
    }
    
    void FixedUpdate()
    {
        Vector3 mousePosition = _playerInputActions.PlayerCamera.MousePosition.ReadValue<Vector2>();
        
        _mouseX = CalculateMouseX(mousePosition);
        _mouseY = CalculateMouseY(mousePosition);
        _playerInputActions.PlayerShip.Roll.performed += _ =>_rollAmount = _playerInputActions.PlayerShip.Roll.ReadValue<float>();
        _playerInputActions.PlayerShip.Roll.canceled += _ =>_rollAmount = _playerInputActions.PlayerShip.Roll.ReadValue<float>() ;
        _playerInputActions.PlayerShip.Thrust.performed += _ => _thrustAmount = _playerInputActions.PlayerShip.Thrust.ReadValue<float>();
        _playerInputActions.PlayerShip.Thrust.canceled += _ => _thrustAmount = _playerInputActions.PlayerShip.Thrust.ReadValue<float>();
        _playerInputActions.PlayerShip.Yaw.performed += _ => _yawAmount = _playerInputActions.PlayerShip.Yaw.ReadValue<float>();
        _playerInputActions.PlayerShip.Yaw.canceled += _ => _yawAmount = _playerInputActions.PlayerShip.Yaw.ReadValue<float>();
        _playerInputActions.PlayerShip.Pitch.performed += _ => _pitchAmount = _playerInputActions.PlayerShip.Pitch.ReadValue<float>();
        _playerInputActions.PlayerShip.Pitch.canceled += _ => _pitchAmount = _playerInputActions.PlayerShip.Pitch.ReadValue<float>();
        ApplyMovementForces();
    }

    private void ApplyMovementForces()
    {
        if (!Mathf.Approximately(0f, _mouseY))
        {
            _rigidBody.AddTorque(transform.right * (rotateYForce * _mouseY * Time.fixedDeltaTime));
        }

        if (!Mathf.Approximately(0f, _rollAmount))
        {
            _rigidBody.AddTorque(transform.forward * (rollForce * _rollAmount * Time.fixedDeltaTime));
        }

        if (!Mathf.Approximately(0f, _mouseX))
        {
            _rigidBody.AddTorque(transform.up * (rotateXForce * _mouseX * Time.fixedDeltaTime));
        }

        if (!Mathf.Approximately(0f, _thrustAmount))
        {
            _rigidBody.AddForce(transform.forward * (thrustForce * _thrustAmount * Time.fixedDeltaTime));
        }
        if (!Mathf.Approximately(0f, _yawAmount))
        {
            _rigidBody.AddForce(transform.right * (yawForce * _yawAmount * Time.fixedDeltaTime));
        }
        if (!Mathf.Approximately(0f, _pitchAmount))
        {
            _rigidBody.AddForce(transform.up * (pitchForce * _pitchAmount * Time.fixedDeltaTime));
        }
    }
    private void ToggleCameras()
    {
        _cinemachineBrain.ActiveVirtualCamera.Priority += _priorityDiff * -1;
        this.enabled = !this.enabled;
    }

    public void SetThrust(float setThrust)
    {
        thrustForce = setThrust;
    }
}
