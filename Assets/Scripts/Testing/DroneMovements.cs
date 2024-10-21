using Player;
using UnityEngine;

namespace Testing
{
    
    public class DroneMovements : MonoBehaviour
    {
         public static DroneMovements Instance;

        [SerializeField] private float thrustSpeed = 50f;
        [SerializeField] private float rotationSpeed = 2f;
        [SerializeField] private float boostMultiplier = 2f;
        [SerializeField] private Transform lookTarget; 
        

        
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
            
            _playerInputActions.PlayerShip.Enable();
        }

        private void FixedUpdate()
        {
            if (_playerInputActions.PlayerShip.Thrust.IsPressed())
            {
                float thrustInput = _playerInputActions.PlayerShip.Thrust.ReadValue<float>();
                MoveThrust(thrustInput);
            }

            if (_playerInputActions.PlayerShip.Yaw.IsPressed())
            {
                float yawInput = _playerInputActions.PlayerShip.Yaw.ReadValue<float>();
                RotateYaw(yawInput);
            }

            
            Vector3 directionToTarget = (lookTarget.position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            
        }

        private void MoveThrust(float thurstInput)
        {
            if (thurstInput != 0)
            {
                float currentSpeed = thrustSpeed * (thurstInput > 0 ? boostMultiplier : 1f);
                transform.Translate( thurstInput * currentSpeed * Time.deltaTime * Vector3.forward);
            }
        }
        private void RotateYaw(float yawInput)
        {
            if (yawInput != 0)
            {
                Vector3 directionToTarget = transform.position - lookTarget.position;

                float yawRotation = yawInput * rotationSpeed * Time.deltaTime;

                directionToTarget = Quaternion.Euler(0, yawRotation*-1, 0) * directionToTarget;

                transform.position = lookTarget.position + directionToTarget;

                transform.LookAt(lookTarget);
            }
        }


    }

}
