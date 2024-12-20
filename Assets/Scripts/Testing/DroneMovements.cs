using Player;
using UnityEngine;

namespace Testing
{
    [RequireComponent(typeof(Rigidbody))]
    public class DroneMovements : MonoBehaviour
    {
        public static DroneMovements Instance;

        [SerializeField] private float thrustSpeed = 50f;
        [SerializeField] private float rotationSpeed = 2f;
        [SerializeField] private float boostMultiplier = 2f;
        [SerializeField] private float dodgeSpeed = 5f;
        [SerializeField] private Transform lookTarget;

        private bool isDodging = false;
        private Rigidbody _rb;
        private PlayerInputActions _playerInputActions;

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
            _rb = GetComponent<Rigidbody>(); // Initialize the Rigidbody
            _playerInputActions.PlayerShip.Enable();
        }

        private void FixedUpdate()
        {
            // Handle thrust movement
            if (_playerInputActions.PlayerShip.Thrust.IsPressed())
            {
                float thrustInput = _playerInputActions.PlayerShip.Thrust.ReadValue<float>();
                MoveThrust(thrustInput);
            }

            // Handle yaw rotation
            if (_playerInputActions.PlayerShip.Yaw.IsPressed())
            {
                float yawInput = _playerInputActions.PlayerShip.Yaw.ReadValue<float>();
                RotateYaw(yawInput);
            }

            // Rotate towards the look target
            Vector3 directionToTarget = (lookTarget.position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Handle dodge movement
            if (!isDodging && _playerInputActions.PlayerShip.Roll.IsPressed())
            {
                float dodgeDirection = _playerInputActions.PlayerShip.Roll.ReadValue<float>();
                if (dodgeDirection > 0)
                {
                    Dodge(Vector3.left);
                }
                else if (dodgeDirection < 0)
                {
                    Dodge(Vector3.right);
                }
            }
        }

        private void MoveThrust(float thrustInput)
        {
            if (thrustInput != 0)
            {
                float currentSpeed = thrustSpeed * (thrustInput > 0 ? boostMultiplier : 1f);
                transform.Translate(thrustInput * currentSpeed * Time.deltaTime * Vector3.forward);
            }
        }

        private void RotateYaw(float yawInput)
        {
            if (yawInput != 0)
            {
                Vector3 directionToTarget = transform.position - lookTarget.position;

                float yawRotation = yawInput * rotationSpeed * Time.deltaTime;

                directionToTarget = Quaternion.Euler(0, yawRotation * -1, 0) * directionToTarget;

                transform.position = lookTarget.position + directionToTarget;

                transform.LookAt(lookTarget);
            }
        }

        private void Dodge(Vector3 direction)
        {
            isDodging = true;
            _rb.AddRelativeForce(direction * dodgeSpeed, ForceMode.Impulse);
            Invoke(nameof(EndDodge), 0.1f);
        }

        private void EndDodge()
        {
            isDodging = false;
        }
    }
}
