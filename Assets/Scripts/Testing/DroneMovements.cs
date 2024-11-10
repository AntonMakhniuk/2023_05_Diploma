using Player;
using UnityEngine;

namespace Testing
{
    public class DroneMovements : MonoBehaviour
    {
        public static DroneMovements Instance;

        [SerializeField] private float thrustSpeed = 50f;
        [SerializeField] private float baseRotationSpeed = 0.5f;
        [SerializeField] private AnimationCurve rotationSpeedCurve;

        [SerializeField] private Transform lookTarget;

        private PlayerInputActions _playerInputActions;
        private bool _isPitchingX, _isYawingY, _isRollingZ;

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

            // Input bindings
            _playerInputActions.PlayerShip.Pitch.performed += _ => _isPitchingX = true;
            _playerInputActions.PlayerShip.Pitch.canceled += _ => _isPitchingX = false;
            _playerInputActions.PlayerShip.Yaw.performed += _ => _isYawingY = true;
            _playerInputActions.PlayerShip.Yaw.canceled += _ => _isYawingY = false;
            _playerInputActions.PlayerShip.Roll.performed += _ =>  _isRollingZ = true;
            _playerInputActions.PlayerShip.Roll.canceled += _ => _isRollingZ = false;
        }

        private void FixedUpdate()
        {
            RotateTowardsCursor();
            
            if (_playerInputActions.PlayerShip.Thrust.IsPressed())
            {
                float thrustInput = _playerInputActions.PlayerShip.Thrust.ReadValue<float>();
                MoveThrust(thrustInput);
            }

            if (_isYawingY) RotateYaw();
            if (_isRollingZ) RotateRoll();
            if (_isPitchingX) RotatePitch();
        }

        private void MoveThrust(float thrustInput)
        {
            transform.Translate(thrustInput * thrustSpeed * Time.deltaTime * Vector3.forward);
        }

        private void RotateTowardsCursor()
        {
            Vector3 directionToTarget = (lookTarget.position - transform.position).normalized;
            float rotationSpeed = baseRotationSpeed * rotationSpeedCurve.Evaluate(Vector3.Distance(lookTarget.position, transform.position) / 20f);

            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            lookTarget.rotation = Quaternion.Slerp(lookTarget.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        private void RotateYaw()
        {
            float yawInput = _playerInputActions.PlayerShip.Yaw.ReadValue<float>();
            transform.Rotate(Vector3.up, yawInput * baseRotationSpeed * Time.deltaTime, Space.World);
        }

        private void RotateRoll()
        {
            float rollInput = _playerInputActions.PlayerShip.Roll.ReadValue<float>();
            transform.Rotate(Vector3.forward, rollInput * baseRotationSpeed * Time.deltaTime, Space.Self);
        }

        private void RotatePitch()
        {
            float pitchInput = _playerInputActions.PlayerShip.Pitch.ReadValue<float>();
            transform.Rotate(Vector3.right, -pitchInput * baseRotationSpeed * Time.deltaTime, Space.Self);
        }
    }
}
